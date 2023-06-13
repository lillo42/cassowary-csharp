namespace Cassowary;

/// <summary>
/// A constraint, consisting of an equation governed by an expression and a relational operator,
/// and an associated strength.
/// </summary>
public readonly record struct Constraint
{
    private readonly ConstraintData _data;

    /// <summary>
    /// Initializes a new instance of the <see cref="Constraint"/>.
    /// </summary>
    /// <param name="expression">The <see cref="Cassowary.Expression"/>.</param>
    /// <param name="relation">The <see cref="WeightedRelation"/>.</param>
    public Constraint(Expression expression, WeightedRelation relation)
        : this(expression, relation.Operator, relation.Strength)
    {
        
    }
    
    /// <summary>
    /// Initializes a new instance of the <see cref="Constraint"/>.
    /// </summary>
    /// <param name="expression">The <see cref="Cassowary.Expression"/>.</param>
    /// <param name="relationalOperator">The <see cref="RelationalOperator"/>.</param>
    /// <param name="strength">The strength.</param>
    public Constraint(Expression expression, RelationalOperator relationalOperator, double strength)
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
    public double Strength => _data.Strength;
}
