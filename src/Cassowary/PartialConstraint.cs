namespace Cassowary;

/// <summary>
/// This is an intermediate type used in the syntactic sugar for specifying constraints. You should not use it
/// directly.
/// </summary>
/// <param name="Expression"></param>
/// <param name="Relation"></param>
public readonly record struct PartialConstraint(Expression Expression, WeightedRelation Relation)
{
    public Constraint ToConstraint(float strength)
        => new(Expression -strength, Relation.Operator, Relation.Strength);

    public Constraint ToConstraint(Expression expression)
        => new(Expression - expression, Relation.Operator, Relation.Strength);

    public static PartialConstraint From(Variable variable, WeightedRelation relation) =>
        new(Expression.From(variable), relation);

    public Constraint ToConstraint(Variable variable)
    {
        var expression = Expression - Expression.From(variable);
        return new(expression, Relation.Operator, Relation.Strength);
    }

    // OR
    public static Constraint operator |(PartialConstraint partial, float value)
        => new(partial.Expression - value, partial.Relation);
    
    public static Constraint operator |(PartialConstraint partial, double value)
        => new(partial.Expression - (float)value, partial.Relation);

    public static Constraint operator |(PartialConstraint partial, Variable variable)
        => new(partial.Expression - variable, partial.Relation);
    
    public static Constraint operator |(PartialConstraint partial, Term term)
        => new(partial.Expression - term, partial.Relation);
    
    public static Constraint operator |(PartialConstraint partial, Expression expression)
        => new(partial.Expression - expression, partial.Relation);
}
