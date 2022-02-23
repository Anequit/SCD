using System;
using System.Runtime.Serialization;

namespace SCD.Core.Exceptions;

public class InvalidAlbumException : Exception
{
    public InvalidAlbumException()
    {
    }

    public InvalidAlbumException(string? message) : base(message)
    {
    }

    public InvalidAlbumException(string? message, Exception? innerException) : base(message, innerException)
    {
    }

    protected InvalidAlbumException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
}