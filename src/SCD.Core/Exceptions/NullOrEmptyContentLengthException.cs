using System;
using System.Runtime.Serialization;

namespace SCD.Core.Exceptions;

public class NullOrZeroContentLengthException : Exception
{
    public NullOrZeroContentLengthException()
    {
    }

    public NullOrZeroContentLengthException(string? message) : base(message)
    {
    }

    public NullOrZeroContentLengthException(string? message, Exception? innerException) : base(message, innerException)
    {
    }

    protected NullOrZeroContentLengthException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
}
