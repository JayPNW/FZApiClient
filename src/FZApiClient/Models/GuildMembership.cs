namespace FZApi.Models;

public record GuildMembership
{
    public string Id { get; init; } = "";

    /// <summary><c>MEMBER</c>, <c>OFFICER</c>, <c>ADMIN</c> or <c>OWNER</c>.</summary>
    public string Role { get; init; } = "";

    public Guild Guild { get; init; } = new();
}
