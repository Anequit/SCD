using System;
using System.Runtime.Serialization;

namespace SCD.Core.Exceptions;

public class NullContentLengthException : Exception
{
    public NullContentLengthException()
    {
    }

    public NullContentLengthException(string? message) : base(message)
    {
    }

    public NullContentLengthException(string? message, Exception? innerException) : base(message, innerException)
    {
    }

    protected NullContentLengthException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
}
