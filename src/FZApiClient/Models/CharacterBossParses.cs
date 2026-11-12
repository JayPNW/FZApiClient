using System.Text.Json;

namespace FZApi.Models;

public record CharacterBossParses
{
    public string Metric { get; init; } = "";
    public string MetricLabel { get; init; } = "";
    public CharacterProfile Character { get; init; } = new();
    public IdSlugName RaidBoss { get; init; } = new();
    public IdSlugName RaidTier { get; init; } = new();
    public IdSlugName Expansion { get; init; } = new();
    public IReadOnlyList<JsonElement> Parses { get; init; } = [];
}
