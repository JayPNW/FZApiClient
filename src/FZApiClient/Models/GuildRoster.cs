using System.Text.Json;

namespace FZApi.Models;

public record GuildRoster
{
    public Guild Guild { get; init; } = new();
    public RosterFilters Filters { get; init; } = new();
    public IReadOnlyList<ClassCount> ClassCounts { get; init; } = [];
    public IReadOnlyList<JsonElement> Characters { get; init; } = [];
    public int TotalCharacters { get; init; }
}
