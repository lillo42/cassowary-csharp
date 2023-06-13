using AutoFixture;
using FluentAssertions;

namespace Cassowary.Tests;

public class RowTest
{
    private readonly Fixture _fixture = new();

    [Fact]
    public void Add()
    {
        var value = _fixture.Create<double>();
        var row = new Row(value);

        var add = _fixture.Create<double>();

        var newValue = row.Add(add);
        newValue.Should().Be(value + add);
        row.Constant.Should().Be(value + add);
    }

    [Fact]
    public void AddSymbol_Should_HaveSymbol_When_CoefficientIsNotNearZero()
    {
        var symbol = _fixture.Create<Symbol>();
        const float coefficient = 100f;

        var row = new Row(_fixture.Create<double>());
        row.Add(symbol, coefficient);

        row.Cells.Should().ContainKey(symbol);
    }

    [Fact]
    public void AddSymbol_Should_NotHaveSymbol_When_CoefficientIsNearToZero()
    {
        var symbol = _fixture.Create<Symbol>();
        const float coefficient = 1E-9f;

        var row = new Row(_fixture.Create<double>());
        row.Add(symbol, coefficient);

        row.Cells.Should().NotContainKey(symbol);
    }

    [Fact]
    public void AddSymbol_Should_UpdateSymbol_When_CoefficientIsNotNearZero()
    {
        var symbol = _fixture.Create<Symbol>();
        const int coefficient = 100;

        var row = new Row(_fixture.Create<double>());
        row.Add(symbol, coefficient);
        row.Cells.Should().ContainKey(symbol);

        const int newCoefficient = 200;
        row.Add(symbol, newCoefficient);
        row.Cells.Should().ContainKey(symbol);
        row.Cells[symbol].Should().Be(newCoefficient + coefficient);
    }

    [Fact]
    public void AddSymbol_Should_RemoveSymbol_When_CoefficientIsNearZero()
    {
        var symbol = _fixture.Create<Symbol>();
        const int coefficient = 100;

        var row = new Row(_fixture.Create<double>());
        row.Add(symbol, coefficient);
        row.Cells.Should().ContainKey(symbol);

        const float newCoefficient = -100f;
        row.Add(symbol, newCoefficient);
        row.Cells.Should().NotContainKey(symbol);
    }
    
    [Fact]
    public void AddRow_Should_ReturnTrue_When_DiffIsNotZero()
    {
        var row = new Row(_fixture.Create<double>());
        var other = new Row(_fixture.Create<double>());
        other.Add(_fixture.Create<Symbol>(), 100);

        row.Add(other, _fixture.Create<double>()).Should().BeTrue();
        row.Cells.Should().NotBeEmpty();
    }
    
    [Fact]
    public void AddRow_Should_ReturnFalse_When_DiffIsZero()
    {
        var row = new Row(_fixture.Create<double>());
        var other = new Row(_fixture.Create<double>());

        row.Add(other, 0).Should().BeFalse();
    }
    
    [Fact]
    public void CoefficientFor_Should_BeZero_When_SymbolIsNotInRow()
    {
        var row = new Row(_fixture.Create<float>()); 
        row.CoefficientFor(_fixture.Create<Symbol>()).Should().Be(0);
    }
    
    [Fact]
    public void CoefficientFor_Should_BeCoefficient_When_SymbolIsInRow()
    {
        var symbol = _fixture.Create<Symbol>();
        const int coefficient = 100;

        var row = new Row(_fixture.Create<double>());
        row.Add(symbol, coefficient);

        row.CoefficientFor(symbol).Should().Be(coefficient);
    }
    
    [Fact]
    public void Remove_Should_DoNothing_When_SymbolIsNotInRow()
    {
        var row = new Row(_fixture.Create<double>());
        row.Remove(_fixture.Create<Symbol>());
    }
    
    [Fact]
    public void Remove_Should_RemoveSymbol_When_SymbolIsInRow()
    {
        var symbol = _fixture.Create<Symbol>();
        const int coefficient = 100;

        var row = new Row(_fixture.Create<double>());
        row.Add(symbol, coefficient);
        row.Cells.Should().ContainKey(symbol);

        row.Remove(symbol);
        row.Cells.Should().NotContainKey(symbol);
    }
    
    [Fact]
    public void ReverseSign_Should_ReverseConstant()
    {
        var row = new Row(_fixture.Create<double>());
        var symbol = _fixture.Create<Symbol>();
        row.Add(symbol, 100);
        var constant = row.Constant;

        row.ReverseSign();
        row.Constant.Should().Be(-constant);
        row.Cells[symbol].Should().Be(-100);
    }

    [Fact]
    public void SolveForSymbol()
    {
        var constant = _fixture.Create<double>();
        var row = new Row(constant);
        
        var symbol = _fixture.Create<Symbol>();
        row.Add(symbol, 100);
        row.Add(_fixture.Create<Symbol>(), 200);
        
        row.SolveForSymbol(symbol);
        
        row.Cells.Should().NotContainKey(symbol);
        row.Constant.Should().Be(constant * (-1 / 100d));
        row.Cells.Should().Match(x => x.All(y => y.Value == -2f));
    }
    
    [Fact]
    public void SolveForSymbols()
    {
        var constant = _fixture.Create<double>();
        var row = new Row(constant);
        
        var symbol1 = _fixture.Create<Symbol>();
        row.Add(symbol1, 100);
        
        var symbol2 = _fixture.Create<Symbol>();
        row.Add(symbol2, 200);
        
        row.SolveForSymbols(symbol1, symbol2);
        
        row.Cells.Should().ContainKey(symbol1);
        row.Cells.Should().NotContainKey(symbol2);
        row.Constant.Should().Be(constant * (-1 / 200d));
    }
    
    [Fact]
    public void Substitute_Should_DoNothing_When_SymbolIsNotInRow()
    {
        var row = new Row(_fixture.Create<double>());
        row.Substitute(_fixture.Create<Symbol>(), _fixture.Create<Row>());
    }
    
    [Fact]
    public void Substitute_Should_SubstituteSymbol_When_SymbolIsInRow()
    {
        var symbol = _fixture.Create<Symbol>();
        const int coefficient = 100;

        var row = new Row(_fixture.Create<double>());
        row.Add(symbol, coefficient);
        row.Cells.Should().ContainKey(symbol);

        var other = new Row(_fixture.Create<double>());
        other.Add(_fixture.Create<Symbol>(), 200);
        row.Substitute(symbol, other);

        row.Cells.Should().NotContainKey(symbol);
        row.Cells.Should().Match(x => x.All(y => y.Value == 20000));
    }
}
