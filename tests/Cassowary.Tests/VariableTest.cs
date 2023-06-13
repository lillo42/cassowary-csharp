using System.Collections.Immutable;
using AutoFixture;
using FluentAssertions;

namespace Cassowary.Tests;

public class VariableTest
{
    private readonly Fixture _fixture = new();

    [Fact]
    public void AddWithValue()
    {
        var variable = new Variable();
        var value = _fixture.Create<double>();

        var expression = variable + value;
        expression.Constant.Should().Be(value);
        expression.Terms.Should().BeEquivalentTo(new[] { new Term(variable, 1) });

        expression = variable + (float)value;
        expression.Constant.Should().Be(value);
        expression.Terms.Should().BeEquivalentTo(new[] { new Term(variable, 1) });

        expression = (float)value + variable;
        expression.Constant.Should().Be(value);
        expression.Terms.Should().BeEquivalentTo(new[] { new Term(variable, 1) });

        expression = value + variable;
        expression.Constant.Should().Be(value);
        expression.Terms.Should().BeEquivalentTo(new[] { new Term(variable, 1) });
    }

    [Fact]
    public void AddWithTerm()
    {
        var variable = new Variable();
        var term = new Term(new Variable(), _fixture.Create<double>());

        var expression = variable + term;
        expression.Constant.Should().Be(0);
        expression.Terms.Should().BeEquivalentTo(new[] { new Term(variable, 1), term });
    }

    [Fact]
    public void AddWithExpression()
    {
        var variable = new Variable();
        var expression = new Expression(
            ImmutableArray<Term>.Empty
                .Add(new Term(new Variable(), _fixture.Create<double>())),
            _fixture.Create<double>()
        );

        var otherExpression = variable + expression;
        otherExpression.Constant.Should().Be(expression.Constant);
        otherExpression.Terms.Should().BeEquivalentTo(expression.Terms.Add(new Term(variable, 1)));
    }

    [Fact]
    public void AddWithVariable()
    {
        var variable = new Variable();
        var otherVariable = new Variable();

        var expression = variable + otherVariable;
        expression.Constant.Should().Be(0);
        expression.Terms.Should().BeEquivalentTo(new[] { new Term(variable, 1), new Term(otherVariable, 1) });
    }

    [Fact]
    public void Negate()
    {
        var variable = new Variable();

        var term = -variable;
        term.Coefficient.Should().Be(-1);
        term.Variable.Should().Be(variable);
    }

    [Fact]
    public void SubtractWithValue()
    {
        var variable = new Variable();
        var value = _fixture.Create<double>();

        var expression = variable - value;
        expression.Constant.Should().Be(-value);
        expression.Terms.Should().BeEquivalentTo(new[] { new Term(variable, 1) });

        expression = variable - (float)value;
        expression.Constant.Should().Be(-value);
        expression.Terms.Should().BeEquivalentTo(new[] { new Term(variable, 1) });

        expression = (float)value - variable;
        expression.Constant.Should().Be(value);
        expression.Terms.Should().BeEquivalentTo(new[] { new Term(variable, -1) });

        expression = value - variable;
        expression.Constant.Should().Be(value);
        expression.Terms.Should().BeEquivalentTo(new[] { new Term(variable, -1) });
    }


    [Fact]
    public void SubtractWithTerm()
    {
        var variable = new Variable();
        var term = new Term(new Variable(), _fixture.Create<double>());

        var expression = variable - term;
        expression.Constant.Should().Be(0);
        expression.Terms.Should()
            .BeEquivalentTo(new[] { new Term(variable, 1), term with { Coefficient = -term.Coefficient } });
    }

    [Fact]
    public void SubtractWithExpression()
    {
        var variable = new Variable();
        var expression = new Expression(
            ImmutableArray<Term>.Empty
                .Add(new Term(new Variable(), _fixture.Create<double>())),
            _fixture.Create<float>()
        );

        var negate = -expression;
        var otherExpression = variable - expression;
        otherExpression.Constant.Should().Be(negate.Constant);
        otherExpression.Terms.Should().BeEquivalentTo(negate.Terms.Add(new Term(variable, 1)));
    }

    [Fact]
    public void Or()
    {
        var variable = new Variable();
        var relation = _fixture.Create<WeightedRelation>();

        var constraint = variable | relation;
        constraint.Relation.Should().Be(relation);
        constraint.Expression.Should().BeEquivalentTo(Expression.From(variable));
    }

    [Fact]
    public void MultiplyWithValue()
    {
        var variable = new Variable();
        var value = _fixture.Create<double>();

        var term = variable * value;
        term.Coefficient.Should().Be(value);
        term.Variable.Should().Be(variable);

        term = variable * (float)value;
        term.Coefficient.Should().Be(value);
        term.Variable.Should().Be(variable);

        term = (float)value * variable;
        term.Coefficient.Should().Be(value);
        term.Variable.Should().Be(variable);

        term = value * variable;
        term.Coefficient.Should().Be(value);
        term.Variable.Should().Be(variable);
    }

    [Fact]
    public void DivideWithValue()
    {
        var variable = new Variable();
        var value = _fixture.Create<double>();

        var term = variable / value;
        term.Coefficient.Should().Be(1 / value);
        term.Variable.Should().Be(variable);

        term = variable / (float)value;
        term.Coefficient.Should().Be(1 / value);
        term.Variable.Should().Be(variable);
    }
}
