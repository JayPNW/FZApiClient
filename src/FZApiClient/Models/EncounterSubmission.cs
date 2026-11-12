namespace FZApi.Models;

public record EncounterSubmission
{
    public Guid Id { get; init; }
    public string ClientEncounterId { get; init; } = "";
    public DateTimeOffset ReceivedAt { get; init; }
    public DateTimeOffset MergedAt { get; init; }
    public UserRef UploadedBy { get; init; } = new();
    public ReportUpload ReportUpload { get; init; } = new();
}
