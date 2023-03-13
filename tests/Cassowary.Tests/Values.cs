namespace Cassowary.Tests;

public record Values(Dictionary<Variable, float> Variables)
{
    private float ValueOf(Variable variable)
        => Variables.GetValueOrDefault(variable, 0);

    private void Update(List<(Variable, float)> changes)
    {
        foreach (var (variable, value) in changes)
        {
            Variables[variable] = value;
        }
    }
    
    public static (Func<Variable, float>, Action<List<(Variable, float)>>) NewValues()
    {
        var values = new Values(new Dictionary<Variable, float>());
        
        var valueOf = new Func<Variable, float>(values.ValueOf);
        var update = new Action<List<(Variable, float)>>(values.Update);
        
        return (valueOf, update);
    }
}
