using SCD.Core.DataModels;
using System.Net.Http.Headers;
using SCD.Core.Exceptions;

namespace SCD.Core.Extensions;

public static class HttpClientExtensions
{
    public static async Task DownloadAsync(this HttpClient httpClient, string url, Stream destination, IProgress<double> progress, CancellationToken cancellationToken)
    {
        using(HttpResponseMessage response = await httpClient.GetAsync(url, HttpCompletionOption.ResponseHeadersRead, cancellationToken))
        {
            response.EnsureSuccessStatusCode();

            long contentLength = (long)(response.Content.Headers.ContentLength is not null ? response.Content.Headers.ContentLength : 500000000);
            long buffer = (contentLength < 100_000) ? (contentLength / 100) : 100_000;
            double dataDownloaded = 0;

            List<FileChunk> fileChunks = GetFileChunks(contentLength, buffer);

            await Parallel.ForEachAsync(fileChunks, new ParallelOptions()
            {
                CancellationToken = cancellationToken
            }, async (i, token) =>
            {
                do
                {
                    try
                    {
                        using(HttpRequestMessage httpRequestMessage = new HttpRequestMessage())
                        {
                            httpRequestMessage.RequestUri = new Uri(url);
                            httpRequestMessage.Headers.Range = new RangeHeaderValue(i.StartingHeaderRange, i.EndingHeaderRange);

                            using(HttpResponseMessage httpResponseMessage = HttpClientHelper.HttpClient.SendAsync(httpRequestMessage, token).Result)
                            {
                                i.Data = await httpResponseMessage.Content.ReadAsByteArrayAsync(token);
                                i.Downloaded = true;


                                dataDownloaded += (long)httpResponseMessage.Content.Headers.ContentLength;
                                progress.Report(dataDownloaded / contentLength * 100);
                            }
                        }
                    }
                    catch(Exception ex)
                    {
                        switch(ex)
                        {
                            case AggregateException or TaskCanceledException or NullContentLengthException:
                                return;

                            default:
                                break;
                        }
                    }
                } while(!i.Downloaded);
            });

            foreach(FileChunk chunk in fileChunks)
            {
                destination.Seek(chunk.StartingHeaderRange, SeekOrigin.Begin);
                await destination.WriteAsync(chunk.Data, cancellationToken);
                await destination.FlushAsync(cancellationToken);
            }

            fileChunks.Clear();
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

        startingHeaderRanges.Clear();

        return fileChunks;
    }
}
