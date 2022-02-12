namespace SCD.Core.DataModels;

public class FileChunk
{
    public int Position { get; set; } = 0;

    public long StartingHeaderRange { get; set; } = 0;
    public long EndingHeaderRange { get; set; } = 0;

    public bool Downloaded { get; set; } = false;
    public byte[]? Data { get; set; }
}
