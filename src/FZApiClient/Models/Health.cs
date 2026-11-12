namespace FZApi.Models;

public record Health
{
    public bool Ok { get; init; }
    public string Status { get; init; } = "";
}
