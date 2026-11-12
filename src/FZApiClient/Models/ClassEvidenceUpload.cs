namespace FZApi.Models;

public record ClassEvidenceUpload
{
    public required string CharacterName { get; init; }
    public required string SuggestedClass { get; init; }
    public required string Source { get; init; }
    public required double Confidence { get; init; }
    public required string EvidenceType { get; init; }
    public required string EvidenceText { get; init; }
    public required string Tier { get; init; }
}
