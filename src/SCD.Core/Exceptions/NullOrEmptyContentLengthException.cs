using System;
using System.Runtime.Serialization;

namespace SCD.Core.Exceptions;

public class NullOrEmptyContentLengthException : Exception
{
    public NullOrEmptyContentLengthException()
    {
    }

    public NullOrEmptyContentLengthException(string? message) : base(message)
    {
    }

    public NullOrEmptyContentLengthException(string? message, Exception? innerException) : base(message, innerException)
    {
    }

    protected NullOrEmptyContentLengthException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
}
