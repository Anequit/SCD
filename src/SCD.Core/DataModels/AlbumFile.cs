using SCD.Core.Helpers;
using SCD.Core.Extensions;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SCD.Core.DataModels;

public record AlbumFile
{
    [JsonPropertyName("name")]
    public string Filename { get; init; } = "";

    [JsonPropertyName("file")]
    public string Url { get; init; } = "";

    [JsonIgnore]
    public FileChunk[]? FileChunks { get; set; }

    public async Task BuildFileChunks()
    {
        
    }
}
