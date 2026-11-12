namespace FZApi.Models;

public record EncounterParticipantUpload
{
    public required string Name { get; init; }
    public long Damage { get; init; }
    public double Dps { get; init; }
    public long Healing { get; init; }
    public double Hps { get; init; }
    public string? TopAbility { get; init; }
    public string? TopHealAbility { get; init; }
    public int? HitCount { get; init; }
    public int? HealCount { get; init; }
    public double? CritPct { get; init; }
    public double? HealCritPct { get; init; }

    /// <summary>False for a known pet.</summary>
    public bool IsPlayer { get; init; } = true;

    public string? Owner { get; init; }
}
