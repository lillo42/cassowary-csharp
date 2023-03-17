using AutoFixture;
using FluentAssertions;

namespace Cassowary.Tests;

public class WeightedRelationTest
{
    private readonly Fixture _fixture = new();

    [Fact]
    public void Eq()
    {
        var strength = _fixture.Create<float>();
        var relation = WeightedRelation.Eq(strength);
        relation.Operator.Should().Be(RelationalOperator.Equal);
        relation.Strength.Should().Be(strength);
    }
    
    [Fact]
    public void LessOrEq()
    {
        var strength = _fixture.Create<float>();
        var relation = WeightedRelation.LessOrEq(strength);
        relation.Operator.Should().Be(RelationalOperator.LessThanOrEqual);
        relation.Strength.Should().Be(strength);
    }
    
    [Fact]
    public void GreaterOrEq()
    {
        var strength = _fixture.Create<float>();
        var relation = WeightedRelation.GreaterOrEq(strength);
        relation.Operator.Should().Be(RelationalOperator.GreaterThanOrEqual);
        relation.Strength.Should().Be(strength);
    }

    [Fact]
    public void Or()
    {
        var relation = _fixture.Create<WeightedRelation>();
        var strength = _fixture.Create<float>();
        
        var partialConstraint = strength | relation;
        partialConstraint.Expression.Should().Be(Expression.From(strength));
        partialConstraint.Relation.Should().Be(relation);
        
        partialConstraint = (double)strength | relation;
        partialConstraint.Expression.Should().Be(Expression.From(strength));
        partialConstraint.Relation.Should().Be(relation);
    }
}
