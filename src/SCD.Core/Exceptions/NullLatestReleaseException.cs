using System;
using System.Runtime.Serialization;

namespace SCD.Core.Exceptions;

internal class NullReleaseException : Exception
{
    public NullReleaseException()
    {
    }

    public NullReleaseException(string? message) : base(message)
    {
    }

    public NullReleaseException(string? message, Exception? innerException) : base(message, innerException)
    {
    }

    protected NullReleaseException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
}
