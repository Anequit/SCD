using System.Net.Http.Headers;

namespace SCD.Core.Extensions;

public static class HttpClientExtensions
{
    public static async Task DownloadAsync(this HttpClient httpClient, string url, Stream destination, IProgress<decimal> progress, CancellationToken cancellationToken)
    {
        using(HttpResponseMessage response = await httpClient.GetAsync(url, HttpCompletionOption.ResponseHeadersRead, cancellationToken))
        {
            // If content length is null, then set contentLength to default 500mb
            long contentLength = (long)(response.Content.Headers.ContentLength is not null ? response.Content.Headers.ContentLength : 500000000);
            long dataDownloaded = 0;
            long buffer = (contentLength < 100_000) ? (contentLength / 100) : 100_000;

            do
            {
                cancellationToken.ThrowIfCancellationRequested();

                using(HttpRequestMessage httpRequest = new HttpRequestMessage())
                {
                    httpRequest.RequestUri = new Uri(url);
                    httpRequest.Headers.Range = new RangeHeaderValue(dataDownloaded, dataDownloaded + buffer);

                    using(HttpResponseMessage httpResponse = await HttpClientHelper.HttpClient.SendAsync(httpRequest, cancellationToken))
                    {
                        destination.Seek(dataDownloaded, SeekOrigin.Begin);
                        await destination.WriteAsync(await httpResponse.Content.ReadAsByteArrayAsync(), cancellationToken);

                        dataDownloaded += (long)(httpResponse.Content.Headers.ContentLength is not null ? httpResponse.Content.Headers.ContentLength : 0);
                        progress.Report((decimal)dataDownloaded / contentLength * 100);
                    }
                }
            } while(dataDownloaded != response.Content.Headers.ContentLength);
        }
    }
}
