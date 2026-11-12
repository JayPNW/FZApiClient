namespace FZApi.Models;

public record RaidBossRef
{
    /// <summary>The raid boss <c>id</c> the rankings and character operations take.</summary>
    public Guid Id { get; init; }

    public string Slug { get; init; } = "";
    public string Name { get; init; } = "";
    public bool IsProgressionRelevant { get; init; }
    public bool IsLeaderboardRelevant { get; init; }
    public RaidZone RaidZone { get; init; } = new();
}
