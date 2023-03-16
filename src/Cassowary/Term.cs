using System.Collections.Immutable;
using System.Linq.Expressions;

namespace Cassowary;

/// <summary>
/// A variable and a coefficient to multiply that variable by. This is a sub-expression in
/// a constraint equation.
/// </summary>
public readonly record struct Term(Variable Variable, float Coefficient)
{
    public static PartialConstraint operator |(Term term, WeightedRelation relation)
        => new(Expression.From(term), relation);

    // Add
    public static Expression operator +(Term term, float value)
        => new(ImmutableArray<Term>.Empty.Add(term), value);

    public static Expression operator +(Term term, double value)
        => term + (float)value;

    public static Expression operator +(float value, Term term)
        => term + value;

    public static Expression operator +(double value, Term term)
        => term + (float)value;

    public static Expression operator +(Term term, Term other)
        => new(ImmutableArray<Term>.Empty.Add(term).Add(other), 0);

    // Neg
    public static Term operator -(Term term) => term with { Coefficient = -term.Coefficient };

    public static Expression operator -(Term term, float value)
        => new(ImmutableArray<Term>.Empty.Add(term), -value);

    public static Expression operator -(Term term, double value)
        => term - (float)value;

    public static Expression operator -(float value, Term term)
        => term - value;

    public static Expression operator -(double value, Term term)
        => term - (float)value;

    public static Expression operator -(Term term, Expression expression)
    {
        var negate = -expression;
        return negate with { Terms = ImmutableArray<Term>.Empty.Add(term) };
    }

    // Mul
    public static Term operator *(Term term, float value)
        => term with { Coefficient = term.Coefficient * value };

    public static Term operator *(Term term, double value)
        => term * (float)value;

    public static Term operator *(float value, Term term)
        => term * value;

    public static Term operator *(double value, Term term)
        => term * (float)value;

    // Div
    public static Term operator /(Term term, float value)
        => term with { Coefficient = term.Coefficient / value };

    public static Term operator /(Term term, double value)
        => term / (float)value;
}
