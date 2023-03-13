using FluentAssertions;
using static Cassowary.Constraint;
using static Cassowary.Strength;
using static Cassowary.WeightedRelation;

namespace Cassowary.Tests;

public class Quadrilateral
{
    [Fact]
    public void Test()
    {
        var (valueOf, update) = Values.NewValues();

        var points = new[] { new Point(), new Point(), new Point(), new Point() };

        var pointStarts = new[] { (10, 10), (10, 200), (200, 200), (200, 10) };

        var midpoints = new[] { new Point(), new Point(), new Point(), new Point() };

        var solver = new Solver.Solver();

        var weight = 1f;
        const float multiplier = 2f;

        for (var i = 0; i < 4; i++)
        {
            solver.AddConstraints(
                From(points[i].X, Eq(Weak * weight), pointStarts[i].Item1),
                From(points[i].Y, Eq(Weak * weight), pointStarts[i].Item2));

            weight *= multiplier;
        }

        foreach (var (start, end) in new[] { (0, 1), (1, 2), (2, 3), (3, 0) })
        {
            solver.AddConstraints(
                From(midpoints[start].X,
                    Eq(Required),
                    (points[start].X + points[end].X).Div(2)),
                From(midpoints[start].Y,
                    Eq(Required),
                    (points[start].Y + points[end].Y).Div(2))
            );
        }
        
        solver.AddConstraints(
            From(points[0].X + 20, LessOrEq(Strong), points[2].X),
            From(points[0].X + 20, LessOrEq(Strong), points[3].X),
            
            From(points[1].X + 20, LessOrEq(Strong), points[2].X),
            From(points[1].X + 20, LessOrEq(Strong), points[3].X),
            
            From(points[0].X + 20, LessOrEq(Strong), points[1].X),
            From(points[0].X + 20, LessOrEq(Strong), points[2].X),
            
            
            From(points[3].X + 20, LessOrEq(Strong), points[1].X),
            From(points[3].X + 20, LessOrEq(Strong), points[2].X)
            );

        foreach (var point in points)
        {
            solver.AddConstraints(
                From(point.X, GreaterOrEq(Required), 0),
                From(point.Y, GreaterOrEq(Required), 0),
                
                From(point.X, LessOrEq(Required), 500),
                From(point.Y, LessOrEq(Required), 500)
            );
        }

        update(solver.FetchChanges());
        
        valueOf(points[0].X).Should().Be(10);
        valueOf(points[0].Y).Should().Be(105);
        
        valueOf(points[1].X).Should().Be(105);
        valueOf(points[1].Y).Should().Be(200);
        
        valueOf(points[2].X).Should().Be(200);
        valueOf(points[2].Y).Should().Be(105);
        
        valueOf(points[3].X).Should().Be(105);
        valueOf(points[3].Y).Should().Be(10);
    }
}

public record Point(Variable X, Variable Y)
{
    public Point()
        : this(new(), new())
    {
    }
}
