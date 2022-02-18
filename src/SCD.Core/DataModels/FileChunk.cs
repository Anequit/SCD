namespace SCD.Core.DataModels;

public class FileChunk
{
    public int Position { get; init; }

    public long StartingHeaderRange { get; init; }
    public long EndingHeaderRange { get; init; }

    public bool Downloaded { get; set; }
    public byte[]? Data { get; set; }
}
