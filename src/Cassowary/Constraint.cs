namespace Cassowary;

/// <summary>
/// A constraint, consisting of an equation governed by an expression and a relational operator,
/// and an associated strength.
/// </summary>
public readonly record struct Constraint
{
    private readonly ConstraintData _data;
    public Constraint(Expression expression, RelationalOperator relationalOperator, float strength)
    {
        _data = new ConstraintData(expression, strength, relationalOperator);
    }

    /// <summary>
    /// The expression of the left hand side of the constraint equation.
    /// </summary>
    public Expression Expression => _data.Expression;

    /// <summary>
    /// The relational operator governing the constraint.
    /// </summary>
    public RelationalOperator Operator => _data.Operator;

    /// <summary>
    /// The strength of the constraint that the solver will use.
    /// </summary>
    public float Strength => _data.Strength;


    public static Constraint From(Expression expression, WeightedRelation relation, Variable variable) 
        => new PartialConstraint(expression, relation).ToConstraint(variable);
    
    public static Constraint From(Variable variable, WeightedRelation relation, float strength) 
        => PartialConstraint.From(variable, relation).ToConstraint(strength);
    
    public static Constraint From(Variable variable, WeightedRelation relation, Expression expression)
        => PartialConstraint.From(variable, relation).ToConstraint(expression);
}
