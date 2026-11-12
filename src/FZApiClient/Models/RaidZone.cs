namespace FZApi.Models;

public record RaidZone
{
    public Guid Id { get; init; }
    public string Slug { get; init; } = "";
    public string Name { get; init; } = "";
    public string? ShortName { get; init; }
}
