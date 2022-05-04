namespace SCD.Core.DataModels;

public class Part
{
    public int Location { get; set; }
    public byte[]? Content { get; set; }
    public long StartingHeaderRange { get; set; }
    public long EndingHeaderRange { get; set; }
}