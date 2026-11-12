using System.Text.Json.Nodes;

namespace FZApi.Tests.Support;

internal record RecordedRequest(
    string Method,
    Uri Uri,
    IReadOnlyDictionary<string, string> Headers,
    string? ContentType,
    string? Body)
{
    public string PathAndQuery => Uri.PathAndQuery;

    public string? Header(string name) => Headers.TryGetValue(name, out var value) ? value : null;

    public JsonObject ToSnapshot()
    {
        var headers = new JsonObject();
        foreach (var name in new[] { "X-API-Key", "Cookie", "X-Guild-Code" })
        {
            if (Header(name) is { } value)
            {
                headers[name] = value;
            }
        }

        return new JsonObject
        {
            ["method"] = Method,
            ["url"] = Uri.AbsoluteUri,
            ["headers"] = headers,
            ["contentType"] = ContentType,
            ["body"] = string.IsNullOrEmpty(Body) ? null : JsonNode.Parse(Body),
        };
    }
}
