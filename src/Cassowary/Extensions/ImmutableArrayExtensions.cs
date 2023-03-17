using System.Collections.Immutable;

namespace Cassowary.Extensions;

internal static class ImmutableArrayExtensions
{
    public static ImmutableArray<U> ToImmutableArray<T, U>(this ImmutableArray<T> source, Func<T, U> map)
    {
        var builder = ImmutableArray<U>.Empty.ToBuilder();
        for (var index = 0; index < source.Length; index++)
        {
            builder.Add(map(source[index]));
        }

        return builder.ToImmutable();
    }
}
