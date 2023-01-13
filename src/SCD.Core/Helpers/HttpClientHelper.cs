using System;
using System.Net;
using System.Net.Http;
using System.Net.Security;
using System.Security.Authentication;

namespace SCD.Core.Helpers;

public static class HttpClientHelper
{
    public static HttpClient HttpClient { get; } = InitializeClient();

    public static void Cancel() => HttpClient.CancelPendingRequests();

    public static void Dispose() => HttpClient.Dispose();

    private static HttpClient InitializeClient()
    {
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;

        return new HttpClient(new SocketsHttpHandler
        {
            AllowAutoRedirect = true,
            EnableMultipleHttp2Connections = true,
            AutomaticDecompression = DecompressionMethods.All,
            PooledConnectionLifetime = TimeSpan.FromMinutes(5),
            SslOptions = new SslClientAuthenticationOptions
            {
                EnabledSslProtocols = SslProtocols.None,
                EncryptionPolicy = EncryptionPolicy.RequireEncryption,
                AllowRenegotiation = true
            }
        })
        {
            Timeout = TimeSpan.FromSeconds(30)
        };
    }
}