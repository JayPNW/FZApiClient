namespace FZApi.Models;

public record Zone : ZoneRecord
{
    public string Expansion { get; init; } = "";
    public string ExpansionSlug { get; init; } = "";
}
