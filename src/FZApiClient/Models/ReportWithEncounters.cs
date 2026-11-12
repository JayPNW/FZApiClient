namespace FZApi.Models;

public record ReportWithEncounters
{
    public Report Report { get; init; } = new();
    public IReadOnlyList<EncounterSummary> Encounters { get; init; } = [];
}
