using System.Text.Json.Serialization;

namespace FZApi.Models;

public record BonusAction
{
    public Guid Id { get; init; }
    public int ServerId { get; init; }
    public Guid ZoneId { get; init; }
    public Guid? ZoneBonusId { get; init; }
    public Guid? BonusTypeId { get; init; }
    public Guid ActorId { get; init; }

    /// <summary>
    /// <c>ADD</c>, <c>REMOVE</c>, <c>FLAG_REMOVAL</c>, <c>CONFIRM_REMOVAL</c>, <c>CANCEL_REMOVAL</c>, <c>ADMIN_REMOVE</c>,
    /// <c>ADMIN_RESTORE</c>, <c>REPORT</c> or <c>IMPORT</c>.
    /// </summary>
    public string ActionType { get; init; } = "";

    /// <summary>The bonus day as midnight UTC.</summary>
    public DateTimeOffset BonusDate { get; init; }

    public string? Reason { get; init; }
    public BonusActionMetadata? Metadata { get; init; }
    public DateTimeOffset CreatedAt { get; init; }
    public UserSummary Actor { get; init; } = new();
    public ZoneRecord Zone { get; init; } = new();
    public BonusTypeRecord? BonusType { get; init; }
    public bool IsReportable { get; init; }

    [JsonIgnore]
    public DateOnly BonusDay => DateOnly.FromDateTime(BonusDate.UtcDateTime);
}
