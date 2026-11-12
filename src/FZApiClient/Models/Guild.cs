namespace FZApi.Models;

public record Guild
{
    /// <summary>The guild's ID. Operations that take a guild need this, not the slug.</summary>
    public Guid Id { get; init; }

    public string Name { get; init; } = "";
    public string Slug { get; init; } = "";
    public string? Description { get; init; }
    public ServerRef Server { get; init; } = new();
    public GuildCounts Counts { get; init; } = new();
    public DateTimeOffset CreatedAt { get; init; }
    public DateTimeOffset UpdatedAt { get; init; }
}
