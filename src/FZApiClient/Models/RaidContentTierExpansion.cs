namespace FZApi.Models;

public record RaidContentTierExpansion
{
    public Guid Id { get; init; }
    public string Slug { get; init; } = "";
    public string Name { get; init; } = "";
    public int SortOrder { get; init; }
    public bool IsActive { get; init; }
}
