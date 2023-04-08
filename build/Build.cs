using System.Linq;
using Nuke.Common;
using Nuke.Common.CI;
using Nuke.Common.CI.GitHubActions;
using Nuke.Common.Execution;
using Nuke.Common.Git;
using Nuke.Common.IO;
using Nuke.Common.ProjectModel;
using Nuke.Common.Tooling;
using Nuke.Common.Tools.Coverlet;
using Nuke.Common.Tools.DotNet;
using Nuke.Common.Tools.GitVersion;
using Nuke.Common.Utilities.Collections;
using static Nuke.Common.IO.FileSystemTasks;
using static Nuke.Common.Tools.DotNet.DotNetTasks;

[DotNetVerbosityMapping]
[UnsetVisualStudioEnvironmentVariables]
[ShutdownDotNetAfterServerBuild]
class Build : NukeBuild
{
    /// Support plugins are available for:
    ///   - JetBrains ReSharper        https://nuke.build/resharper
    ///   - JetBrains Rider            https://nuke.build/rider
    ///   - Microsoft VisualStudio     https://nuke.build/visualstudio
    ///   - Microsoft VSCode           https://nuke.build/vscode
    public static int Main() => Execute<Build>(x => x.Compile);

    [Parameter("Configuration to build - Default is 'Debug' (local) or 'Release' (server)")]
    readonly Configuration Configuration = IsLocalBuild ? Configuration.Debug : Configuration.Release;

    [Parameter("Nuget API key", Name = "api-key")] readonly string NugetApiKey;

    [Parameter("NuGet Source for Packages", Name = "nuget-source")]
    readonly string NugetSource = "https://api.nuget.org/v3/index.json";

    [Solution] readonly Solution Solution;
    [GitRepository] readonly GitRepository GitRepository;
    [CI] GitHubActions GitHubActions;

    string Version
    {
        get
        {
            if(GitHubActions != null && GitHubActions.Ref != null && GitHubActions.Ref.StartsWith("refs/tags/"))
            {
                return GitHubActions.Ref.Replace("refs/tags/v", "");
            }
            
            if(GitRepository.Tags is { Count: > 0 })
            {
                var lastTag = GitRepository.Tags.Last().Split('.');
                int.TryParse(lastTag[^1], out var lastDigit);
                return string.Join('.', lastTag[..^1]) + (lastDigit + 1);
            }

            return "0.0.0";
        }
    }
    
    AbsolutePath SourceDirectory => RootDirectory / "src";
    AbsolutePath TestsDirectory => RootDirectory / "tests";
    AbsolutePath ArtifactsDirectory => RootDirectory / "artifacts";

    AbsolutePath PackageDirectory => ArtifactsDirectory / "packages";

    AbsolutePath TestResultDirectory => ArtifactsDirectory / "test-results";

    AbsolutePath CoverageReportDirectory => ArtifactsDirectory / "coverage-report";

    Target Clean => _ => _
        .Before(Restore)
        .Executes(() =>
        {
            DotNetClean(s => s
                .SetProject(Solution)
                .SetConfiguration(Configuration));
            SourceDirectory.GlobDirectories("**/bin", "**/obj").ForEach(DeleteDirectory);
            TestsDirectory.GlobDirectories("**/bin", "**/obj").ForEach(DeleteDirectory);
            EnsureCleanDirectory(ArtifactsDirectory);
        });

    Target Restore => _ => _
        .After(Clean)
        .Executes(() =>
        {
            DotNetRestore(s => s
                .SetProjectFile(Solution));
        });

    Target Compile => _ => _
        .DependsOn(Restore)
        .DependsOn(Clean)
        .Executes(() =>
        {
            DotNetBuild(s => s
                .SetNoRestore(InvokedTargets.Contains(Restore))
                .SetProjectFile(Solution)
                .SetConfiguration(Configuration)
                .SetAssemblyVersion(Version)
                .SetFileVersion(Version)
                .SetInformationalVersion(Version + ".Branch." + GitRepository.Branch + ".Sha." + GitRepository.Head));
        });

    Target Coverage => _ => _
        .Produces(CoverageReportDirectory)
        .Executes(() => { });

    Target Tests => _ => _
        .DependsOn(Compile)
        .Produces(TestResultDirectory / "*.trx")
        .Produces(TestResultDirectory / "*.xml")
        .Executes(() =>
        {
            DotNetTest(s => s
                .SetProjectFile(Solution)
                .SetNoBuild(InvokedTargets.Contains(Compile))
                .SetResultsDirectory(TestResultDirectory)
                .SetConfiguration(Configuration)
                .EnableNoRestore()
                .When(InvokedTargets.Contains(Coverage) || IsServerBuild, _ => _
                    .EnableCollectCoverage()
                    .SetCoverletOutputFormat(CoverletOutputFormat.opencover)
                    .When(IsServerBuild, _ => _.EnableUseSourceLink()))
                .CombineWith(Solution.GetProjects("*.Tests") , (_, v) => _
                    .SetProjectFile(v)
                    .SetLoggers($"trx;LogFileName={v.Name}.trx")
                    .SetCoverletOutput(TestResultDirectory / $"{v.Name}.xml")));
        });

    Target Pack => _ => _
        .DependsOn(Compile)
        .Produces(PackageDirectory / "*.nupkg")
        .Produces(PackageDirectory / "*.snupkg")
        .Executes(() =>
        {
            DotNetPack(s => s
                .SetProject(Solution)
                .SetNoBuild(InvokedTargets.Contains(Compile))
                .SetConfiguration(Configuration)
                .SetOutputDirectory(PackageDirectory)
                .SetVersion(Version)
                .EnableIncludeSource()
                .EnableIncludeSymbols()
                .EnableNoRestore());
        });

    Target Publish => _ => _
        .After(Pack)
        .Consumes(Pack)
        .Requires(() => NugetApiKey)
        .Requires(() => Configuration.Equals(Configuration.Release))
        .Executes(() =>
        {
           DotNetNuGetPush(s => s
                .SetSource(NugetSource)
                .SetApiKey(NugetApiKey)
                .EnableSkipDuplicate()
                .CombineWith(
                    PackageDirectory.GlobFiles("*.nupkg", "*.snupkg"),
                    (_, v) => _.SetTargetPath(v)));
        });
}
