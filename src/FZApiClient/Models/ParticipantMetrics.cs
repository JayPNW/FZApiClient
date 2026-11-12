namespace FZApi.Models;

public record ParticipantMetrics
{
    public long DisplayDamage { get; init; }
    public double DamageSharePercent { get; init; }
    public long DisplayHealing { get; init; }
    public double HealingSharePercent { get; init; }
    public long DirectDamage { get; init; }
    public long PetDamage { get; init; }
    public long TotalDamage { get; init; }
    public int DirectHitCount { get; init; }
    public int PetHitCount { get; init; }
    public int TotalHitCount { get; init; }
    public long DirectHealing { get; init; }
    public long PetHealing { get; init; }
    public long TotalHealing { get; init; }
    public int DirectHealCount { get; init; }
    public int PetHealCount { get; init; }
    public int TotalHealCount { get; init; }
    public double Hps { get; init; }
}
