using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SCD.Core.Exceptions;
internal class FailedToDownloadFileChunkDataException : Exception
{
    public FailedToDownloadFileChunkDataException()
    {
    }

    public FailedToDownloadFileChunkDataException(string? message) : base(message)
    {
    }

    public FailedToDownloadFileChunkDataException(string? message, Exception? innerException) : base(message, innerException)
    {
    }

    protected FailedToDownloadFileChunkDataException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
}
