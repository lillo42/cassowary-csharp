using FluentAssertions;
using Xunit.Abstractions;
using static Cassowary.Strength;
using static Cassowary.WeightedRelation;

namespace Cassowary.Tests;

public class Quadrilateral
{
    private readonly ITestOutputHelper _output;

    public Quadrilateral(ITestOutputHelper output)
    {
        _output = output;
    }

    [Fact]
    public void Test()
    {
        var (valueOf, update) = Values.NewValues(_output);

        var points = new[] { new Point(), new Point(), new Point(), new Point() };
        var pointStarts = new[] { (10, 10), (10, 200), (200, 200), (200, 10) };
        var midpoints = new[] { new Point(), new Point(), new Point(), new Point() };

        var solver = new Solver.Solver();

        var weight = 1f;
        const float multiplier = 2f;

        for (var i = 0; i < 4; i++)
        {
            solver.AddConstraints(
                points[i].X | Eq(Weak * weight) | pointStarts[i].Item1,
                points[i].Y | Eq(Weak * weight) | pointStarts[i].Item2);

            weight *= multiplier;
        }

        foreach (var (start, end) in new[] { (0, 1), (1, 2), (2, 3), (3, 0) })
        {
            solver.AddConstraints(
                midpoints[start].X | Eq(Required) | (points[start].X + points[end].X) / 2,
                midpoints[start].Y | Eq(Required) | (points[start].Y + points[end].Y) / 2
            );
        }

        solver.AddConstraints(
            points[0].X + 20 | LessOrEq(Strong) | points[2].X,
            points[0].X + 20 | LessOrEq(Strong) | points[3].X,
            
            points[1].X + 20 | LessOrEq(Strong) | points[2].X,
            points[1].X + 20 | LessOrEq(Strong) | points[3].X,
            
            points[0].Y + 20 | LessOrEq(Strong) | points[1].Y,
            points[0].Y + 20 | LessOrEq(Strong) | points[2].Y,
            
            points[3].Y + 20 | LessOrEq(Strong) | points[1].Y,
            points[3].Y + 20 | LessOrEq(Strong) | points[2].Y
        );

        foreach (var point in points)
        {
            solver.AddConstraints(
                point.X | GreaterOrEq(Required) | 0,
                point.Y | GreaterOrEq(Required) | 0,
                
                point.X | LessOrEq(Required) | 500,
                point.Y | LessOrEq(Required) | 500
            );
        }

        update(solver.FetchChanges());

        valueOf(midpoints[0].X).Should().Be(10);
        valueOf(midpoints[0].Y).Should().Be(105);

        valueOf(midpoints[1].X).Should().Be(105);
        valueOf(midpoints[1].Y).Should().Be(200);

        valueOf(midpoints[2].X).Should().Be(200);
        valueOf(midpoints[2].Y).Should().Be(105);

        valueOf(midpoints[3].X).Should().Be(105);
        valueOf(midpoints[3].Y).Should().Be(10);

        solver.AddEditVariable(points[2].X, Strong);
        solver.AddEditVariable(points[2].Y, Strong);
        solver.SuggestValue(points[2].X, 300);
        solver.SuggestValue(points[2].Y, 400);

        update(solver.FetchChanges());

        valueOf(points[0].X).Should().Be(10);
        valueOf(points[0].Y).Should().Be(10);

        valueOf(points[1].X).Should().Be(10);
        valueOf(points[1].Y).Should().Be(200);

        valueOf(points[2].X).Should().Be(300);
        valueOf(points[2].Y).Should().Be(400);

        valueOf(points[3].X).Should().Be(200);
        valueOf(points[3].Y).Should().Be(10);

        valueOf(midpoints[0].X).Should().Be(10);
        valueOf(midpoints[0].Y).Should().Be(105);

        valueOf(midpoints[1].X).Should().Be(155);
        valueOf(midpoints[1].Y).Should().Be(300);

        valueOf(midpoints[2].X).Should().Be(250);
        valueOf(midpoints[2].Y).Should().Be(205);

        valueOf(midpoints[3].X).Should().Be(105);
        valueOf(midpoints[3].Y).Should().Be(10);
    }
}

public record Point(Variable X, Variable Y)
{
    public Point()
        : this(new(), new())
    {
    }
}
