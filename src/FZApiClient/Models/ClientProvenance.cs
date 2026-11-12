namespace FZApi.Models;

public record ClientProvenance
{
    public required string SessionId { get; init; }

    /// <summary><c>Live</c> or <c>Replay</c>.</summary>
    public required string Mode { get; init; }

    public required DateTimeOffset StartedAtUtc { get; init; }
    public required DateTimeOffset LastObservedAtUtc { get; init; }
    public DateTimeOffset? FirstObservedAtUtc { get; init; }
    public DateTimeOffset? LastLineObservedAtUtc { get; init; }
    public DateTimeOffset? FirstLogTimestamp { get; init; }
    public DateTimeOffset? LastLogTimestamp { get; init; }
    public int? LinesObserved { get; init; }
    public int? TimestampedLines { get; init; }
    public int? ParsedEvents { get; init; }
    public int? OutOfOrderTimestampCount { get; init; }
    public int? LargeLogGapCount { get; init; }
    public int? LargeReceiveGapCount { get; init; }
    public int? BurstLineCount { get; init; }
    public int? SuspiciousBurstCount { get; init; }
    public double? ObservedWallClockSeconds { get; init; }
    public double? LogTimeSpanSeconds { get; init; }
    public double? AverageLinesPerSecond { get; init; }
    public double? AverageParsedEventsPerSecond { get; init; }
    public double? AverageLogLinesPerSecond { get; init; }
    public int? MaxLinesPerSecond { get; init; }
    public double? TrustScore { get; init; }

    /// <summary><c>LiveEligible</c>, <c>LocalReplay</c> or <c>LiveSuspicious</c>.</summary>
    public required string Eligibility { get; init; }

    public required bool IsUploadEligible { get; init; }
}
