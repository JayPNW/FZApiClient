namespace FZApi.Models;

public record RaidContent
{
    public RankingPartition? ActiveRankingPartition { get; init; }
    public IReadOnlyList<RaidContentExpansion> Expansions { get; init; } = [];
    public IReadOnlyList<RaidContentTier> RaidTiers { get; init; } = [];
    public IReadOnlyList<RankingPartition> RankingPartitions { get; init; } = [];
    public ServerRef Server { get; init; } = new();
}
