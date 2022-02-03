using Newtonsoft.Json;
using SCD.Core.DataModels;
using SCD.Core.Exceptions;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace SCD.Core.Utilities;

public static class Web
{
    public static void Open(string url)
    {
        // https://stackoverflow.com/a/43232486
        if(RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            url = url.Replace("&", "^&");
            Process.Start(new ProcessStartInfo("cmd", $"/c start {url}") { CreateNoWindow = true });
        }
        else if(RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
        {
            Process.Start("xdg-open", url);
        }
        else if(RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
        {
            Process.Start("open", url);
        }
    }

    public static async Task<Album> FetchAlbumAsync(string url)
    {
        string albumIdentifier = url.Substring(url.Length - 8);

        string response = await HttpClientHandler.HttpClient.GetStringAsync("https://cyberdrop.me/api/album/get/" + albumIdentifier);

        Album? album = JsonConvert.DeserializeObject<Album>(response);

        if(album is null || !album.Success)
        {
            throw new InvalidUrlException(url);
        }

        return album;
    }
}