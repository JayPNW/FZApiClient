namespace FZApi.Models;

public record ServerProgression
{
    public ServerRef Server { get; init; } = new();
    public ProgressionTree Progression { get; init; } = new();
    public RaidContent RaidContent { get; init; } = new();
}
