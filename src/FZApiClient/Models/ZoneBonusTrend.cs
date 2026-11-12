namespace FZApi.Models;

public record ZoneBonusTrend
{
    public Guid Id { get; init; }
    public string Slug { get; init; } = "";
    public string Name { get; init; } = "";
    public string Expansion { get; init; } = "";
    public Guid ExpansionId { get; init; }
    public string ExpansionSlug { get; init; } = "";
    public int? MinLevel { get; init; }
    public int? MaxLevel { get; init; }
    public string? ZoneType { get; init; }

    public IReadOnlyDictionary<string, int> Counts { get; init; } = new Dictionary<string, int>();

    public int Total { get; init; }
}
