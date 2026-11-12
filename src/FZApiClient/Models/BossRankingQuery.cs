namespace FZApi.Models;

public record BossRankingQuery
{
    public required int ServerId { get; init; }
    public required Guid RankingPartitionId { get; init; }
    public string? CharacterClass { get; init; }
    public bool? OfficialOnly { get; init; }
    public Guid? GuildId { get; init; }
    public int? Limit { get; init; }
    public ParseMetric? Metric { get; init; }
}
