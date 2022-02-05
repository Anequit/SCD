namespace SCD.Core.Extensions;

public static class StreamExtensions
{
    public static async Task CopyToAsync(this Stream source, Stream stream, int bufferSize, IProgress<long> progress, CancellationToken cancellationToken)
    {
        int bytesRead;
        byte[]? buffer = new byte[bufferSize];
        long totalBytesRead = 0;

        do
        {
            cancellationToken.ThrowIfCancellationRequested();

            bytesRead = await source.ReadAsync(buffer, 0, buffer.Length, cancellationToken).ConfigureAwait(false);

            await stream.WriteAsync(buffer, 0, bytesRead, cancellationToken).ConfigureAwait(false);

            totalBytesRead += bytesRead;
            progress?.Report(totalBytesRead);
        } while(bytesRead != 0);
    }
}
