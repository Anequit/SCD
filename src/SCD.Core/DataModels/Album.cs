namespace SCD.Core.DataModels;

public class Album
{
    public bool Success { get; set; } = false;
    public AlbumFile[]? Files { get; set; }
}