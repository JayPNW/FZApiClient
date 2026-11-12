namespace FZApi.Models;

public record BonusTypeRecord : BonusType
{
    public bool IsSystem { get; init; }
    public DateTimeOffset CreatedAt { get; init; }
    public DateTimeOffset UpdatedAt { get; init; }
}
