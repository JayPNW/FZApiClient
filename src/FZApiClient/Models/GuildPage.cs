using System.Text.Json;

namespace FZApi.Models;

public record GuildPage
{
    public Guild Guild { get; init; } = new();
    public IReadOnlyList<GuildMember> Members { get; init; } = [];
    public IReadOnlyList<JsonElement> UploadCodes { get; init; } = [];
    public IReadOnlyList<EncounterSummary> RecentEncounters { get; init; } = [];
    public IReadOnlyList<JsonElement> RecentBossKills { get; init; } = [];
    public IReadOnlyList<JsonElement> RecentObjectiveCompletions { get; init; } = [];
}
