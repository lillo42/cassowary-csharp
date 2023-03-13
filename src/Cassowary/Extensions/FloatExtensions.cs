namespace Cassowary.Extensions;

internal static class FloatExtensions
{
    public static bool IsNearZero(this float value)
    {
        const float EPS = 1E-8f;
        if (value < 0)
        {
            return -value < EPS;
        }

        return value < EPS;
    }

    public static float Max(this float source, float other)
        => Math.Max(source, other);
    
    public static float Min(this float source, float other)
        => Math.Min(source, other);
}
