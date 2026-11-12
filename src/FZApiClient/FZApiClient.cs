using System.Net.Http.Json;
using System.Text.Json;
using FZApi.Json;

namespace FZApi;

public partial class FZApiClient
{
    public static readonly Uri FrostreaverZone = new("https://frostreaver.zone");
    public static readonly Uri FangbreakerZone = new("https://fangbreaker.zone");

    private readonly HttpClient _http;
    private readonly FZApiClientOptions _options;
    private readonly Uri _baseAddress;

    public FZApiClient(HttpClient httpClient, FZApiClientOptions? options = null)
    {
        _http = httpClient;
        _options = options ?? new FZApiClientOptions();
        _baseAddress = httpClient.BaseAddress ?? _options.BaseAddress ?? FrostreaverZone;
    }

    private enum Auth
    {
        None,
        ApiKey,
        Session,
    }

    private static string Segment(string value) => Uri.EscapeDataString(value);

    private static string Segment(Guid value) => value.ToString("D");

    private async Task<T> GetDataAsync<T>(string path, CancellationToken cancellationToken, Auth auth = Auth.None)
    {
        var body = await SendAsync(HttpMethod.Get, path, null, auth, cancellationToken).ConfigureAwait(false);
        return ReadData<T>(body);
    }

    private async Task<T> SendForDataAsync<T>(
        HttpMethod method, string path, HttpContent? content, Auth auth, CancellationToken cancellationToken)
    {
        var body = await SendAsync(method, path, content, auth, cancellationToken).ConfigureAwait(false);
        return ReadData<T>(body);
    }

    private async Task<string> SendAsync(
        HttpMethod method,
        string path,
        HttpContent? content,
        Auth auth,
        CancellationToken cancellationToken,
        Action<HttpRequestMessage>? configure = null)
    {
        using var request = new HttpRequestMessage(method, new Uri(_baseAddress, path)) { Content = content };
        request.Headers.Accept.ParseAdd("application/json");
        if (_http.DefaultRequestHeaders.UserAgent.Count == 0)
        {
            request.Headers.UserAgent.ParseAdd(_options.UserAgent);
        }

        switch (auth)
        {
            case Auth.ApiKey:
                request.Headers.Add("X-API-Key", RequireCredential(_options.ApiKey, nameof(FZApiClientOptions.ApiKey)));
                break;
            case Auth.Session:
                request.Headers.Add(
                    "Cookie", "fz.sid=" + RequireCredential(_options.SessionCookie, nameof(FZApiClientOptions.SessionCookie)));
                break;
        }

        configure?.Invoke(request);

        using var response = await _http.SendAsync(request, cancellationToken).ConfigureAwait(false);
        var body = await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
        if (!response.IsSuccessStatusCode)
        {
            throw FZApiException.FromResponse(response.StatusCode, body);
        }

        return body;
    }

    private static string RequireCredential(string? value, string name)
        => string.IsNullOrEmpty(value)
            ? throw new InvalidOperationException($"This operation needs {nameof(FZApiClientOptions)}.{name}.")
            : value;

    private static T ReadData<T>(string body)
    {
        var envelope = JsonSerializer.Deserialize<Envelope<T>>(body, FZApiJson.Options)
                       ?? throw new JsonException("The response body was empty.");
        if (!envelope.Ok)
        {
            throw new FZApiException(System.Net.HttpStatusCode.OK, envelope.Error ?? "Request failed", body);
        }

        return envelope.Data!;
    }

    private static JsonContent Json<T>(T value, JsonSerializerOptions? options = null)
        => JsonContent.Create(value, options: options ?? FZApiJson.Options);

    private record Envelope<T>(bool Ok, T? Data, string? Error);
}
