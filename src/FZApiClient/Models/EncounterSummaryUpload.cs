namespace FZApi.Models;

public record EncounterSummaryUpload
{
    public string Event { get; init; } = "encounter_summary";

    /// <summary>The client's own ID for the fight, unique per upload. fzClient sends a UUID without dashes.</summary>
    public required string EncounterId { get; init; }

    public required string TargetName { get; init; }

    /// <summary>The zone name from the log, or <c>unknown</c>.</summary>
    public required string Zone { get; init; }

    public string? CharacterName { get; init; }
    public string? Character { get; init; }
    public string? ServerName { get; init; }
    public string? Server { get; init; }

    /// <summary>An EverQuest class such as <c>Shadow Knight</c>, or empty when not known.</summary>
    public string? CharacterClass { get; init; }

    public string? CharacterClassSource { get; init; }
    public double? CharacterClassConfidence { get; init; }
    public IReadOnlyList<string>? CharacterClassEvidence { get; init; }
    public string? ClientVersion { get; init; }

    /// <summary>The report to add the encounter to, by name. Empty to let the site choose.</summary>
    public string? ReportName { get; init; }

    public ClientProvenance? ClientProvenance { get; init; }
    public required DateTimeOffset StartedAt { get; init; }
    public required DateTimeOffset EndedAt { get; init; }
    public required double DurationSec { get; init; }
    public long? TotalDamage { get; init; }
    public double? Dps { get; init; }
    public long? TotalHealing { get; init; }
    public double? Hps { get; init; }
    public required IReadOnlyList<EncounterParticipantUpload> Participants { get; init; }
    public IReadOnlyList<ClassEvidenceUpload>? ClassEvidence { get; init; }
}
