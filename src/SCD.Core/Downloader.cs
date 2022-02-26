using SCD.Core.DataModels;
using SCD.Core.Extensions;
using SCD.Core.Helpers;
using SCD.Core.Utilities;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace SCD.Core;

public static class Downloader
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
    public static async Task DownloadAndSaveAlbumAsync(Album album, string downloadLocation, CancellationToken cancellationToken)
    {
        if(album.AlbumFiles is null || album.AlbumFiles.Length == 0)
            return;

        if(string.IsNullOrEmpty(album.Title))
            album.Title = "Album";

        // Normalize path by removing invalid characters from the album directory
        string path = PathUtilities.RemoveInvalidPathChars(Path.Combine(downloadLocation, album.Title));

        if(!Directory.Exists(path))
            Directory.CreateDirectory(path);

        foreach(AlbumFile file in album.AlbumFiles)
        {
            ProgressChanged?.Invoke(0);
            FileChanged?.Invoke(file);

            cancellationToken.ThrowIfCancellationRequested();

            // If the file url is empty or the file name is empty, then skip over the file
            if(string.IsNullOrEmpty(file.Filename) || string.IsNullOrEmpty(file.Url))
                continue;

            string filePath = Path.Combine(path, PathUtilities.RemoveInvalidFilenameChars(file.Filename));

            if(File.Exists(filePath))
                continue;

            using(FileStream fileStream = File.OpenWrite(filePath))
            {
                try
                {
                    FileChunk[] fileChunks = await HttpClientHelper.HttpClient.DownloadFileChunksAsync(file.Url, _progress, cancellationToken);

                    GC.Collect();

                    await FileUtilities.Save(fileChunks, fileStream, cancellationToken);
                }
                catch(Exception ex)
                {
                    switch(ex)
                    {
                        // If the CancellationToken was called, then ignore it
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
