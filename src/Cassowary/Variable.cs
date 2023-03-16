using System.Collections.Immutable;
using System.Linq.Expressions;

namespace Cassowary;

/// <summary>
/// Identifies a variable for the constraint solver.
/// Each new variable is unique in the view of the solver, but copying or cloning the variable produces
/// a copy of the same variable.
/// </summary>
/// <param name="Size"></param>
public readonly record struct Variable(ulong Size)
{
    private static ulong s_nextId = 0;

    /// <summary>
    /// Initializes a new instance of the <see cref="Variable"/> struct.
    /// </summary>
    /// <returns>New instance <see cref="Variable"/>.</returns>
    public Variable()
        : this(CreateNextId())
    {
    }

    private static ulong CreateNextId()
    {
        var id = Interlocked.Increment(ref s_nextId);
        return id - 1;
    }

    // ADD
    public static Expression operator +(Variable variable, float value)
        => new(ImmutableArray<Term>.Empty.Add(new(variable, 1)), value);

    public static Expression operator +(float value, Variable variable)
        => variable + value;

    public static Expression operator +(Variable variable, double value)
        => variable + (float)value;

    public static Expression operator +(double value, Variable variable)
        => variable + (float)value;

    public static Expression operator +(Variable variable, Term term)
        => new(ImmutableArray<Term>.Empty
            .Add(new(variable, 1))
            .Add(term), 0);

    public static Expression operator +(Variable variable, Expression expression)
        => expression with { Terms = expression.Terms.Add(new(variable, 1)) };

    public static Expression operator +(Variable variable, Variable other)
        => new(ImmutableArray<Term>.Empty
            .Add(new(variable, 1))
            .Add(new(other, 1)), 0);

    // Sub
    public static Term operator -(Variable variable)
        => new(variable, -1);

    public static Expression operator -(Variable variable, float value)
        => new(ImmutableArray<Term>.Empty.Add(new(variable, 1)), -value);

    public static Expression operator -(Variable variable, double value)
        => variable - (float)value;

    public static Expression operator -(float value, Variable variable)
        => variable - value;

    public static Expression operator -(double value, Variable variable)
        => variable - (float)value;

    public static Expression operator -(Variable variable, Term term)
        => new(ImmutableArray<Term>.Empty
            .Add(new(variable, 1))
            .Add(-term), 0);

    public static Expression operator -(Variable variable, Expression expression)
    {
        var negate = -expression;
        return negate with { Terms = negate.Terms.Add(new(variable, 1)) };
    }

    // OR
    public static PartialConstraint operator |(Variable variable, WeightedRelation relation)
        => new(Expression.From(variable), relation);
    
    // Mul
    public static Term operator *(Variable variable, float value)
        => new(variable, value);
    
    public static Term operator *(Variable variable, double value)
        => variable * (float)value;
    
    public static Term operator *(float value, Variable variable)
        => variable * value;
    
    public static Term operator *(double value, Variable variable)
        => variable * (float)value;
    
    // Div
    public static Term operator /(Variable variable, float value)
        => new(variable, 1 / value);

    public static Term operator /(Variable variable, double value)
        => variable / (float)value;
}
