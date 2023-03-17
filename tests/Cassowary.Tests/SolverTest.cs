using AutoFixture;
using FluentAssertions;
using static Cassowary.Strength;
using static Cassowary.WeightedRelation;

namespace Cassowary.Tests;

public class SolverTest
{
    private readonly Fixture _fixture = new();
    private readonly Solver _solver = new();

    [Fact]
    public void AddConstraintsWithArray()
    {
        var constraint1 = new Variable() | Eq(Required) | 10;
        var constraint2 = new Variable() | Eq(Required) | 10;

        _solver.AddConstraints(constraint1, constraint2);

        _solver.HasConstraint(constraint1).Should().BeTrue();
        _solver.HasConstraint(constraint2).Should().BeTrue();
    }

    [Fact]
    public void AddConstraintsWithEnumerable()
    {
        var constraint1 = new Variable() | Eq(Required) | 10;
        var constraint2 = new Variable() | Eq(Required) | 10;

        _solver.AddConstraints(new List<Constraint> { constraint1, constraint2 });

        _solver.HasConstraint(constraint1).Should().BeTrue();
        _solver.HasConstraint(constraint2).Should().BeTrue();
    }

    [Fact]
    public void AddConstraint_Should_AddConstraint()
    {
        var constraint = new Variable() | Eq(Required) | 10;
        _solver.AddConstraint(constraint);
    }

    [Fact]
    public void AddConstraint_Should_Throw_When_AlreadyHaveConstraint()
    {
        var constraint = new Variable() | Eq(Required) | 10;
        _solver.AddConstraint(constraint);

        _solver.Invoking(solver => solver.AddConstraint(constraint))
            .Should().Throw<ArgumentException>();
    }
}
