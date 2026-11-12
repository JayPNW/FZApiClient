using System.Text.Json.Serialization;

namespace FZApi.Models;

public record AllStarFilters
{
    public int ServerId { get; init; }
    public Guid RankingPartitionId { get; init; }
    public string Metric { get; init; } = "";

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public Guid? ExpansionId { get; init; }

    /// <summary>For one boss's standings. Sent by the live API but not in the spec.</summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public Guid? RaidBossId { get; init; }

    /// <summary>For one boss's standings. Sent by the live API but not in the spec.</summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? Page { get; init; }

    /// <summary>For one boss's standings. Sent by the live API but not in the spec.</summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? Limit { get; init; }
}
