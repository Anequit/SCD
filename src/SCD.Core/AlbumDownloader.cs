using SCD.Core.DataModels;
using SCD.Core.Utilities;
using System;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace SCD.Core;

public static class AlbumDownloader
{
    public static event Action<AlbumFile>? FileChanged;
    public static event Action<Exception>? ErrorOccurred;

    public static async Task DownloadAlbumAsync(Album album, string downloadLocation, IProgress<decimal> progress, CancellationToken token)
    {
        // Create file directory for album files
        if(!Directory.Exists(downloadLocation))
            Directory.CreateDirectory(downloadLocation);

        // Return early if album is emptyy
        if(album.AlbumFiles.Count == 0)
            return;

        // Initialize file downloader
        FileDownloader fileDownloader = new FileDownloader(progress, 1024 * 256, 1000);

        do
        {
            // Take first file in queue
            AlbumFile file = album.AlbumFiles.Peek();

            // Notify UI of file change
            FileChanged?.Invoke(file);

            // If the file url is empty or the file name is empty, then skip over the file
            if(string.IsNullOrEmpty(file.Filename) || string.IsNullOrEmpty(file.Url))
                continue;

            // Create file path
            string filePath = Path.Combine(downloadLocation, Parser.ParseValidFilename(file.Filename));

            // Check if current file exists already
            if(File.Exists(filePath))
            {
                // Remove file from queue
                album.AlbumFiles.Dequeue();

                // Move to next file
                continue;
            }

            try
            {
                // Download current file
                await fileDownloader.DownloadFileAsync(file, filePath, token);
            }
            catch(Exception ex)
            {
                // Remove file if exception was thrown
                File.Delete(filePath);

                // Propagate token cancellation exception
                token.ThrowIfCancellationRequested();

                // Retry if IO exception or http request exception
                if(ex is IOException or HttpRequestException)
                    continue;

                // Notify UI if unexpected error
                ErrorOccurred?.Invoke(ex);

                // Propagate unexpected error
                throw;
            }

            // Remove completed file from queue
            album.AlbumFiles.Dequeue();
        } while(album.AlbumFiles.Count > 0);
    }
}