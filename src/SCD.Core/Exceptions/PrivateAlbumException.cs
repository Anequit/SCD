using System;
using System.Runtime.Serialization;

namespace SCD.Core.Exceptions;

public class PrivateAlbumException : Exception
{
    public PrivateAlbumException()
    {
    }

    public PrivateAlbumException(string? message) : base(message)
    {
    }

    public PrivateAlbumException(string? message, Exception? innerException) : base(message, innerException)
    {
    }

    protected PrivateAlbumException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
}
