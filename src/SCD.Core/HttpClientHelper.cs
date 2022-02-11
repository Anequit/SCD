using System.Net;
using System.Net.Security;
using System.Security.Authentication;

namespace SCD.Core;

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
                    EnableMultipleHttp2Connections = true,
                    SslOptions = new SslClientAuthenticationOptions()
                    {
                        AllowRenegotiation = true,
                        EnabledSslProtocols = SslProtocols.None
                    }
                });
            }

            return _httpClient;
        }
    }

    public static void Cancel() => HttpClient.CancelPendingRequests();
}
