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

internal class DownloadHandler
{
    private readonly IProgress<decimal> _progress;
    private readonly SemaphoreSlim _semaphoreSlim;
    private readonly int _buffer;

    private long _downloaded;
    private long _contentLength;

    public DownloadHandler(IProgress<decimal> progress, int buffer, int throttle)
    {
        _progress = progress;
        _buffer = buffer;

        _semaphoreSlim = new SemaphoreSlim(throttle);
    }

    public async Task DownloadAsync(string url, Stream stream, CancellationToken token)
    {
        _downloaded = 0;

        using(HttpResponseMessage headerResponse = await HttpClientHelper.HttpClient.GetAsync(url, HttpCompletionOption.ResponseHeadersRead, token))
        {
            headerResponse.EnsureSuccessStatusCode();

            _contentLength = headerResponse.Content.Headers.ContentLength ?? throw new NullOrZeroContentLengthException(url);

            Part[] parts = PartHelper.BuildPartArray(_contentLength, _buffer);

            Task[] tasks = new Task[parts.Length];

            for(int x = 0; x < parts.Length; x++)
                tasks[x] = DownloadPartAsync(parts[x], url, token);

            Task.WaitAll(tasks, token);

            foreach(Part part in parts)
            {
                stream.Seek(part.StartingHeaderRange, SeekOrigin.Begin);
                await stream.WriteAsync(part.Content, token);
                await stream.FlushAsync(token);
            }
        }
    }

    private async Task DownloadPartAsync(Part part, string url, CancellationToken token)
    {
        await _semaphoreSlim.WaitAsync(token);

        bool downloaded = false;

        do
        {
            try
            {
                using(HttpRequestMessage requestMessage = new HttpRequestMessage())
                {
                    requestMessage.RequestUri = new Uri(url);
                    requestMessage.Headers.Range = new RangeHeaderValue(part.StartingHeaderRange, part.EndingHeaderRange);

                    using(HttpResponseMessage responseMessage = await HttpClientHelper.HttpClient.SendAsync(requestMessage, HttpCompletionOption.ResponseContentRead, token))
                    {
                        responseMessage.EnsureSuccessStatusCode();

                        part.Content = await responseMessage.Content.ReadAsByteArrayAsync(token);
                        downloaded = true;
                    }
                }
            }
            catch(Exception)
            {
                if(token.IsCancellationRequested)
                    return;
            }
        }
        while(downloaded == false);

        _downloaded += part.EndingHeaderRange - part.StartingHeaderRange;
        _progress.Report((decimal)_downloaded / _contentLength * 100);

        _semaphoreSlim.Release();
    }
}