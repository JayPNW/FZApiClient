using System.Text.Json;

namespace FZApi.Models;

public record EncounterSummary
{
    public Guid Id { get; init; }
    public Guid ReportId { get; init; }
    public string Zone { get; init; } = "";
    public string TargetName { get; init; } = "";
    public DateTimeOffset StartedAt { get; init; }
    public DateTimeOffset EndedAt { get; init; }
    public int DurationSec { get; init; }
    public string Result { get; init; } = "";
    public long TotalDamage { get; init; }
    public double Dps { get; init; }
    public long TotalHealing { get; init; }
    public double Hps { get; init; }
    public int PlayerCount { get; init; }
    public bool RankingEligible { get; init; }
    public string? RankingIneligibleReason { get; init; }
    public int ParticipantCount { get; init; }
    public int ContributorCount { get; init; }

    public string CoverageLabel { get; init; } = "";

    public ServerRef Server { get; init; } = new();
    public GuildRef? Guild { get; init; }
    public JsonElement? RaidBoss { get; init; }
    public JsonElement? RaidTier { get; init; }
    public JsonElement? RankingPartition { get; init; }
    public DateTimeOffset CreatedAt { get; init; }
    public DateTimeOffset UpdatedAt { get; init; }
}
