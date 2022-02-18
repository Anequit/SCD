using SCD.Core.DataModels;
using SCD.Core.Extensions;
using SCD.Core.Utilities;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace SCD.Core;

public static class AlbumDownloader
{
    private static readonly Progress<double> _progress = new Progress<double>(progressAmount =>
    {
        // When progress is reported, invoke ProgressChanged with the current progress rounded.
        ProgressChanged?.Invoke(Math.Round(progressAmount, MidpointRounding.ToZero));
    });

    /// <summary>
    /// Fired when the current file changes.
    /// </summary>
    public static event Action<AlbumFile>? FileChanged;

    /// <summary>
    /// Fired when progress on the current file has changed.
    /// </summary>
    public static event Action<double>? ProgressChanged;

    /// <summary>
    /// Fired when the album is finished downloading.
    /// </summary>
    public static event Action<string>? DownloadFinished;

    /// <summary>
    /// Fired when an error occur.
    /// </summary>
    public static event Action<string>? ErrorOccurred;

    /// <summary>
    /// Downloads an album asynchronous.
    /// </summary>
    /// <param name="album">Album being downloaded.</param>
    /// <param name="downloadLocation">Location the album should be stored.</param>
    /// <param name="cancellationToken">Token to cancel the download.</param>
    /// <returns></returns>
    public static async Task DownloadAsync(Album album, string downloadLocation, CancellationToken cancellationToken)
    {
        //  If album files is null or empty, then return
        if(album.Files is null || album.Files.Length == 0)
            return;

        // If the album doesn't have a title, then set it to Album
        if(string.IsNullOrEmpty(album.Title))
            album.Title = "Album";

        // Normalize path by removing invalid characters from the album directory
        string path = PathUtilities.NormalizePath(Path.Combine(downloadLocation, album.Title));

        if(!Directory.Exists(path))
            Directory.CreateDirectory(path);

        foreach(AlbumFile file in album.Files)
        {
            ProgressChanged?.Invoke(0);
            FileChanged?.Invoke(file);

            cancellationToken.ThrowIfCancellationRequested();

            // If the file url is empty or the file name is empty, then skip over the file
            if(string.IsNullOrEmpty(file.File) || string.IsNullOrEmpty(file.Name))
                continue;

            // Normalize the file path
            string filePath = PathUtilities.NormalizePath(Path.Combine(path, file.Name));

            // Skip if it exists already
            if(File.Exists(filePath))
                continue;

            using(FileStream fileStream = File.OpenWrite(filePath))
            {
                try
                {
                    // Download and populate fileChunks
                    FileChunk[] fileChunks = await HttpClientHelper.HttpClient.DownloadFileChunksAsync(file.File, _progress, cancellationToken);

                    // Clean up any left up data from downloading the file
                    GC.Collect();

                    // Iterate over fileChunks
                    foreach(FileChunk chunk in fileChunks)
                    {
                        // Seek to the correct position, write the chunks data, and then flush the fileStream
                        fileStream.Seek(chunk.StartingHeaderRange, SeekOrigin.Begin);
                        await fileStream.WriteAsync(chunk.Data, cancellationToken);
                        await fileStream.FlushAsync(cancellationToken);
                    }
                }
                catch(Exception ex)
                {
                    switch(ex)
                    {
                        // If the CancellationToken was called, then ignore
                        case OperationCanceledException:
                            break;

                        default:
                            ErrorOccurred?.Invoke(ex.Message);
                            break;
                    }
                }
            }
        }

        DownloadFinished?.Invoke(path);
    }
}
