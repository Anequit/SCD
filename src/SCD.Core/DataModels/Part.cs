namespace SCD.Core.DataModels;

public class FileChunk
{
    public required long StartingHeaderRange { get; init; }
    public required long EndingHeaderRange { get; set; }

    public byte[]? Content { get; set; }
}