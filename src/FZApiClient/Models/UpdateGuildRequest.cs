namespace FZApi.Models;

public record UpdateGuildRequest
{
    public required string Name { get; init; }

    /// <summary>Changing the slug changes the guild's public address.</summary>
    public required string Slug { get; init; }

    public required string Description { get; init; }
}
