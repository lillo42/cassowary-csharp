using System.Collections.Immutable;

namespace Cassowary;

/// <summary>
/// Identifies a variable for the constraint solver.
/// Each new variable is unique in the view of the solver, but copying or cloning the variable produces
/// a copy of the same variable.
/// </summary>
/// <param name="Id">The variable id.</param>
public readonly record struct Variable(Guid Id)
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Variable"/> struct.
    /// </summary>
    /// <returns>New instance <see cref="Variable"/>.</returns>
    public Variable()
        : this(Guid.NewGuid())
    {
    }

    #region operator +

    /// <summary>
    /// Add <paramref name="value"/> to <paramref name="variable"/> and return new <see cref="Expression"/>.
    /// </summary>
    /// <param name="variable">The <see cref="Variable"/>.</param>
    /// <param name="value">The value.</param>
    /// <returns>New <see cref="Expression"/> instance with <paramref name="variable"/> value <paramref name="value"/> as <see cref="Expression.Constant"/>.</returns>
    public static Expression operator +(Variable variable, float value)
        => variable + (double)value;
    /// <summary>
    /// Add <paramref name="value"/> to <paramref name="variable"/> and return new <see cref="Expression"/>.
    /// </summary>
    /// <param name="variable">The <see cref="Variable"/>.</param>
    /// <param name="value">The value.</param>
    /// <returns>New <see cref="Expression"/> instance with <paramref name="variable"/> value <paramref name="value"/> as <see cref="Expression.Constant"/>.</returns>
    public static Expression operator +(float value, Variable variable)
        => variable + value;

    /// <summary>
    /// Add <paramref name="value"/> to <paramref name="variable"/> and return new <see cref="Expression"/>.
    /// </summary>
    /// <param name="variable">The <see cref="Variable"/>.</param>
    /// <param name="value">The value.</param>
    /// <returns>New <see cref="Expression"/> instance with <paramref name="variable"/> value <paramref name="value"/> as <see cref="Expression.Constant"/>.</returns>
    public static Expression operator +(Variable variable, double value)
        => new(ImmutableArray.Create(new Term(variable, 1)), value);

    /// <summary>
    /// Add <paramref name="value"/> to <paramref name="variable"/> and return new <see cref="Expression"/>.
    /// </summary>
    /// <param name="variable">The <see cref="Variable"/>.</param>
    /// <param name="value">The value.</param>
    /// <returns>New <see cref="Expression"/> instance with <paramref name="variable"/> value <paramref name="value"/> as <see cref="Expression.Constant"/>.</returns>
    public static Expression operator +(double value, Variable variable)
        => variable + value;

    /// <summary>
    /// Add <paramref name="term"/> to <paramref name="variable"/> and return new <see cref="Expression"/>.
    /// </summary>
    /// <param name="variable">The <see cref="Variable"/>.</param>
    /// <param name="term">The <see cref="Term"/>.</param>
    /// <returns>New <see cref="Expression"/> instance with <paramref name="variable"/> and <paramref name="term"/> and <see cref="Expression.Constant"/> as 0.</returns>
    public static Expression operator +(Variable variable, Term term)
        => new(ImmutableArray.Create(term, new(variable, 1)), 0);

    /// <summary>
    /// Add <paramref name="expression"/> to <paramref name="variable"/> and return new <see cref="Expression"/>.
    /// </summary>
    /// <param name="variable">The <see cref="Variable"/>.</param>
    /// <param name="expression">The <see cref="Expression"/>.</param>
    /// <returns>New <see cref="Expression"/> instance with <paramref name="variable"/>.</returns>
    public static Expression operator +(Variable variable, Expression expression)
        => expression with { Terms = expression.Terms.Add(new(variable, 1)) };

    /// <summary>
    /// Add <paramref name="other"/> to <paramref name="variable"/> and return new <see cref="Expression"/>.
    /// </summary>
    /// <param name="variable">The <see cref="Variable"/>.</param>
    /// <param name="other">The <see cref="Variable"/>.</param>
    /// <returns>New <see cref="Expression"/> instance with both variables and constant as 0.</returns>
    public static Expression operator +(Variable variable, Variable other)
        => new(ImmutableArray.Create(new Term(variable, 1), new Term(other, 1)), 0);

    #endregion

    #region operator -

    /// <summary>
    /// Negate <paramref name="variable"/> and return new <see cref="Term"/>.
    /// </summary>
    /// <param name="variable">The <see cref="Variable"/>.</param>
    /// <returns>New <see cref="Term"/> instance with the <paramref name="variable"/> and <see cref="Term.Coefficient"/> as -1.</returns>
    public static Term operator -(Variable variable)
        => new(variable, -1);

    /// <summary>
    /// Subtract <paramref name="value"/> from <paramref name="variable"/> and return new <see cref="Expression"/>.
    /// </summary>
    /// <param name="variable">The <see cref="Variable"/>.</param>
    /// <param name="value">The value.</param>
    /// <returns>New <see cref="Expression"/> instance with <paramref name="variable"/> and negate <paramref name="value"/> as <see cref="Expression.Constant"/> .</returns>
    public static Expression operator -(Variable variable, float value)
        => variable - (double)value;

    /// <summary>
    /// Subtract <paramref name="value"/> from <paramref name="variable"/> and return new <see cref="Expression"/>.
    /// </summary>
    /// <param name="variable">The <see cref="Variable"/>.</param>
    /// <param name="value">The value.</param>
    /// <returns>New <see cref="Expression"/> instance with <paramref name="variable"/> and negate <paramref name="value"/> as <see cref="Expression.Constant"/> .</returns>
    public static Expression operator -(Variable variable, double value)
        => new(ImmutableArray.Create(new Term(variable, 1)), -value);

    /// <summary>
    /// Subtract <paramref name="value"/> from <paramref name="variable"/> and return new <see cref="Expression"/>.
    /// </summary>
    /// <param name="variable">The <see cref="Variable"/>.</param>
    /// <param name="value">The value.</param>
    /// <returns>New <see cref="Expression"/> instance with <paramref name="variable"/> and negate <paramref name="value"/> as <see cref="Expression.Constant"/> .</returns>
    public static Expression operator -(float value, Variable variable)
        => (double)value - variable;

    /// <summary>
    /// Subtract <paramref name="value"/> from <paramref name="variable"/> and return new <see cref="Expression"/>.
    /// </summary>
    /// <param name="variable">The <see cref="Variable"/>.</param>
    /// <param name="value">The value.</param>
    /// <returns>New <see cref="Expression"/> instance with <paramref name="variable"/> and negate <paramref name="value"/> as <see cref="Expression.Constant"/> .</returns>
    public static Expression operator -(double value, Variable variable)
        => new(ImmutableArray.Create(new Term(variable, -1)), value);

    /// <summary>
    /// Subtract <paramref name="term"/> from <paramref name="variable"/> and return new <see cref="Expression"/>.
    /// </summary>
    /// <param name="variable">The <see cref="Variable"/>.</param>
    /// <param name="term">The <see cref="Term"/>.</param>
    /// <returns>New <see cref="Expression"/> instance.</returns>
    public static Expression operator -(Variable variable, Term term)
        => new(ImmutableArray.Create(new Term(variable, 1), -term), 0);

    /// <summary>
    /// Subtract <paramref name="expression"/> from <paramref name="variable"/> and return new <see cref="Expression"/>.
    /// </summary>
    /// <param name="variable">The <see cref="Variable"/>.</param>
    /// <param name="expression">The <see cref="Expression"/>.</param>
    /// <returns>New <see cref="Expression"/> instance.</returns>
    public static Expression operator -(Variable variable, Expression expression)
    {
        var negate = -expression;
        return negate with { Terms = negate.Terms.Add(new(variable, 1)) };
    }

    #endregion

    #region operator |

    /// <summary>
    /// Or <paramref name="variable"/> with <paramref name="relation"/>.
    /// </summary>
    /// <param name="variable">The <see cref="Variable"/>.</param>
    /// <param name="relation">The <see cref="WeightedRelation"/>.</param>
    /// <returns>New <see cref="PartialConstraint"/> instance.</returns>
    public static PartialConstraint operator |(Variable variable, WeightedRelation relation)
        => new(Expression.From(variable), relation);

    #endregion

    #region operator *

    /// <summary>
    /// Multiply <paramref name="variable"/> with <paramref name="value"/> and return new <see cref="Term"/>.
    /// </summary>
    /// <param name="variable">The <see cref="Variable"/>.</param>
    /// <param name="value">The value.</param>
    /// <returns>New <see cref="Term"/> instance.</returns>
    public static Term operator *(Variable variable, float value)
        => variable * (double)value;

    /// <summary>
    /// Multiply <paramref name="variable"/> with <paramref name="value"/> and return new <see cref="Term"/>.
    /// </summary>
    /// <param name="variable">The <see cref="Variable"/>.</param>
    /// <param name="value">The value.</param>
    /// <returns>New <see cref="Term"/> instance.</returns>
    public static Term operator *(Variable variable, double value)
        => new(variable, value);

    /// <summary>
    /// Multiply <paramref name="variable"/> with <paramref name="value"/> and return new <see cref="Term"/>.
    /// </summary>
    /// <param name="variable">The <see cref="Variable"/>.</param>
    /// <param name="value">The value.</param>
    /// <returns>New <see cref="Term"/> instance.</returns>
    public static Term operator *(float value, Variable variable)
        => variable * value;

    /// <summary>
    /// Multiply <paramref name="variable"/> with <paramref name="value"/> and return new <see cref="Term"/>.
    /// </summary>
    /// <param name="variable">The <see cref="Variable"/>.</param>
    /// <param name="value">The value.</param>
    /// <returns>New <see cref="Term"/> instance.</returns>
    public static Term operator *(double value, Variable variable)
        => variable * value;

    #endregion

    #region operator /

    /// <summary>
    /// Divide <paramref name="variable"/> by <paramref name="value"/> and return new <see cref="Term"/>.
    /// </summary>
    /// <param name="variable">The <see cref="Variable"/>.</param>
    /// <param name="value">The value.</param>
    /// <returns>New <see cref="Term"/>.</returns>
    public static Term operator /(Variable variable, float value)
        => variable / (double)value;

    /// <summary>
    /// Divide <paramref name="variable"/> by <paramref name="value"/> and return new <see cref="Term"/>.
    /// </summary>
    /// <param name="variable">The <see cref="Variable"/>.</param>
    /// <param name="value">The value.</param>
    /// <returns>New <see cref="Term"/>.</returns>
    public static Term operator /(Variable variable, double value)
        => new(variable, 1 / value);

    #endregion
}
