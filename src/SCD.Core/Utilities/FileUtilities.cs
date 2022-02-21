using SCD.Core.DataModels;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace SCD.Core.Utilities;

public static class FileUtilities
{
    public static async Task Save(FileChunk[] fileChunks, Stream destination, CancellationToken cancellationToken)
    {
        foreach(FileChunk chunk in fileChunks)
        {
            // Seek to the correct position, write the chunks data, and then flush the fileStream
            destination.Seek(chunk.StartingHeaderRange, SeekOrigin.Begin);
            await destination.WriteAsync(chunk.Data, cancellationToken);
            await destination.FlushAsync(cancellationToken);
        }
    }
}
