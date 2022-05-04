using SCD.Core.DataModels;
using SCD.Core.Exceptions;
using SCD.Core.Utilities;
using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace SCD.Core.Extensions;

public static class HttpClientExtensions
{
    public static async Task<Album> FetchAlbumAsync(this HttpClient httpClient, string url, CancellationToken token)
    {
        using(HttpRequestMessage requestMessage = new HttpRequestMessage())
        {
            requestMessage.RequestUri = new Uri("https://cyberdrop.me/api/album/get/" + Parser.ParseAlbumIdentifierFromUrl(url));
            requestMessage.Method = HttpMethod.Get;
            requestMessage.Headers.UserAgent.ParseAdd("SCD");
            requestMessage.Headers.Accept.ParseAdd("application/json");

            using(HttpResponseMessage responseMessage = await httpClient.SendAsync(requestMessage, token))
            {
                responseMessage.EnsureSuccessStatusCode();

                Album album = JsonSerializer.Deserialize<Album>(await responseMessage.Content.ReadAsStringAsync(token)) ?? throw new FailedToFetchAlbumException("Failed to fetch album.");

                if(!album.Success)
                {
                    switch(album.Description)
                    {
                        // Private album 
                        case "This album is not available for public.":
                            throw new FailedToFetchAlbumException("Album is private.");

                        // Will only occur if the url provided is invalid.
                        case "No token provided." or "Album not found.":
                            throw new FailedToFetchAlbumException("Album not found.");

                        // Can occur if the server is having some issues but is still up.
                        case "An unexpected error occcured. Try again?":
                            throw new FailedToFetchAlbumException("Unexpected error.");
                    }
                }

                return album;
            }
        }

    }

    public static async Task<Release> FetchLatestReleaseAsync(this HttpClient httpClient)
    {
        using(HttpRequestMessage httpRequestMessage = new HttpRequestMessage())
        {
            httpRequestMessage.RequestUri = new Uri("https://api.github.com/repos/Anequit/SCD/releases/latest");
            httpRequestMessage.Method = HttpMethod.Get;
            httpRequestMessage.Headers.UserAgent.ParseAdd("SCD");
            httpRequestMessage.Headers.Accept.ParseAdd("application/json");

            using(HttpResponseMessage httpResponseMessage = await httpClient.SendAsync(httpRequestMessage))
            {
                httpResponseMessage.EnsureSuccessStatusCode();

                Release release = JsonSerializer.Deserialize<Release>(await httpResponseMessage.Content.ReadAsStringAsync()) ?? throw new FailedToFetchLatestRelease();

                return release;
            }
        }
    }
}
