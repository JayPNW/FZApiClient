namespace FZApi.Models;

public record ZoneBonusStat
{
    public Guid BonusTypeId { get; init; }
    public string Bonus { get; init; } = "";
    public string BonusLabel { get; init; } = "";
    public int Count { get; init; }
}
