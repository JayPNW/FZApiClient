namespace FZApi.Models;

public record ParticipantCharacter
{
    public Guid Id { get; init; }
    public string Name { get; init; } = "";
    public string NormalizedName { get; init; } = "";
    public string? ClassName { get; init; }
    public int? Level { get; init; }
    public bool IsPet { get; init; }
    public string? OwnerName { get; init; }
}
