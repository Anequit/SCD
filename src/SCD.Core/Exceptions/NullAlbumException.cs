using System.Runtime.Serialization;

namespace SCD.Core.Exceptions;

public class NullAlbumException : Exception
{
    public NullAlbumException()
    {
    }

    public NullAlbumException(string? message) : base(message)
    {
    }

    public NullAlbumException(string? message, Exception? innerException) : base(message, innerException)
    {
    }

    protected NullAlbumException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
}
