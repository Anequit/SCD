using SCD.Core.DataModels;
using SCD.Core.Exceptions;
using SCD.Core.Helpers;
using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace SCD.Core.Extensions;

public static class HttpClientExtensions
{
    /// <summary>
    /// Downloads file chunks asynchronously
    /// </summary>
    /// <param name="httpClient"></param>
    /// <param name="url">Url being downloaded from.</param>
    /// <param name="progress"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>Array of populated file chunks.</returns>
    /// <exception cref="NullOrEmptyContentLengthException"></exception>
    public static async Task<FileChunk[]> DownloadFileChunksAsync(this HttpClient httpClient, string url, IProgress<double> progress, CancellationToken cancellationToken)
    {
        // Get only headers of url.
        using(HttpResponseMessage response = await httpClient.GetAsync(url, HttpCompletionOption.ResponseHeadersRead, cancellationToken))
        {
            response.EnsureSuccessStatusCode();

            if(response.Content.Headers.ContentLength is null)
                throw new NullOrEmptyContentLengthException();

            // Length of the file being downloaded.
            long contentLength = (long)response.Content.Headers.ContentLength;

            // Optimal buffer for file size with a limit of 100,000.
            long buffer = (contentLength < 100_000) ? (contentLength / 100) : 100_000;

            // Total amount of data downloaded from the current file.
            double dataDownloaded = 0;

            // Get filechunks with only position, starting range header, and ending range header.
            FileChunk[] fileChunks = GetFileChunks(contentLength, buffer);

            // Iterate through the fileChunks in parallel
            await Parallel.ForEachAsync(fileChunks, new ParallelOptions()
            {
                CancellationToken = cancellationToken
            }, async (fileChunk, token) =>
            {
                // Download and store the chunk data in the current fileChunk
                fileChunk.Data = await DownloadFileChunkDataAsync(fileChunk, url, token);
                dataDownloaded += fileChunk.Data.LongLength;

                // Report new downloading progress
                progress.Report(dataDownloaded / contentLength * 100);
            });

            return fileChunks;
        }
    }

    /// <summary>
    /// Builds an array of FileChunks to iterate over.
    /// </summary>
    /// <param name="contentLength">Size of the file being downloaded.</param>
    /// <param name="bufferSize">Size of the buffer.</param>
    /// <returns>Populated array of FileChunks.</returns>
    private static FileChunk[] GetFileChunks(long contentLength, long bufferSize)
    {
        // All buffers in range of 0 and contentLength
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

    /// <summary>
    /// Downloads file chunk data asynchronously.
    /// </summary>
    /// <param name="fileChunk">Current Chunk.</param>
    /// <param name="url">Url being downloaded from.</param>
    /// <param name="cancellationToken"></param>
    /// <returns>Current freshly downloaded data.</returns>
    private static async Task<byte[]> DownloadFileChunkDataAsync(FileChunk fileChunk, string url, CancellationToken cancellationToken)
    {
        byte[] data;

        do
        {
            data = new byte[fileChunk.EndingHeaderRange - fileChunk.StartingHeaderRange];

            try
            {
                // Build httpRequestMessage with the url and our range header values from the fileChunk
                using(HttpRequestMessage httpRequestMessage = new HttpRequestMessage())
                {
                    httpRequestMessage.RequestUri = new Uri(url);
                    httpRequestMessage.Headers.Range = new RangeHeaderValue(fileChunk.StartingHeaderRange, fileChunk.EndingHeaderRange);

                    // Make request to with httpRequestMessage and lock the thread by waiting for the result.
                    using(HttpResponseMessage httpResponseMessage = HttpClientHelper.HttpClient.SendAsync(httpRequestMessage, cancellationToken).Result)
                    {
                        if(httpResponseMessage.Content.Headers.ContentLength is null or 0)
                            throw new NullOrEmptyContentLengthException();

                        // Store the data from the reponse
                        data = await httpResponseMessage.Content.ReadAsByteArrayAsync(cancellationToken);
                        fileChunk.Downloaded = true;
                    }
                }
            }
            catch(Exception ex)
            {
                // Thrown if CancellationToken is canceled otherwise ignore and try again.
                if(ex is AggregateException or TaskCanceledException or NullOrEmptyContentLengthException)
                    break;

                continue;
            }
        } while(!fileChunk.Downloaded);

        return data;
    }
}
