namespace FZApi.Models;

public record GuildProgressionGuild
{
    public Guid Id { get; init; }
    public string Name { get; init; } = "";
    public string Slug { get; init; } = "";
    public ServerRef Server { get; init; } = new();
}
