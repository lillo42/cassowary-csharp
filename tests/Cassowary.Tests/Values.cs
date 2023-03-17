using Xunit.Abstractions;

namespace Cassowary.Tests;

public record Values(ITestOutputHelper Output, Dictionary<Variable, float> Variables)
{
    private float ValueOf(Variable variable)
        => Variables.GetValueOrDefault(variable, 0);

    private void Update(List<(Variable, float)> changes)
    {
        foreach (var (variable, value) in changes)
        {
            Output.WriteLine("{0} changed to {1}", variable, value);
            Variables[variable] = value;
        }
    }

    public static (Func<Variable, float>, Action<List<(Variable, float)>>) NewValues(ITestOutputHelper output)
    {
        var values = new Values(output, new Dictionary<Variable, float>());

        var valueOf = new Func<Variable, float>(values.ValueOf);
        var update = new Action<List<(Variable, float)>>(values.Update);

        return (valueOf, update);
    }
}
