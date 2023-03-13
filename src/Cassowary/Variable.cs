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

    public static Expression operator +(Variable left, float right)
        => new(new List<Term> { new(left, 1) }, right);

    public static Expression operator +(Variable left, Variable right)
        => left.Add(right);

    public Expression Add(Variable other) => new(new List<Term> { new(this, 1), new(other, 1) }, 0);
}
