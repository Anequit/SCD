using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace SCD.Core.DataModels;

public class Album
{
    [JsonPropertyName("success")]
    public bool Success { get; init; } = false;

    [JsonPropertyName("title")]
    public string Title { get; set; } = string.Empty;

    [JsonPropertyName("description")]
    public string Description { get; init; } = string.Empty;

    [JsonPropertyName("files")]
    public Queue<AlbumFile>? AlbumFiles { get; init; }
}