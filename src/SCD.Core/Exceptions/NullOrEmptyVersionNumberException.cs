using System;
using System.Runtime.Serialization;

namespace SCD.Core.Exceptions;

internal class NullOrEmptyVersionNumberException : Exception
{
    public NullOrEmptyVersionNumberException()
    {
    }

    public NullOrEmptyVersionNumberException(string? message) : base(message)
    {
    }

    public NullOrEmptyVersionNumberException(string? message, Exception? innerException) : base(message, innerException)
    {
    }

    protected NullOrEmptyVersionNumberException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
}
