using System.Collections.Immutable;
using Cassowary.Extensions;

namespace Cassowary;

/// <summary>
/// An expression that can be the left hand or right hand side of a constraint equation.
/// It is a linear combination of variables, i.e. a sum of variables weighted by coefficients, plus an optional constant.
/// </summary>
public readonly record struct Expression(ImmutableArray<Term> Terms, float Constant)
{
    /// <summary>
    /// Constructs an expression of the form _n_, where n is a constant real number, not a variable.
    /// </summary>
    /// <param name="constant">The constant value.</param>
    /// <returns>New instance of <see cref="Expression"/>.</returns>
    public static Expression From(float constant) => new(new(), constant);

    /// <summary>
    /// Constructs an expression from a single term. Forms an expression of the form _n x_
    /// where n is the coefficient, and x is the variable.
    /// </summary>
    /// <param name="term">The <see cref="Term"/>.</param>
    /// <returns>New instance of <see cref="Expression"/>.</returns>
    public static Expression From(Term term) => new(ImmutableArray<Term>.Empty.Add(term), 0);

    /// <summary>
    /// Constructs an expression from a single variable. Forms an expression of the form _x_
    /// </summary>
    /// <param name="variable">The <see cref="Variable"/>.</param>
    /// <returns>New instance of <see cref="Expression"/>.</returns>
    public static Expression From(Variable variable) => From(new Term(variable, 1));

    // Add
    public static Expression operator +(Expression expression, float value)
        => expression with { Constant = expression.Constant + value };

    public static Expression operator +(Expression expression, double value)
        => expression + (float)value;

    public static Expression operator +(float value, Expression expression)
        => expression + value;

    public static Expression operator +(double value, Expression expression)
        => expression + (float)value;

    public static Expression operator +(Expression expression, Expression other) 
        => new(expression.Terms.AddRange(other.Terms), expression.Constant + other.Constant);

    // Sub
    public static Expression operator -(Expression expression)
        => new(expression.Terms.ToImmutableArray(term => -term), -expression.Constant);

    public static Expression operator -(Expression expression, float value)
        => expression with { Constant = expression.Constant - value };

    public static Expression operator -(Expression expression, Term term)
        => expression with { Terms = expression.Terms.Add(-term) };

    public static Expression operator -(Expression expression, Variable variable)
        => expression with { Terms = expression.Terms.Add(new Term(variable, -1)) };

    public static Expression operator -(Expression expression, Expression other)
    {
        var negative = -other;
        return new(expression.Terms.AddRange(negative.Terms), expression.Constant + negative.Constant);
    }

    // OR
    public static PartialConstraint operator |(Expression expression, WeightedRelation relation)
        => new(expression, relation);

    // Mul
    public static Expression operator *(Expression expression, float value)
        => new(expression.Terms.ToImmutableArray(x => x * value),
            expression.Constant * value);

    public static Expression operator *(Expression expression, double value)
        => expression * (float)value;

    public static Expression operator *(float value, Expression expression)
        => expression * value;

    public static Expression operator *(double value, Expression expression)
        => expression * (float)value;

    // Div
    public static Expression operator /(Expression expression, float value)
        => new(expression.Terms.ToImmutableArray(term => term / value), expression.Constant / value);

    public static Expression operator /(Expression expression, double value)
        => expression * (float)value;
}
