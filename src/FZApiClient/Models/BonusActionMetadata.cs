using System.Text.Json;
using System.Text.Json.Serialization;

namespace FZApi.Models;

public record BonusActionMetadata
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? BonusKey { get; init; }

    /// <summary><c>client_log_signal</c> for reports sent with an API key.</summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Source { get; init; }

    [JsonExtensionData]
    public Dictionary<string, JsonElement>? Other { get; init; }
}
