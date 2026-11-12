using System.Text;

namespace FZApi.Tests.Support;

internal class LiveApi
{
    private readonly RecordingHandler _recorder = new(new SocketsHttpHandler());

    public LiveApi()
    {
        var baseAddress = Environment.GetEnvironmentVariable("FZ_API_BASE_ADDRESS") is { Length: > 0 } configured
            ? new Uri(configured)
            : FZApiClient.FrostreaverZone;
        Client = new FZApiClient(
            new HttpClient(_recorder) { Timeout = TimeSpan.FromSeconds(60) },
            new FZApiClientOptions { BaseAddress = baseAddress, UserAgent = "FZApiClient-tests" });
    }

    public FZApiClient Client { get; }

    public async Task<T> Call<T>(Func<FZApiClient, Task<T>> call, string dataPath)
    {
        var result = await call(Client);
        var body = _recorder.LastResponseBody ?? throw new InvalidOperationException("No response was recorded.");
        JsonShape.AssertCovers(body, dataPath, result);
        return result;
    }

    private class RecordingHandler(HttpMessageHandler inner) : DelegatingHandler(inner)
    {
        public string? LastResponseBody { get; private set; }

        protected override async Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var response = await base.SendAsync(request, cancellationToken);
            var body = await response.Content.ReadAsStringAsync(cancellationToken);
            LastResponseBody = body;
            var mediaType = response.Content.Headers.ContentType?.MediaType ?? "application/json";
            response.Content = new StringContent(body, Encoding.UTF8, mediaType);
            return response;
        }
    }
}
