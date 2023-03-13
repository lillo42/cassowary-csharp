namespace Cassowary.Solver;

internal readonly record struct Symbol(int Size, SymbolType Type)
{
    public static Symbol Invalid => new(0, SymbolType.Invalid);
}
