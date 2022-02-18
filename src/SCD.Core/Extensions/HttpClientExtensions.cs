using SCD.Core.DataModels;
using SCD.Core.Exceptions;
using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace SCD.Core.Extensions;

public static class HttpClientExtensions
{
    public static async Task<FileChunk[]> DownloadFileChunksAsync(this HttpClient httpClient, string url, IProgress<double> progress, CancellationToken cancellationToken)
    {
        using(HttpResponseMessage response = await httpClient.GetAsync(url, HttpCompletionOption.ResponseHeadersRead, cancellationToken))
        {
            response.EnsureSuccessStatusCode();

            if(response.Content.Headers.ContentLength is null)
                throw new NullOrEmptyContentLengthException();

            long contentLength = (long)response.Content.Headers.ContentLength;
            long buffer = (contentLength < 100_000) ? (contentLength / 100) : 100_000;
            double dataDownloaded = 0;

            FileChunk[] fileChunks = GetFileChunks(contentLength, buffer);

            await Parallel.ForEachAsync(fileChunks, new ParallelOptions()
            {
                CancellationToken = cancellationToken
            }, async (fileChunk, token) =>
            {
                fileChunk.Data = await DownloadFileChunkDataAsync(fileChunk, url, token);
                dataDownloaded += fileChunk.Data.LongLength;
                progress.Report(dataDownloaded / contentLength * 100);
            });

            return fileChunks;
        }
    }

    private static FileChunk[] GetFileChunks(long contentLength, long bufferSize)
    {
        int[] startingHeaderRanges = Enumerable.Range(0, (int)contentLength).Where(x => x % bufferSize == 0).ToArray();

        FileChunk[] fileChunks = new FileChunk[startingHeaderRanges.Length];

        foreach(int i in Enumerable.Range(0, startingHeaderRanges.Length))
        {
            fileChunks[i] = new FileChunk()
            {
                Position = i,
                StartingHeaderRange = startingHeaderRanges[i],
                EndingHeaderRange = startingHeaderRanges[i] + (int)bufferSize
            };
        }

        return fileChunks;
    }

    private static async Task<byte[]> DownloadFileChunkDataAsync(FileChunk fileChunk, string url, CancellationToken cancellationToken)
    {
        byte[] data;

        do
        {
            data = new byte[fileChunk.EndingHeaderRange - fileChunk.StartingHeaderRange];

            try
            {
                using(HttpRequestMessage httpRequestMessage = new HttpRequestMessage())
                {
                    httpRequestMessage.RequestUri = new Uri(url);
                    httpRequestMessage.Headers.Range = new RangeHeaderValue(fileChunk.StartingHeaderRange, fileChunk.EndingHeaderRange);

                    using(HttpResponseMessage httpResponseMessage = HttpClientHelper.HttpClient.SendAsync(httpRequestMessage, cancellationToken).Result)
                    {
                        if(httpResponseMessage.Content.Headers.ContentLength is null or 0)
                            throw new NullOrEmptyContentLengthException();

                        data = await httpResponseMessage.Content.ReadAsByteArrayAsync(cancellationToken);
                        fileChunk.Downloaded = true;
                    }
                }
            }
            catch(Exception ex)
            {
                if(ex is AggregateException or TaskCanceledException or NullOrEmptyContentLengthException)
                    break;

                continue;
            }
        } while(!fileChunk.Downloaded);

        return data;
    }
}
