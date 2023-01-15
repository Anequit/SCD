using System;
using System.Text.Json.Serialization;

namespace SCD.Core.DataModels;

public class AlbumFile : IDisposable
{
    public AlbumFile(string filename, string url)
    {
        Filename = filename;
        Url = url;
    }

    [JsonPropertyName("name")]
    public required string Filename { get; init; }

    [JsonPropertyName("file")]
    public required string Url { get; init; }

    public FileChunk[]? FileChunks { get; set; } = null;

    public void Dispose()
    {
        if(FileChunks is not null)
            Array.Clear(FileChunks);
        
        GC.SuppressFinalize(this);
    }
}