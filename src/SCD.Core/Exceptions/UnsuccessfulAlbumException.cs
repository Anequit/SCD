using System;
using System.Runtime.Serialization;

namespace SCD.Core.Exceptions;

public class UnsuccessfulAlbumException : Exception
{
    public UnsuccessfulAlbumException()
    {
    }

    public UnsuccessfulAlbumException(string? message) : base(message)
    {
    }

    public UnsuccessfulAlbumException(string? message, Exception? innerException) : base(message, innerException)
    {
    }

    protected UnsuccessfulAlbumException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
}
