using SCD.Core.DataModels;
using SCD.Core.Exceptions;
using SCD.Core.Helpers;
using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace SCD.Core;

class FileDownloader
{
    private readonly int _buffer;
    private readonly IProgress<decimal> _progress;
    private readonly SemaphoreSlim _semaphoreSlim;
    private long _contentLength;

    private long _downloaded;

    public FileDownloader(IProgress<decimal> progress, int buffer, int throttle)
    {
        _progress = progress;
        _buffer = buffer;

        _semaphoreSlim = new SemaphoreSlim(throttle);
    }

    public async Task<FileChunk[]> DownloadFileChunksAsync(AlbumFile albumFile, CancellationToken token)
    {
        _downloaded = 0;

        using(HttpResponseMessage headerResponse = await HttpClientHelper.HttpClient.GetAsync(albumFile.Url, HttpCompletionOption.ResponseHeadersRead, token))
        {
            headerResponse.EnsureSuccessStatusCode();

            _contentLength = headerResponse.Content.Headers.ContentLength ?? throw new NullOrZeroContentLengthException(albumFile.Url);

            FileChunk[] parts = PartHelper.BuildPartArray(_contentLength, _buffer);

            Task[] tasks = new Task[parts.Length];

            for(int x = 0; x < parts.Length; x++)
                tasks[x] = DownloadChunkAsync(parts[x], albumFile.Url, token);

            Task.WaitAll(tasks, token);

            return parts;
        }
    }

    private async Task DownloadChunkAsync(FileChunk chunk, string fileUrl, CancellationToken token)
    {
        await _semaphoreSlim.WaitAsync(token);

        do
        {
            try
            {
                using(HttpRequestMessage requestMessage = new HttpRequestMessage())
                {
                    requestMessage.RequestUri = new Uri(fileUrl);
                    requestMessage.Headers.Range = new RangeHeaderValue(chunk.StartingHeaderRange, chunk.EndingHeaderRange);

                    using(HttpResponseMessage responseMessage = await HttpClientHelper.HttpClient.SendAsync(requestMessage, HttpCompletionOption.ResponseContentRead, token))
                    {
                        responseMessage.EnsureSuccessStatusCode();

                        chunk.Content = await responseMessage.Content.ReadAsByteArrayAsync(token);

                        break;
                    }
                }
            }
            catch(Exception)
            {
                if(token.IsCancellationRequested)
                    return;
            }
        } while(true);

        _downloaded += chunk.EndingHeaderRange - chunk.StartingHeaderRange;
        _progress.Report((decimal)_downloaded / _contentLength * 100);

        _semaphoreSlim.Release();
    }
}