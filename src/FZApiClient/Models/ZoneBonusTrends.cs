namespace FZApi.Models;

public record ZoneBonusTrends
{
    public IReadOnlyList<BonusType> BonusTypes { get; init; } = [];
    public IReadOnlyList<ZoneBonusTrend> Zones { get; init; } = [];
}
