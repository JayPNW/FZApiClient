namespace FZApi.Models;

public record ProgressionExpansion
{
    public Guid Id { get; init; }
    public string Slug { get; init; } = "";
    public string Name { get; init; } = "";
    public int SortOrder { get; init; }
    public IReadOnlyList<ProgressionTier> Tiers { get; init; } = [];
}
