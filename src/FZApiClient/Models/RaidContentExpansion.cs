namespace FZApi.Models;

public record RaidContentExpansion
{
    public Guid Id { get; init; }
    public Guid ExpansionId { get; init; }
    public string Slug { get; init; } = "";
    public string Name { get; init; } = "";
    public int SortOrder { get; init; }
    public bool GloballyActive { get; init; }
    public bool IsActive { get; init; }
    public bool EffectivelyActive { get; init; }
    public DateTimeOffset? UnlocksAt { get; init; }
    public string? Notes { get; init; }
}
