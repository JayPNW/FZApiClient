namespace FZApi.Models;

public record Report
{
    public Guid Id { get; init; }
    public string Name { get; init; } = "";
    public string DisplayName { get; init; } = "";
    public string Visibility { get; init; } = "";
    public DateTimeOffset StartedAt { get; init; }
    public DateTimeOffset EndedAt { get; init; }
    public DateTimeOffset FirstUploadAt { get; init; }
    public DateTimeOffset LastUploadAt { get; init; }
    public int UploadCount { get; init; }
    public int SubmissionCount { get; init; }
    public int EncounterCount { get; init; }
    public ServerRef Server { get; init; } = new();
    public GuildRef? Guild { get; init; }
    public UserRef? CreatedBy { get; init; }
    public DateTimeOffset CreatedAt { get; init; }
    public DateTimeOffset UpdatedAt { get; init; }
    public bool IsInvalid { get; init; }
    public bool IsHidden { get; init; }
    public DateTimeOffset? ModeratedAt { get; init; }
    public Guid? ModeratedById { get; init; }
    public string? ModerationReason { get; init; }
    public DateTimeOffset? DeletedAt { get; init; }
    public Guid? DeletedById { get; init; }
    public string? DeleteReason { get; init; }
    public int OpenContentReportCount { get; init; }
}
