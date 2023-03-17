using FluentAssertions;
using Xunit.Abstractions;
using static Cassowary.Strength;
using static Cassowary.WeightedRelation;

namespace Cassowary.Tests;

public class Removal
{
    private readonly ITestOutputHelper _output;

    public Removal(ITestOutputHelper output)
    {
        _output = output;
    }

    [Fact]
    public void RemoveConstraint()
    {
        var (valueOf, update) = Values.NewValues(_output);

        var solver = new Solver.Solver();

        var val = new Variable();

        var constraint = val | Eq(Required) | 100;

        solver.AddConstraint(constraint);
        update(solver.FetchChanges());
        
        valueOf(val).Should().Be(100);
        
        solver.RemoveConstraint(constraint);
        solver.AddConstraint(val | Eq(Required) | 0);
        update(solver.FetchChanges());
        
        valueOf(val).Should().Be(0);
    }
}
