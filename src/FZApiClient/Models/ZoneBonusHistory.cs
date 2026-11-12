namespace FZApi.Models;

public record ZoneBonusHistory
{
    public Zone Zone { get; init; } = new();

    /// <summary>Newest first.</summary>
    public IReadOnlyList<ZoneBonusHistoryEntry> History { get; init; } = [];

    /// <summary>Bonus types the zone never had are left out.</summary>
    public IReadOnlyList<ZoneBonusStat> Stats { get; init; } = [];
}
