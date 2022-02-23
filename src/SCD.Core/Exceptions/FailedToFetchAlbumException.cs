using System;
using System.Runtime.Serialization;

namespace SCD.Core.Exceptions;

public class FailedToFetchAlbumException : Exception
{
    public FailedToFetchAlbumException()
    {
    }

    public FailedToFetchAlbumException(string? message) : base(message)
    {
    }

    public FailedToFetchAlbumException(string? message, Exception? innerException) : base(message, innerException)
    {
    }

    protected FailedToFetchAlbumException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
}