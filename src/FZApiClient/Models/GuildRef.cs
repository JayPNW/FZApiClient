namespace FZApi.Models;

public record GuildRef
{
    public Guid Id { get; init; }
    public string Name { get; init; } = "";
    public string Slug { get; init; } = "";
}
