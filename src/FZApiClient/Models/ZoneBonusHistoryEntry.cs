namespace FZApi.Models;

public record ZoneBonusHistoryEntry
{
    public Guid Id { get; init; }
    public DateOnly BonusDate { get; init; }
    public string Bonus { get; init; } = "";
    public string BonusLabel { get; init; } = "";
    public string State { get; init; } = "";
}
