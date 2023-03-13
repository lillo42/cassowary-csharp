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
        => new(Expression.Sub(strength), Relation.Operator, Relation.Strength);

    public Constraint ToConstraint(Expression expression)
        => new(Expression.Sub(expression), Relation.Operator, Relation.Strength);

    public static PartialConstraint From(Variable variable, WeightedRelation relation) =>
        new(Expression.From(variable), relation);

    public Constraint ToConstraint(Variable variable)
    {
        var expression = Expression.Sub(Expression.From(variable));
        return new(expression, Relation.Operator, Relation.Strength);
    } 
}
