namespace FZApi.Models;

public record EncounterDetail : EncounterSummary
{
    public IReadOnlyList<EncounterParticipant> Participants { get; init; } = [];
    public IReadOnlyList<EncounterSubmission> Submissions { get; init; } = [];
}
