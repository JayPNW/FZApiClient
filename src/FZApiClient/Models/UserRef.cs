namespace FZApi.Models;

public record UserRef
{
    public Guid Id { get; init; }
    public string Username { get; init; } = "";
}
