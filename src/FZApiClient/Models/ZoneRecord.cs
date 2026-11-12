namespace FZApi.Models;

public record ZoneRecord
{
    public Guid Id { get; init; }
    public string Slug { get; init; } = "";

    /// <summary>The name to use as <see cref="BonusReport.ZoneName"/> when reporting.</summary>
    public string Name { get; init; } = "";

    public Guid ExpansionId { get; init; }
    public int? MinLevel { get; init; }
    public int? MaxLevel { get; init; }

    /// <summary><c>Indoor</c>, <c>Outdoor</c>, or null when not known.</summary>
    public string? ZoneType { get; init; }

    public int? LegacyZoneId { get; init; }
    public bool IsActive { get; init; }
    public DateTimeOffset CreatedAt { get; init; }
    public DateTimeOffset UpdatedAt { get; init; }
}
