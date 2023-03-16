using System.Diagnostics;
using Cassowary.Extensions;

namespace Cassowary.Solver;

internal record Row(Dictionary<Symbol, float> Cells, float Constant)
{
    public Row(float constant)
        : this(new(), constant)
    {
    }

    public float Constant { get; private set; } = Constant;

    public float Add(float value)
    {
        Constant += value;
        return Constant;
    }

    public void Add(Symbol symbol, float coefficient)
    {
        if (Cells.TryGetValue(symbol, out var entry))
        {
            entry += coefficient;
            Cells[symbol] = entry;
            if (entry.IsNearZero())
            {
                Cells.Remove(symbol);
            }
        }
        else if (!coefficient.IsNearZero())
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
        if (!Cells.TryGetValue(symbol, out var coeff))
        {
            throw new UnreachableException("Symbol not found in row");
        }

        Cells.Remove(symbol);

        var coefficient = -1 / coeff;
        Constant *= coefficient;
        foreach (var (key, value) in Cells)
        {
            Cells[key] = value * coefficient;
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
        if (Cells.TryGetValue(symbol, out var coefficient))
        {
            Cells.Remove(symbol);
            return Add(row, coefficient);
        }

        return false;
    }
}
