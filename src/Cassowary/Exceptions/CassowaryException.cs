using System.Runtime.Serialization;

namespace Cassowary.Exceptions;

/// <summary>
/// Base class for all Cassowary exceptions.
/// </summary>
public abstract class CassowaryException : Exception
{
    /// <inheritdoc />
    protected CassowaryException()
    {
    }

    /// <inheritdoc />
    protected CassowaryException(string? message)
        : base(message)
    {
    }

    /// <inheritdoc />
    protected CassowaryException(string? message, Exception? innerException)
        : base(message, innerException)
    {
    }

    /// <inheritdoc />
    protected CassowaryException(SerializationInfo info, StreamingContext context)
        : base(info, context)
    {
    }
}
