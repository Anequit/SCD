using SCD.Core.DataModels;
using System;

namespace SCD.Core.Helpers;

public class PartHelper
{
    public static Part[] BuildPartArray(long contentLength, int buffer)
    {
        if(contentLength <= 0)
            throw new ArgumentOutOfRangeException(nameof(contentLength));

        if(buffer <= 0)
            throw new ArgumentOutOfRangeException(nameof(buffer));

        int amount = (int)Math.Ceiling((double)contentLength / buffer);

        Part[] parts = new Part[amount];

        for(int x = 0; x < amount; x++)
        {
            parts[x] = new Part()
            {
                Location = x,
                StartingHeaderRange = x * buffer,
                EndingHeaderRange = (x * buffer) + buffer
            };
        }

        parts[^1].EndingHeaderRange = contentLength;

        return parts;
    }
}
