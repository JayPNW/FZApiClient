using System.Text.Json;

namespace FZApi.Models;

public record ServerObjective
{
    public Guid Id { get; init; }
    public string Slug { get; init; } = "";
    public string Name { get; init; } = "";
    public string? Description { get; init; }

    /// <summary>Such as <c>ALL_REQUIRED</c>.</summary>
    public string CompletionMode { get; init; } = "";

    public int? RequiredCount { get; init; }
    public int CompletionTarget { get; init; }
    public int RequiredBossCount { get; init; }
    public int SortOrder { get; init; }
    public bool IsActive { get; init; }
    public bool IsProgressionRelevant { get; init; }
    public ExpansionRef Expansion { get; init; } = new();
    public ObjectiveRaidTier RaidTier { get; init; } = new();
    public int CompletionCount { get; init; }
    public JsonElement? FirstCompletion { get; init; }
    public IReadOnlyList<JsonElement> TopCompletions { get; init; } = [];
    public IReadOnlyList<JsonElement> RecentCompletions { get; init; } = [];
    public IReadOnlyList<RequiredBoss> RequiredBosses { get; init; } = [];
}
