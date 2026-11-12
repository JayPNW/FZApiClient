using System.Text.Json;

namespace FZApi.Models;

public record RequiredBoss
{
    public Guid Id { get; init; }
    public int SortOrder { get; init; }
    public bool IsRequired { get; init; }
    public bool Killed { get; init; }
    public DateTimeOffset? KilledAt { get; init; }
    public JsonElement? FirstKill { get; init; }
    public IReadOnlyList<JsonElement> TopKills { get; init; } = [];
    public RaidBossRef RaidBoss { get; init; } = new();
}
