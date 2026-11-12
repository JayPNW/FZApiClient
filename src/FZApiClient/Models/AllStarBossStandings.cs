using System.Text.Json;
using System.Text.Json.Serialization;

namespace FZApi.Models;

public record AllStarBossStandings : AllStarStandings
{
    /// <summary>The boss. Sent by the live API but not in the spec, so its fields aren't typed.</summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public JsonElement? Boss { get; init; }

    public int Page { get; init; }
    public int PageSize { get; init; }
    public int TotalPages { get; init; }
}
