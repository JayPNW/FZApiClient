namespace FZApi.Models;

public record RankingPartition
{
    public Guid Id { get; init; }

    public Guid RankingPartitionId { get; init; }

    public Guid RaidTierId { get; init; }
    public Guid AnchorExpansionId { get; init; }
    public string Slug { get; init; } = "";
    public string Name { get; init; } = "";
    public int SortOrder { get; init; }
    public bool GlobalIsDefault { get; init; }
    public bool GlobalIsFrozen { get; init; }
    public DateTimeOffset GlobalStartsAt { get; init; }
    public DateTimeOffset? GlobalEndsAt { get; init; }
    public bool IsActive { get; init; }
    public bool IsFrozen { get; init; }
    public bool EffectivelyFrozen { get; init; }
    public DateTimeOffset? StartsAt { get; init; }
    public DateTimeOffset? EndsAt { get; init; }
    public string? Notes { get; init; }
    public ExpansionRef AnchorExpansion { get; init; } = new();
    public IReadOnlyList<ExpansionRef> IncludedExpansions { get; init; } = [];
}
