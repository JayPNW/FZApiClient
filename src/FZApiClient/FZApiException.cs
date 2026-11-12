using System.Net;
using System.Text.Json;

namespace FZApi;

public class FZApiException : Exception
{
    public FZApiException(HttpStatusCode statusCode, string error, string? responseBody)
        : base($"{(int)statusCode} {statusCode}: {error}")
    {
        StatusCode = statusCode;
        Error = error;
        ResponseBody = responseBody;
    }

    public HttpStatusCode StatusCode { get; }

    public string Error { get; }

    public string? ResponseBody { get; }

    internal static FZApiException FromResponse(HttpStatusCode statusCode, string? body)
    {
        var error = TryReadError(body) ?? (string.IsNullOrWhiteSpace(body) ? statusCode.ToString() : body.Trim());
        return new FZApiException(statusCode, error, body);
    }

    private static string? TryReadError(string? body)
    {
        if (string.IsNullOrWhiteSpace(body))
        {
            return null;
        }

        try
        {
            using var document = JsonDocument.Parse(body);
            if (document.RootElement.ValueKind == JsonValueKind.Object &&
                document.RootElement.TryGetProperty("error", out var error) &&
                error.ValueKind == JsonValueKind.String)
            {
                return error.GetString();
            }
        }
        catch (JsonException)
        {
        }

        return null;
    }
}
