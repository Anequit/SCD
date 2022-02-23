using System.Text.Json.Serialization;

namespace SCD.Core.DataModels;

public record Album
{
    [JsonPropertyName("success")]
    public bool Success { get; init; }

    [JsonPropertyName("title")]
    public string? Title { get; set; }

    [JsonPropertyName("description")]
    public string? Description { get; init; }

    [JsonPropertyName("files")]
    public AlbumFile[]? AlbumFiles { get; init; }
}