using System.Collections.Immutable;
using AutoFixture;
using FluentAssertions;

namespace Cassowary.Tests;

public class PartialConstraintTest
{
    private readonly Fixture _fixture = new();

    [Fact]
    public void OrWithValue()
    {
        var partial = new PartialConstraint(new Expression(
                ImmutableArray<Term>.Empty.Add(new Term(new Variable(), _fixture.Create<float>())),
                _fixture.Create<float>()),
            _fixture.Create<WeightedRelation>());

        var value = _fixture.Create<float>();

        var constraint = partial | value;
        constraint.Expression.Constant.Should().Be(partial.Expression.Constant - value);
        constraint.Expression.Terms.Should().BeEquivalentTo(partial.Expression.Terms);

        constraint = partial | (double)value;
        constraint.Expression.Constant.Should().Be(partial.Expression.Constant - value);
        constraint.Expression.Terms.Should().BeEquivalentTo(partial.Expression.Terms);

        constraint = value | partial;
        constraint.Expression.Constant.Should().Be(partial.Expression.Constant - value);
        constraint.Expression.Terms.Should().BeEquivalentTo(partial.Expression.Terms);

        constraint = (double)value | partial;
        constraint.Expression.Constant.Should().Be(partial.Expression.Constant - value);
        constraint.Expression.Terms.Should().BeEquivalentTo(partial.Expression.Terms);
    }

    [Fact]
    public void OrWithVariable()
    {
        var partial = new PartialConstraint(new Expression(
                ImmutableArray<Term>.Empty.Add(new Term(new Variable(), _fixture.Create<float>())),
                _fixture.Create<float>()),
            _fixture.Create<WeightedRelation>());

        var variable = new Variable();
        
        var constraint = partial | variable;
        constraint.Expression.Constant.Should().Be(partial.Expression.Constant);
        constraint.Expression.Terms.Should().BeEquivalentTo(partial.Expression.Terms.Add(new Term(variable, -1)));
    }
    
    [Fact]
    public void OrWithExpression()
    {
        var partial = new PartialConstraint(new Expression(
                ImmutableArray<Term>.Empty.Add(new Term(new Variable(), _fixture.Create<float>())),
                _fixture.Create<float>()),
            _fixture.Create<WeightedRelation>());

        var expression = new Expression(
            ImmutableArray<Term>.Empty.Add(new Term(new Variable(), _fixture.Create<float>())),
            _fixture.Create<float>());

        var constraint = partial | expression;
        constraint.Expression.Constant.Should().Be(partial.Expression.Constant - expression.Constant);
        constraint.Expression.Terms.Should().BeEquivalentTo(partial.Expression.Terms.Add(-expression.Terms[0]));
    }
    
    [Fact]
    public void OrWithTerm()
    {
        var partial = new PartialConstraint(new Expression(
                ImmutableArray<Term>.Empty.Add(new Term(new Variable(), _fixture.Create<float>())),
                _fixture.Create<float>()),
            _fixture.Create<WeightedRelation>());

        var term = new Term(new Variable(), _fixture.Create<float>());

        var constraint = partial | term;
        constraint.Expression.Constant.Should().Be(partial.Expression.Constant);
        constraint.Expression.Terms.Should().BeEquivalentTo(partial.Expression.Terms.Add(-term));
    }
}
