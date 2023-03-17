using FluentAssertions;

namespace Cassowary.Tests;

public class StrengthTest
{
    [Theory]
    [InlineData(0, 0)]
    [InlineData(1, 1)]
    [InlineData(-1, 0)]
    [InlineData(Strength.Required + 1, Strength.Required)]
    public void Clip(float strength, float expected)
    {
        var result = Strength.Clip(strength);
        result.Should().Be(expected);
    }

    [Theory]
    [InlineData(0, 0, 0, 0, 0)] 
    [InlineData(1, 1, 1, 1, 1_001_001)]
    [InlineData(-1, 1, 1, 1, 1_001)]
    [InlineData(-1, -1, 1, 1, 1)]
    [InlineData(-1, -1, -1, 1, 0)]
    [InlineData(-1, -1, -1, -1, 1_001_001)]
    public void Create(float a, float b, float c, float w, float expected)
    {
        var result = Strength.Create(a, b, c, w);
        result.Should().Be(expected);
    }
}
