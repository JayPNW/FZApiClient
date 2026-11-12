namespace FZApi;

public class FZApiClientOptions
{
    /// <summary>Both hosts serve the same API and data.</summary>
    public Uri? BaseAddress { get; init; }

    /// <summary>Keys begin with <c>fz_live_</c>.</summary>
    public string? ApiKey { get; init; }

    public string? SessionCookie { get; init; }

    public string UserAgent { get; init; } = "FZApiClient";
}
