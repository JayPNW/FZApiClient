using System.Text.Json;

namespace FZApi.Models;

public record CharacterProfile
{
    public Guid Id { get; init; }
    public string Name { get; init; } = "";
    public string NormalizedName { get; init; } = "";
    public string? ClassName { get; init; }
    public string? ClassSource { get; init; }

    /// <summary>A number, a string or null.</summary>
    public JsonElement? ClassConfidence { get; init; }

    public bool ClassLocked { get; init; }
    public DateTimeOffset? ClassUpdatedAt { get; init; }
    public int? Level { get; init; }
    public bool IsPet { get; init; }
    public string? OwnerName { get; init; }
    public ServerRef Server { get; init; } = new();
    public GuildRef? Guild { get; init; }
    public CharacterCounts Counts { get; init; } = new();
    public DateTimeOffset CreatedAt { get; init; }
    public DateTimeOffset UpdatedAt { get; init; }
}
