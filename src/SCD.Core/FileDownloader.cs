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

    public async Task DownloadFileAsync(AlbumFile albumFile, string saveLocation, CancellationToken token)
    {
        // Reset amount downloaded
        _downloaded = 0;

        // Get file headers
        using(HttpResponseMessage headerResponse = await HttpClientHelper.HttpClient.GetAsync(albumFile.Url, HttpCompletionOption.ResponseHeadersRead, token))
        {
            // Ensure headers were successful
            headerResponse.EnsureSuccessStatusCode();

            // Ensure content length is valid
            _contentLength = headerResponse.Content.Headers.ContentLength ?? throw new NullOrZeroContentLengthException(albumFile.Url);

            // Generate file chunk array 
            albumFile.FileChunks = FileChunkHelper.BuildChunkArray(_contentLength, _buffer);

            // Initalize chunk task array
            Task[] tasks = new Task[albumFile.FileChunks.Length];

            // Populate chunk task array
            for(int x = 0; x < albumFile.FileChunks.Length; x++)
                tasks[x] = DownloadChunkAsync(albumFile.FileChunks[x], albumFile.Url, token);

            // Wait for all chunks to download
            Task.WaitAll(tasks, token);

            // Save file to disk
            await SaveFileAsync(albumFile, saveLocation, token);
        }
    }

    private async Task DownloadChunkAsync(FileChunk chunk, string fileUrl, CancellationToken token)
    {
        // Wait for available position
        await _semaphoreSlim.WaitAsync(token);

        do
        {
            try
            {
                // Initalize http request
                using(HttpRequestMessage requestMessage = new HttpRequestMessage())
                {
                    requestMessage.RequestUri = new Uri(fileUrl);
                    requestMessage.Headers.Range = new RangeHeaderValue(chunk.StartingHeaderRange, chunk.EndingHeaderRange);

                    // Fetch chunk content
                    using(HttpResponseMessage responseMessage = await HttpClientHelper.HttpClient.SendAsync(requestMessage, HttpCompletionOption.ResponseContentRead, token))
                    {
                        responseMessage.EnsureSuccessStatusCode();

                        // Store chunk content
                        chunk.Content = await responseMessage.Content.ReadAsByteArrayAsync(token);

                        // Exit
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

        // Increase downloaded amount
        _downloaded += chunk.EndingHeaderRange - chunk.StartingHeaderRange;

        // Report downloaded percentage
        _progress.Report((decimal)_downloaded / _contentLength * 100);

        // Free up position
        _semaphoreSlim.Release();
    }

    private async Task SaveFileAsync(AlbumFile albumFile, string saveLocation, CancellationToken token)
    {
        // Initialize optimimal file stream options
        FileStreamOptions options = new FileStreamOptions
        {
            PreallocationSize = albumFile.FileChunks![^1].EndingHeaderRange,
            BufferSize = _buffer,
            Access = FileAccess.Write,
            Mode = FileMode.CreateNew,
            Share = FileShare.None,
            Options = FileOptions.SequentialScan
        };

        // Save File
        await using(FileStream fileStream = new FileStream(saveLocation, options))
        {
            foreach(FileChunk chunk in albumFile.FileChunks)
            {
                // Seek to start of file chunk
                fileStream.Seek(chunk.StartingHeaderRange, SeekOrigin.Begin);

                // Write file chunk
                await fileStream.WriteAsync(chunk.Content, token);
            }
        }
    }
}