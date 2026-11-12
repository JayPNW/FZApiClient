namespace FZApi.Models;

public record ReportUpload
{
    public Guid Id { get; init; }
    public string Destination { get; init; } = "";
    public string Visibility { get; init; } = "";
    public string ClientVersion { get; init; } = "";
    public string IntegrityStatus { get; init; } = "";
    public bool RankingEligible { get; init; }
    public string? RankingIneligibleReason { get; init; }
    public DateTimeOffset ReceivedAt { get; init; }
}
