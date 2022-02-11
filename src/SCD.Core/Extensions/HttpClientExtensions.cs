using SCD.Core.DataModels;
using System.Collections.Concurrent;
using System.Net.Http.Headers;

namespace SCD.Core.Extensions;

public static class HttpClientExtensions
{
    public static async Task DownloadAsync(this HttpClient httpClient, string url, Stream destination, IProgress<decimal> progress, CancellationToken cancellationToken)
    {
        using(HttpResponseMessage response = await httpClient.GetAsync(url, HttpCompletionOption.ResponseHeadersRead, cancellationToken))
        {
            long contentLength = (long)(response.Content.Headers.ContentLength is not null ? response.Content.Headers.ContentLength : 500000000);
            long buffer = (contentLength < 100_000) ? (contentLength / 100) : 100_000;
            long dataDownloaded = 0;

            List<FileChunk> fileChunks = GetFileChunks(contentLength, buffer);

            Parallel.ForEach(Partitioner.Create(fileChunks, true), async i =>
            {
                cancellationToken.ThrowIfCancellationRequested();

                using(HttpRequestMessage httpRequestMessage = new HttpRequestMessage())
                {
                    httpRequestMessage.RequestUri = new Uri(url);
                    httpRequestMessage.Headers.Range = new RangeHeaderValue(i.StartingHeaderRange, i.EndingHeaderRange);

                    HttpResponseMessage? httpResponseMessage = null;

                    do
                    {
                        try
                        {
                            httpResponseMessage = HttpClientHelper.HttpClient.SendAsync(httpRequestMessage, cancellationToken).Result;
                        }
                        finally
                        {
                            if(httpResponseMessage is not null)
                            {
                                i.Data = await httpResponseMessage.Content.ReadAsByteArrayAsync(cancellationToken);
                                i.Downloaded = true;

                                dataDownloaded += (long)(httpResponseMessage.Content.Headers.ContentLength is not null ? httpResponseMessage.Content.Headers.ContentLength : 0);
                                progress.Report((decimal)dataDownloaded / contentLength * 100);

                                httpResponseMessage.Dispose();
                            }
                        }
                    } while(!i.Downloaded);
                }
            });

            foreach(FileChunk chunk in fileChunks)
            {
                destination.Seek(chunk.StartingHeaderRange, SeekOrigin.Begin);
                await destination.WriteAsync(chunk.Data, cancellationToken);
            }
        }
    }

    private static List<FileChunk> GetFileChunks(long contentLength, long bufferSize)
    {
        List<FileChunk> fileChunks = new List<FileChunk>();

        List<int> startingHeaderRanges = Enumerable.Range(0, (int)contentLength).Where(x => x % bufferSize == 0).ToList();

        foreach(int i in Enumerable.Range(0, startingHeaderRanges.Count))
        {
            fileChunks.Add(new FileChunk()
            {
                Position = i,
                StartingHeaderRange = startingHeaderRanges[i],
                EndingHeaderRange = startingHeaderRanges[i] + (int)bufferSize
            });
        }

        return fileChunks;
    }
}
