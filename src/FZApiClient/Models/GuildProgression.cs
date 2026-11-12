namespace FZApi.Models;

public record GuildProgression
{
    public GuildProgressionGuild Guild { get; init; } = new();
    public IReadOnlyList<GuildObjective> Objectives { get; init; } = [];
}
