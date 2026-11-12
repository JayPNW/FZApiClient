namespace FZApi.Models;

public record GuildMember
{
    /// <summary>The membership's ID, for <see cref="FZApiClient.RemoveGuildMemberAsync"/>.</summary>
    public Guid Id { get; init; }

    /// <summary><c>MEMBER</c>, <c>OFFICER</c>, <c>ADMIN</c> or <c>OWNER</c>.</summary>
    public string Role { get; init; } = "";

    public UserSummary User { get; init; } = new();
    public DateTimeOffset CreatedAt { get; init; }
    public DateTimeOffset UpdatedAt { get; init; }
}
