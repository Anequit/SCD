using System.Text.Json.Serialization;

namespace SCD.Core.DataModels;

public class AlbumFile
{
    [JsonPropertyName("name")]
    public string Filename { get; init; } = "";

    [JsonPropertyName("file")]
    public string? Url { get; init; } = "";
}
