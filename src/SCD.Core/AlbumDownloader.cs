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
        if(!Directory.Exists(downloadLocation))
            Directory.CreateDirectory(downloadLocation);

        if(album.AlbumFiles is null || album.AlbumFiles.Count == 0)
            return;

        DownloadHandler downloadHandler = new DownloadHandler(progress, 1024 * 256, 20);

        do
        {
            token.ThrowIfCancellationRequested();

            AlbumFile file = album.AlbumFiles.Peek();

            FileChanged?.Invoke(file);

            // If the file url is empty or the file name is empty, then skip over the file
            if(string.IsNullOrEmpty(file.Filename) || string.IsNullOrEmpty(file.Url))
                continue;

            string filePath = Path.Combine(downloadLocation, Parser.ParseValidFilename(file.Filename));

            if(File.Exists(filePath))
            {
                album.AlbumFiles.Dequeue();
                continue;
            }

            using(FileStream fileStream = File.OpenWrite(filePath))
            {
                do
                {
                    try
                    {
                        await downloadHandler.DownloadAsync(file.Url, fileStream, token);
                        album.AlbumFiles.Dequeue();
                        break;
                    }
                    catch(Exception ex)
                    {
                        if(ex is TaskCanceledException or HttpRequestException or OperationCanceledException)
                        {
                            token.ThrowIfCancellationRequested();

                            continue;
                        }

                        ErrorOccurred?.Invoke(ex);
                    }
                } while(true);
            }
        } while(album.AlbumFiles.Count > 0);
    }
}
