using System.Text.Json.Serialization;

namespace FZApi.Models;

public record ApiKeyStatus
{
    public bool HasApiKey { get; init; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? MaskedApiKey { get; init; }
}
