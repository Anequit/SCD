using System;
using System.Net.Http;
using System.Threading;

namespace SCD.Core.Helpers;

public static class HttpClientHelper
{
    private static HttpClient? _httpClient;

    public static HttpClient HttpClient
    {
        get
        {
            if(_httpClient == null)
            {
                _httpClient = new HttpClient(new SocketsHttpHandler()
                {
                    AllowAutoRedirect = true,
                    KeepAlivePingTimeout = Timeout.InfiniteTimeSpan,
                    MaxConnectionsPerServer = Environment.ProcessorCount,
                    EnableMultipleHttp2Connections = true
                });
            }

            return _httpClient;
        }
    }

    /// <summary>
    /// Cancel pending requests
    /// </summary>
    public static void Cancel() => HttpClient.CancelPendingRequests();
}
