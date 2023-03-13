using FluentAssertions;
using static Cassowary.Constraint;
using static Cassowary.Strength;
using static Cassowary.WeightedRelation;

namespace Cassowary.Tests;

public class Removal
{
    [Fact]
    public void RemoveConstraint()
    {
        var (valueOf, update) = Values.NewValues();

        var solver = new Solver.Solver();

        var val = new Variable();

        var constraint = From(val, Eq(Required), 100);

        solver.AddConstraint(constraint);
        update(solver.FetchChanges());
        
        valueOf(val).Should().Be(100);
        
        solver.RemoveConstraint(constraint);
        solver.AddConstraint(From(val, Eq(Required), 0));
        update(solver.FetchChanges());
        
        valueOf(val).Should().Be(0);
    }
}
