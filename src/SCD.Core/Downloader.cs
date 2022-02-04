using SCD.Core.DataModels;

namespace SCD.Core;

public static class Downloader
{
    public static event EventHandler<AlbumFile>? FileChanged;
    public static event EventHandler<int>? ProgressChanged;

    public static void DownloadAlbum(Album album)
    {
        if(album.Files is null || album.Files.Length == 0)
            return;

        foreach(AlbumFile file in album.Files)
        {
            // Download logic for each file
        }
    }
}
