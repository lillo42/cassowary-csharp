using System.Collections.Immutable;
using AutoFixture;
using Cassowary.Extensions;
using FluentAssertions;

namespace Cassowary.Tests;

public class ExpressionTest
{
    private readonly Fixture _fixture = new();
    
    [Fact]
    public void ExpressionEqualsExpression()
    {
        var guid = Guid.NewGuid();
        var variable = new Variable(guid);
        (variable | WeightedRelation.Eq(1) | 5)
            .Should().Be(variable | WeightedRelation.Eq(1) | 5);
    }

    [Fact]
    public void FromConstant()
    {
        var constant = _fixture.Create<double>();
        var expression = Expression.From(constant);

        expression.Constant.Should().Be(constant);
        expression.Terms.Should().BeEmpty();
    }

    [Fact]
    public void FromTerm()
    {
        var term = new Term(new Variable(), _fixture.Create<double>());
        var expression = Expression.From(term);

        expression.Constant.Should().Be(0);
        expression.Terms.Should().HaveCount(1);
        expression.Terms.Should().Contain(term);
    }

    [Fact]
    public void FromVariable()
    {
        var variable = new Variable();
        var expression = Expression.From(variable);

        expression.Constant.Should().Be(0);
        expression.Terms.Should().HaveCount(1);
        expression.Terms.Should().Contain(new Term(variable, 1));
    }

    [Fact]
    public void SumExpressionWithValue()
    {
        var constant = _fixture.Create<double>();
        var expression = Expression.From(constant);

        constant = _fixture.Create<double>();

        var sum = expression + constant;
        sum.Constant.Should().Be(expression.Constant + constant);

        sum = constant + expression;
        sum.Constant.Should().Be(expression.Constant + constant);


        sum = expression + (float)constant;
        sum.Constant.Should().Be(expression.Constant + constant);

        sum = (float)constant + expression;
        sum.Constant.Should().Be(expression.Constant + constant);
    }

    [Fact]
    public void SumExpressionWithExpression()
    {
        var constant = _fixture.Create<double>();
        var expression = Expression.From(constant);

        var otherConstant = _fixture.Create<double>();
        var otherExpression = Expression.From(otherConstant);

        var sum = expression + otherExpression;
        sum.Constant.Should().Be(expression.Constant + otherExpression.Constant);
        sum.Terms.Should().BeEquivalentTo(expression.Terms.AddRange(otherExpression.Terms));
    }

    [Fact]
    public void NegateExpression()
    {
        var constant = _fixture.Create<double>();
        var expression = Expression.From(constant);

        var negated = -expression;
        negated.Constant.Should().Be(-expression.Constant);
        negated.Terms.Should().BeEquivalentTo(expression.Terms.ToImmutableArray(term => -term));
    }

    [Fact]
    public void SubExpressionWithValue()
    {
        var constant = _fixture.Create<double>();
        var expression = Expression.From(constant);

        constant = _fixture.Create<float>();

        var sub = expression - constant;
        sub.Constant.Should().Be(expression.Constant - constant);

        sub = expression - (float)constant;
        sub.Constant.Should().Be(expression.Constant - constant);

        var negate = -expression;
        sub = constant - expression;
        sub.Constant.Should().Be(negate.Constant + constant);

        sub = (float)constant - expression;
        sub.Constant.Should().Be(negate.Constant + constant);
    }

    [Fact]
    public void SubExpressionWithTerm()
    {
        var expression = Expression.From(_fixture.Create<double>());

        var term = new Term(new Variable(), _fixture.Create<double>());

        var sub = expression - term;
        sub.Terms.Should().HaveCount(1);
        sub.Terms.Should().Contain(-term);
    }

    [Fact]
    public void SubExpressionWithVariable()
    {
        var expression = Expression.From(_fixture.Create<double>());

        var variable = new Variable();

        var sub = expression - variable;
        sub.Terms.Should().HaveCount(1);
        sub.Terms.Should().Contain(new Term(variable, -1));
    }

    [Fact]
    public void SubExpressionWithExpression()
    {
        var expression = new Expression(ImmutableArray<Term>.Empty
            .Add(new Term(new Variable(), 1)), _fixture.Create<double>());

        var otherExpression = new Expression(ImmutableArray<Term>.Empty
            .Add(new Term(new Variable(), 2)), _fixture.Create<double>());


        var sub = expression - otherExpression;
        sub.Constant.Should().Be(expression.Constant - otherExpression.Constant);
        sub.Terms.Should()
            .BeEquivalentTo(expression.Terms.AddRange(otherExpression.Terms.ToImmutableArray(term => -term)));
    }

    [Fact]
    public void Or()
    {
        var expression = Expression.From(_fixture.Create<double>());
        var relation = _fixture.Create<WeightedRelation>();

        var partial = expression | relation;
        partial.Expression.Should().Be(expression);
        partial.Relation.Should().Be(relation);
    }

    [Fact]
    public void Multiply()
    {
        var expression = new Expression(ImmutableArray<Term>.Empty
            .Add(new(new Variable(), 2)), _fixture.Create<double>());

        var multiplier = _fixture.Create<double>();

        var product = expression * multiplier;
        product.Constant.Should().Be(expression.Constant * multiplier);
        product.Terms.Should().BeEquivalentTo(expression.Terms.ToImmutableArray(term => term * multiplier));

        product = expression * (float)multiplier;
        product.Constant.Should().Be(expression.Constant * multiplier);
        product.Terms.Should().BeEquivalentTo(expression.Terms.ToImmutableArray(term => term * multiplier));

        product = multiplier * expression;
        product.Constant.Should().Be(expression.Constant * multiplier);
        product.Terms.Should().BeEquivalentTo(expression.Terms.ToImmutableArray(term => term * multiplier));

        product = (float)multiplier * expression;
        product.Constant.Should().Be(expression.Constant * multiplier);
        product.Terms.Should().BeEquivalentTo(expression.Terms.ToImmutableArray(term => term * multiplier));
    }

    [Fact]
    public void Div()
    {
        var expression = new Expression(ImmutableArray<Term>.Empty
            .Add(new(new Variable(), 2)), _fixture.Create<double>());

        var divider = _fixture.Create<double>();

        var remaning = expression / divider;
        remaning.Constant.Should().Be(expression.Constant / divider);
        remaning.Terms.Should().BeEquivalentTo(expression.Terms.ToImmutableArray(term => term / divider));

        remaning = expression / (double)divider;
        remaning.Constant.Should().Be(expression.Constant / divider);
        remaning.Terms.Should().BeEquivalentTo(expression.Terms.ToImmutableArray(term => term / divider));
    }
}
