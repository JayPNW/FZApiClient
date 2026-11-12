namespace FZApi.Models;

public record BonusReport
{
    /// <summary>The <see cref="Server.Name"/>, not the slug.</summary>
    public required string ServerName { get; init; }

    public required string ZoneName { get; init; }

    /// <summary>A <see cref="BonusType.Key"/>, including <c>none</c>. Not <c>unconfirmed</c>.</summary>
    public required string BonusKey { get; init; }
}
