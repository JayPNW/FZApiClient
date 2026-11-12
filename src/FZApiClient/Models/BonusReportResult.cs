using System.Text.Json.Serialization;
using FZApi.Json;

namespace FZApi.Models;

public record BonusReportResult
{
    /// <summary>False when the site already showed this bonus for the zone, so nothing changed.</summary>
    public bool Updated { get; init; }

    [JsonConverter(typeof(EmptyObjectAsNullConverter<ZoneBonus>))]
    public ZoneBonus? ZoneBonus { get; init; }
}
