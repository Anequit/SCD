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
                    KeepAlivePingTimeout = Timeout.InfiniteTimeSpan
                });
            }

            return _httpClient;
        }
    }

    public static void Cancel() => HttpClient.CancelPendingRequests();
}
