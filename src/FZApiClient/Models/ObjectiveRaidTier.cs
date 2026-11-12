using System.Text.Json;
using System.Text.Json.Serialization;

namespace FZApi.Models;

public record ObjectiveRaidTier
{
    public Guid Id { get; init; }
    public string Slug { get; init; } = "";
    public string Name { get; init; } = "";
    public int SortOrder { get; init; }

    /// <summary>Present in server progression; absent in guild progression.</summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IReadOnlyList<JsonElement>? RankingPartitions { get; init; }
}
