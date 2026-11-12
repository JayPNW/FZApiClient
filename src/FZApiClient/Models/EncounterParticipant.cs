using System.Text.Json;

namespace FZApi.Models;

public record EncounterParticipant
{
    public Guid Id { get; init; }
    public string Name { get; init; } = "";
    public long Damage { get; init; }
    public double Dps { get; init; }
    public long Healing { get; init; }
    public double Hps { get; init; }
    public string TopAbility { get; init; } = "";
    public string TopHealAbility { get; init; } = "";
    public int HitCount { get; init; }
    public int HealCount { get; init; }
    public double CritPercent { get; init; }
    public double HealCritPercent { get; init; }
    public bool IsPlayer { get; init; }
    public string ParticipantKind { get; init; } = "";
    public string? OwnerName { get; init; }

    public ParticipantCharacter? Character { get; init; }

    public JsonElement? OwnerCharacter { get; init; }
    public JsonElement? ParseRecord { get; init; }
    public JsonElement? HealingParseRecord { get; init; }
    public IReadOnlyList<JsonElement> ParseRecords { get; init; } = [];
    public ParticipantMetrics Metrics { get; init; } = new();
}
