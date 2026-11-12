using System.Text.Json;

namespace FZApi.Models;

public record ProgressionTier
{
    public Guid Id { get; init; }
    public string Slug { get; init; } = "";
    public string Name { get; init; } = "";
    public int SortOrder { get; init; }
    public IReadOnlyList<JsonElement> RankingPartitions { get; init; } = [];
    public IReadOnlyList<ServerObjective> Objectives { get; init; } = [];
}
