using System.Text.Json;

namespace FZApi.Models;

public record CharacterPage
{
    /// <summary><c>DAMAGE</c> or <c>HEALING</c>.</summary>
    public string Metric { get; init; } = "";

    public string MetricLabel { get; init; } = "";
    public CharacterProfile Character { get; init; } = new();

    public IReadOnlyList<string> ClassOptions { get; init; } = [];

    public CharacterPermissions Permissions { get; init; } = new();
    public IReadOnlyList<JsonElement> BestParseTiers { get; init; } = [];
    public IReadOnlyList<JsonElement> ExpansionGroups { get; init; } = [];
    public IReadOnlyList<JsonElement> AllStarRanks { get; init; } = [];
    public IReadOnlyList<JsonElement> RecentParses { get; init; } = [];
}
