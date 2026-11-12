namespace FZApi.Models;

public record AllStarQuery
{
    public required int ServerId { get; init; }

    /// <summary>A <see cref="RankingPartition.RankingPartitionId"/> (not its <c>Id</c>).</summary>
    public required Guid RankingPartitionId { get; init; }

    public Guid? ExpansionId { get; init; }
    public ParseMetric? Metric { get; init; }
    public DateOnly? SnapshotDate { get; init; }
    public int? Limit { get; init; }

    public int? Page { get; init; }

    public string? ClassName { get; init; }
}
