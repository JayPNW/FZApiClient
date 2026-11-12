using System.Text.Json;

namespace FZApi.Models;

public record AllStarStandings
{
    public JsonElement? Snapshot { get; init; }
    public AllStarFilters Filters { get; init; } = new();

    /// <summary>The site reads <c>averagePercentile</c> from each item. Other fields aren't documented.</summary>
    public IReadOnlyList<JsonElement> Standings { get; init; } = [];

    public int TotalStandings { get; init; }
}
