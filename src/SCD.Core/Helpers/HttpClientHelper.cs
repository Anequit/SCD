using System;
using System.Net;
using System.Net.Http;
using System.Net.Security;
using System.Security.Authentication;

namespace SCD.Core.Helpers;

public static class HttpClientHelper
{
    private static readonly HttpClient _httpClient = InitializeClient();

    public static HttpClient HttpClient => _httpClient;

    public static void Cancel() => HttpClient.CancelPendingRequests();

    public static void Dispose() => HttpClient.Dispose();

    private static HttpClient InitializeClient()
    {
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;

        return new HttpClient(new SocketsHttpHandler()
        {
            AllowAutoRedirect = true,
            EnableMultipleHttp2Connections = true,
            AutomaticDecompression = DecompressionMethods.All,
            SslOptions = new SslClientAuthenticationOptions()
            {
                EnabledSslProtocols = SslProtocols.None,
                EncryptionPolicy = EncryptionPolicy.RequireEncryption,
                AllowRenegotiation = true
            }
        })
        {
            Timeout = TimeSpan.FromSeconds(10)
        };
    }
}
