using System.Text.Json.Serialization;

namespace FZApi.Models;

public record ZoneBonus
{
    public Guid Id { get; init; }
    public int ServerId { get; init; }
    public Guid ZoneId { get; init; }
    public Guid BonusTypeId { get; init; }

    /// <summary>The bonus day as midnight UTC. Use <see cref="BonusDay"/> for the date.</summary>
    public DateTimeOffset BonusDate { get; init; }

    public string State { get; init; } = "";
    public Guid CreatedById { get; init; }
    public Guid? RemovedById { get; init; }
    public DateTimeOffset? RemovedAt { get; init; }
    public int? LegacyBonusId { get; init; }
    public DateTimeOffset CreatedAt { get; init; }
    public DateTimeOffset UpdatedAt { get; init; }

    [JsonIgnore]
    public DateOnly BonusDay => DateOnly.FromDateTime(BonusDate.UtcDateTime);
}
