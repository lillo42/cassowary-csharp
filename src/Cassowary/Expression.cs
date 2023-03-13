namespace Cassowary;

/// <summary>
/// An expression that can be the left hand or right hand side of a constraint equation.
/// It is a linear combination of variables, i.e. a sum of variables weighted by coefficients, plus an optional constant.
/// </summary>
public record Expression(List<Term> Terms, float Constant)
{
    /// <summary>
    /// The terms in the expression.
    /// </summary>
    public List<Term> Terms { get; } = Terms;

    /// <summary>
    /// The constant in the expression.
    /// </summary>
    public float Constant { get; set; } = Constant;

    /// <summary>
    /// Mutates this expression by multiplying it by minus one.
    /// </summary>
    public void Negative()
    {
        Constant = -Constant;
        for (var i = 0; i < Terms.Count; i++)
        {
            var (variable, coefficient) = Terms[i];
            Terms[i] = -Terms[i];
        }
    }

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
    public static Expression From(Term term) => new(new() { term }, 0);

    /// <summary>
    /// Constructs an expression from a single variable. Forms an expression of the form _x_
    /// </summary>
    /// <param name="variable">The <see cref="Variable"/>.</param>
    /// <returns>New instance of <see cref="Expression"/>.</returns>
    public static Expression From(Variable variable) => From(new Term(variable, 1));

    public Expression Div(float value)
    {
        Constant /= value;
        for (var index = 0; index < Terms.Count; index++)
        {
            var term = Terms[index];
            Terms[index] = term / value;
        }

        return this;
    }

    public Expression Sub(float value)
    {
        Constant -= value;
        return this;
    }
    
    public Expression Sub(Expression expression)
    {
        expression.Negative();
        expression.Terms.AddRange(expression.Terms);
        expression.Constant += expression.Constant;
        return this;
    }

    public Expression Sub(Variable variable)
    {
        Terms.Add(new Term(variable, -1));
        return this;
    }
}
