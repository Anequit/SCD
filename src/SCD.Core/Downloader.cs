using SCD.Core.DataModels;
using SCD.Core.Extensions;
using SCD.Core.Utilities;
using System.Globalization;

namespace SCD.Core;

public static class AlbumDownloader
{
    private static readonly Progress<decimal> _progress = new Progress<decimal>(progressAmount => ProgressChanged?.Invoke(Convert.ToInt32(decimal.Round(progressAmount, MidpointRounding.ToZero), CultureInfo.InvariantCulture)));

    public static event Action<AlbumFile>? FileChanged;
    public static event Action<int>? ProgressChanged;
    public static event Action<string>? DownloadFinished;

    public static async Task Download(Album album, string downloadLocation, CancellationToken cancellationToken)
    {
        if(album.Files is null || album.Files.Length == 0)
            return;

        if(string.IsNullOrEmpty(album.Title))
            album.Title = "Album";

        string path = PathUtilities.NormalizePath(Path.Combine(downloadLocation, album.Title));

        if(!Directory.Exists(path))
            Directory.CreateDirectory(path);

        foreach(AlbumFile file in album.Files)
        {
            FileChanged?.Invoke(file);

            cancellationToken.ThrowIfCancellationRequested();

            if(string.IsNullOrEmpty(file.File) || string.IsNullOrEmpty(file.Name))
                continue;

            string filePath = PathUtilities.NormalizePath(Path.Combine(path, file.Name));

            if(File.Exists(filePath))
                continue;

            using(FileStream fileStream = File.OpenWrite(filePath))
            {
                await HttpClientHelper.HttpClient.DownloadAsync(file.File, fileStream, 100000, _progress, cancellationToken);
            }
        }

        DownloadFinished?.Invoke(path);
    }
}
