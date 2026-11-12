namespace FZApi.Models;

public record ProgressionTree
{
    public IReadOnlyList<ProgressionExpansion> Expansions { get; init; } = [];
}
