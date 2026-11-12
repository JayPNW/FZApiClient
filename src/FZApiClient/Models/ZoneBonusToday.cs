using System.Text.Json;
using System.Text.Json.Serialization;

namespace FZApi.Models;

public record ZoneBonusToday
{
    /// <summary>Null when nobody has reported the zone today.</summary>
    public Guid? Id { get; init; }

    public Guid ZoneId { get; init; }
    public string ZoneSlug { get; init; } = "";
    public string Name { get; init; } = "";
    public string Expansion { get; init; } = "";
    public Guid ExpansionId { get; init; }
    public string ExpansionSlug { get; init; } = "";
    public int? MinLevel { get; init; }
    public int? MaxLevel { get; init; }
    public string? ZoneType { get; init; }
    public Guid? BonusTypeId { get; init; }

    /// <summary>A bonus key, or <c>unconfirmed</c> when nobody has reported the zone today.</summary>
    public string Bonus { get; init; } = "";

    public string BonusLabel { get; init; } = "";

    /// <summary><c>ACTIVE</c> or <c>UNCONFIRMED</c>. Treat any other value as <c>UNCONFIRMED</c>.</summary>
    public string State { get; init; } = "";

    public DateOnly BonusDate { get; init; }

    /// <summary>Reserved by the site. Currently always null.</summary>
    public JsonElement? PendingRemovalBy { get; init; }

    /// <summary>Whether someone reported this zone's bonus, including a report of <c>none</c>.</summary>
    [JsonIgnore]
    public bool IsConfirmed => State == "ACTIVE";
}
