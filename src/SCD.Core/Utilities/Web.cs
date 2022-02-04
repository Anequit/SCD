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
        string albumIdentifier = url.Substring(url.LastIndexOf('/')); // ArgumentOutOfRangeException

        using(HttpResponseMessage response = await HttpClientHandler.HttpClient.GetAsync("https://cyberdrop.me/api/album/get/" + albumIdentifier))
        {
            response.EnsureSuccessStatusCode(); // HttpRequestException

            Album? album = JsonConvert.DeserializeObject<Album>(await response.Content.ReadAsStringAsync());

            if(album is null)
                throw new NullAlbumException();

            if(album.Description != null && !album.Success)
                throw new UnsuccessfulAlbumException(album.Description);

            return album;
        }
    }
}