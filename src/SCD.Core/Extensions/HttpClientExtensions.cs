namespace SCD.Core.Extensions;

public static class HttpClientExtensions
{
    public static async Task DownloadAsync(this HttpClient httpClient, string url, Stream destination, IProgress<decimal> progress, CancellationToken cancellationToken)
    {
        using(HttpResponseMessage? response = await httpClient.GetAsync(url, HttpCompletionOption.ResponseHeadersRead, cancellationToken))
        {
            long? contentLength = response.Content.Headers.ContentLength;

            using(Stream? content = await response.Content.ReadAsStreamAsync(cancellationToken))
            {
                if(progress == null || !contentLength.HasValue)
                {
                    await content.CopyToAsync(destination);
                    return;
                }

                Progress<long>? relativeProgress = new Progress<long>(totalBytes => progress.Report((decimal)totalBytes / contentLength.Value * 100));

                await content.CopyToAsync(destination, 8192, relativeProgress, cancellationToken);
            }
        }
    }
}
