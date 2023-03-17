using AutoFixture;
using FluentAssertions;

namespace Cassowary.Tests;

public class ConstraintTest
{
    private readonly Fixture _fixture = new();
    
    [Fact]
    public void Properties()
    {
        var relational = _fixture.Create<RelationalOperator>();
        var strength = _fixture.Create<float>();
        var expression = new Expression();
        var constraint = new Constraint(expression, relational, strength);
        
        constraint.Expression.Should().Be(expression);
        constraint.Operator.Should().Be(relational);
        constraint.Strength.Should().Be(strength);
    }
}
