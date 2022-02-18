using SCD.Core.DataModels;
using SCD.Core.Exceptions;
using System.Diagnostics;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Text.Json;
using System.Threading.Tasks;

namespace SCD.Core.Utilities;

public static class WebUtilities
{
    /// <summary>
    /// Opens webpage in default browser.
    /// </summary>
    /// <param name="url">Url to open.</param>
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

    /// <summary>
    /// Fetch a potential album from a url.
    /// </summary>
    /// <param name="url">Link to album.</param>
    /// <returns>Successfully fetched album.</returns>
    /// <exception cref="NullAlbumException">If the album doesn't deserialize</exception>
    /// <exception cref="UnsuccessfulAlbumException">If album doesn't exist or is private.</exception>
    public static async Task<Album> FetchAlbumAsync(string url)
    {
        // Get album identifier from url
        string albumIdentifier = url.Substring(url.LastIndexOf('/'));

        // Call api
        using(HttpResponseMessage response = await HttpClientHelper.HttpClient.GetAsync("https://cyberdrop.me/api/album/get/" + albumIdentifier))
        {
            // Check if api had successful response
            response.EnsureSuccessStatusCode();

            // Attempt to deserialize json response from api
            Album? album = JsonSerializer.Deserialize<Album>(await response.Content.ReadAsStringAsync(), new JsonSerializerOptions()
            {
                PropertyNameCaseInsensitive = true
            });

            // If the album doesn't deserialize
            if(album is null)
                throw new NullAlbumException();

            // If album doesn't exist or is private.
            if(album.Description != null && !album.Success)
                throw new UnsuccessfulAlbumException(album.Description);

            return album;
        }
    }
}