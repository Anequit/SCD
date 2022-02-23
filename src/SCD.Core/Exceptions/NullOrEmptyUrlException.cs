using System;
using System.Runtime.Serialization;

namespace SCD.Core.Exceptions;

public class NullOrEmptyUrlException : Exception
{
    public NullOrEmptyUrlException()
    {
    }

    public NullOrEmptyUrlException(string? message) : base(message)
    {
    }

    public NullOrEmptyUrlException(string? message, Exception? innerException) : base(message, innerException)
    {
    }

    protected NullOrEmptyUrlException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
}
