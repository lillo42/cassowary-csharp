using Xunit.Abstractions;

namespace Cassowary.Tests;

public record Values(ITestOutputHelper Output, Dictionary<Variable, double> Variables)
{
    private double ValueOf(Variable variable)
        => Variables.GetValueOrDefault(variable, 0);

    private void Update(List<(Variable, double)> changes)
    {
        foreach (var (variable, value) in changes)
        {
            Output.WriteLine("{0} changed to {1}", variable, value);
            Variables[variable] = value;
        }
    }

    public static (Func<Variable, double>, Action<List<(Variable, double)>>) NewValues(ITestOutputHelper output)
    {
        var values = new Values(output, new Dictionary<Variable, double>());

        var valueOf = new Func<Variable, double>(values.ValueOf);
        var update = new Action<List<(Variable, double)>>(values.Update);

        return (valueOf, update);
    }
}
