namespace FZApi.Models;

public record GuildObjective
{
    public Guid Id { get; init; }
    public string Slug { get; init; } = "";
    public string Name { get; init; } = "";
    public string? Description { get; init; }
    public string CompletionMode { get; init; } = "";
    public int? RequiredCount { get; init; }
    public int CompletionTarget { get; init; }
    public int RequiredBossCount { get; init; }
    public int KilledRequiredCount { get; init; }
    public bool IsComplete { get; init; }
    public DateTimeOffset? CompletedAt { get; init; }
    public ExpansionRef Expansion { get; init; } = new();
    public ObjectiveRaidTier RaidTier { get; init; } = new();
    public IReadOnlyList<RequiredBoss> RequiredBosses { get; init; } = [];
}
