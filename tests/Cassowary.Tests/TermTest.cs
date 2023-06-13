using System.Collections.Immutable;
using AutoFixture;
using FluentAssertions;

namespace Cassowary.Tests;

public class TermTest
{
    private readonly Fixture _fixture = new();

    [Fact]
    public void Or()
    {
        var term = new Term(new Variable(), _fixture.Create<double>());
        var relation = _fixture.Create<WeightedRelation>();

        var partialConstraint = term | relation;
        partialConstraint.Expression.Constant.Should().Be(0);
        partialConstraint.Expression.Terms.Should().BeEquivalentTo(new[] { term });
        partialConstraint.Relation.Should().Be(relation);
    }

    [Fact]
    public void AddWithValue()
    {
        var term = new Term(new Variable(), _fixture.Create<double>());
        var value = _fixture.Create<double>();

        var expression = term + value;
        expression.Constant.Should().Be(value);
        expression.Terms.Should().BeEquivalentTo(new[] { term });

        expression = term + (float)value;
        expression.Constant.Should().Be(value);
        expression.Terms.Should().BeEquivalentTo(new[] { term });

        expression = (float)value + term;
        expression.Constant.Should().Be(value);
        expression.Terms.Should().BeEquivalentTo(new[] { term });

        expression = value + term;
        expression.Constant.Should().Be(value);
        expression.Terms.Should().BeEquivalentTo(new[] { term });
    }

    [Fact]
    public void AddWithTerm()
    {
        var term = new Term(new Variable(), _fixture.Create<double>());
        var otherTerm = new Term(new Variable(), _fixture.Create<double>());

        var expression = term + otherTerm;
        expression.Constant.Should().Be(0);
        expression.Terms.Should().BeEquivalentTo(new[] { term, otherTerm });
    }

    [Fact]
    public void Negate()
    {
        var term = new Term(new Variable(), _fixture.Create<double>());
        var negate = -term;
        negate.Coefficient.Should().Be(-term.Coefficient);
    }

    [Fact]
    public void SubWithValue()
    {
        var term = new Term(new Variable(), _fixture.Create<double>());
        var value = _fixture.Create<double>();

        var expression = term - value;
        expression.Constant.Should().Be(-value);
        expression.Terms.Should().BeEquivalentTo(new[] { term });

        expression = term - (float)value;
        expression.Constant.Should().Be(-value);
        expression.Terms.Should().BeEquivalentTo(new[] { term });

        expression = (float)value - term;
        expression.Constant.Should().Be(value);
        expression.Terms.Should().BeEquivalentTo(new[] { -term });

        expression = value - term;
        expression.Constant.Should().Be(value);
        expression.Terms.Should().BeEquivalentTo(new[] { -term });
    }

    [Fact]
    public void SubWithExpression()
    {
        var term = new Term(new Variable(), _fixture.Create<double>());
        var expression = new Expression(
            ImmutableArray<Term>.Empty.Add(new Term(new Variable(), _fixture.Create<double>())),
            _fixture.Create<double>());

        var negate = -expression;

        var sub = term - expression;
        sub.Constant.Should().Be(negate.Constant);
        sub.Terms.Should().BeEquivalentTo(negate.Terms.Add(term));
    }

    [Fact]
    public void MultiplyWithValue()
    {
        var term = new Term(new Variable(), _fixture.Create<double>());
        var value = _fixture.Create<double>();

        var otherTerm = term * value;
        otherTerm.Coefficient.Should().Be(term.Coefficient * value);

        otherTerm = term * (float)value;
        otherTerm.Coefficient.Should().Be(term.Coefficient * value);

        otherTerm = (float)value * term;
        otherTerm.Coefficient.Should().Be(term.Coefficient * value);

        otherTerm = value * term;
        otherTerm.Coefficient.Should().Be(term.Coefficient * value);
    }

    [Fact]
    public void DividingWithValue()
    {
        var term = new Term(new Variable(), _fixture.Create<double>());
        var value = _fixture.Create<double>();

        var otherTerm = term / value;
        otherTerm.Coefficient.Should().Be(term.Coefficient / value);
        
        otherTerm = term / (float)value;
        otherTerm.Coefficient.Should().Be(term.Coefficient / value);
    }
}
