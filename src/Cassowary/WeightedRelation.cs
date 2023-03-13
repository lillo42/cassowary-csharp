namespace Cassowary;

/// <summary>
/// This is part of the syntactic sugar used for specifying constraints. This enum should be used as part of a
/// constraint expression. See the module documentation for more information.
/// </summary>
public readonly record struct WeightedRelation(RelationalOperator Operator, float Strength)
{
    /// <summary>
    /// ==
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static WeightedRelation Eq(float value) => new(RelationalOperator.Equal, value);

    /// <summary>
    /// Less than or equal to. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static WeightedRelation LessOrEq(float value) => new(RelationalOperator.LessThanOrEqual, value);

    /// <summary>
    /// >= 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static WeightedRelation GreaterOrEq(float value) => new(RelationalOperator.GreaterThanOrEqual, value);

    public static PartialConstraint operator |(float value, WeightedRelation relation) 
        => new(Expression.From(value), relation);
    
    public static PartialConstraint operator |(double value, WeightedRelation relation) 
        => (float)value | relation;
}
