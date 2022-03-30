using SCD.Core.DataModels;
using SCD.Core.Extensions;
using SCD.Core.Helpers;
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
        ProgressChanged?.Invoke(Math.Round(progressAmount, MidpointRounding.ToZero));
    });

    public static event Action<AlbumFile>? FileChanged;

    public static event Action<double>? ProgressChanged;

    public static event Action<string>? DownloadFinished;

    public static event Action<string>? ErrorOccurred;

    public static async Task DownloadAndSaveAlbumAsync(Album album, string downloadLocation, CancellationToken cancellationToken)
    {
        if(album.AlbumFiles is null || album.Title is null)
            return;
        
        // If the album title contains all invalid characters, then set to default Album
        if(string.IsNullOrWhiteSpace(PathUtilities.RemoveInvalidPathChars(album.Title)))
            album.Title = "Album";

        // Remove all invalid chars from path
        downloadLocation = PathUtilities.RemoveInvalidPathChars(downloadLocation);

        string path = Path.Combine(downloadLocation, album.Title);

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
