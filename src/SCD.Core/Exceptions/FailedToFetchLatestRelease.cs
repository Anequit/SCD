using System;
using System.Runtime.Serialization;

namespace SCD.Core.Exceptions;

public class FailedToFetchLatestRelease : Exception
{
    public FailedToFetchLatestRelease()
    {
    }

    public FailedToFetchLatestRelease(string? message) : base(message)
    {
    }

    public FailedToFetchLatestRelease(string? message, Exception? innerException) : base(message, innerException)
    {
    }

    protected FailedToFetchLatestRelease(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
}
