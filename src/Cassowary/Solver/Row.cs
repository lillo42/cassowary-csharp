using System.Diagnostics;
using Cassowary.Extensions;

namespace Cassowary.Solver;

internal class Row
{
    public Row(Dictionary<Symbol, float> cells, float constant)
    {
        Cells = cells;
        Constant = constant;
    }

    public Row(float constant)
        : this(new(), constant)
    {
    }

    public Dictionary<Symbol, float> Cells { get; }
    public float Constant { get; private set; }

    public float Add(float value)
    {
        Constant += value;
        return Constant;
    }

    public void Add(Symbol symbol, float coefficient)
    {
        var existingCoefficient = Cells.GetValueOrDefault(symbol);
        existingCoefficient += coefficient;

        Cells.Remove(symbol);
        if (!existingCoefficient.IsNearZero())
        {
            Cells.Add(symbol, coefficient);
        }
    }

    public bool Add(Row other, float coefficient)
    {
        var diff = other.Constant * coefficient;
        Constant += diff;

        foreach (var (symbol, value) in other.Cells)
        {
            Add(symbol, value * coefficient);
        }

        return diff != 0;
    }

    public void Remove(Symbol symbol) => Cells.Remove(symbol);

    public void ReverseSign()
    {
        Constant = -Constant;
        foreach (var (symbol, value) in Cells)
        {
            Cells[symbol] = -value;
        }
    }

    public void SolveForSymbol(Symbol symbol)
    {
        if (!Cells.TryGetValue(symbol, out var existenceCoefficient))
        {
            throw new UnreachableException("Symbol not found in row");
        }

        Cells.Remove(symbol);

        var coefficient = -1 / existenceCoefficient;
        Constant *= coefficient;
        foreach (var (key, value) in Cells)
        {
            Cells[key] *= coefficient;
        }
    }

    public void SolveForSymbol(Symbol lhs, Symbol rhs)
    {
        Add(lhs, -1);
        SolveForSymbol(rhs);
    }

    public float CoefficientFor(Symbol symbol) => Cells.GetValueOrDefault(symbol);

    public bool Substitute(Symbol symbol, Row row)
    {
        if (!Cells.TryGetValue(symbol, out var coefficient))
        {
            return false;
        }

        Cells.Remove(symbol);
        Add(row, coefficient);
        return true;
    }
}
