using System.Text.Json.Serialization;

namespace SCD.Core.DataModels;

public class Release
{
    [JsonPropertyName("tag_name")]
    public string VersionNumber { get; set; } = "";

    [JsonPropertyName("html_url")]
    public string Url { get; set; } = "";
}
