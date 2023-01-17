using SCD.Core.DataModels;
using System;

namespace SCD.Core.Helpers;

public static class FileChunkHelper
{
    public static FileChunk[] BuildChunkArray(long contentLength, int partSize)
    {
        if(contentLength <= 0)
            throw new ArgumentOutOfRangeException(nameof(contentLength));

        if(partSize <= 0)
            throw new ArgumentOutOfRangeException(nameof(partSize));

        int amount = (int)Math.Ceiling((double)contentLength / partSize);

        FileChunk[] parts = new FileChunk[amount];

        for(int x = 0; x < amount; x++)
        {
            parts[x] = new FileChunk
            {
                StartingHeaderRange = x * partSize,
                EndingHeaderRange = (x * partSize) + partSize
            };
        }

        parts[^1].EndingHeaderRange = contentLength;

        return parts;
    }
}