namespace SCD.Core.DataModels;

public record Album
{
    public bool Success { get; init; } = false;
    public string? Title { get; set; }
    public string? Description { get; init; }
    public AlbumFile[]? Files { get; init; }
}