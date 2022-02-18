namespace SCD.Core.DataModels;

public record Album
{
    public bool Success { get; init; }
    public string? Title { get; set; }
    public string? Description { get; init; }
    public AlbumFile[]? Files { get; init; }
}