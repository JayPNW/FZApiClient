namespace FZApi.Models;

public record UserSummary
{
    public Guid Id { get; init; }
    public string Username { get; init; } = "";

    /// <summary><c>MEMBER</c>, <c>MODERATOR</c>, <c>ADMINISTRATOR</c> or <c>SYSOP</c>.</summary>
    public string Role { get; init; } = "";
}
