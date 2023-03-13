namespace Cassowary;

/// <summary>
/// A variable and a coefficient to multiply that variable by. This is a sub-expression in
/// a constraint equation.
/// </summary>
public readonly record struct Term(Variable Variable, float Coefficient)
{
    public static Term operator /(Term term, float value)
        => term with { Coefficient = term.Coefficient / value };

    public static Term operator -(Term term) => term with { Coefficient = -1 };
}
