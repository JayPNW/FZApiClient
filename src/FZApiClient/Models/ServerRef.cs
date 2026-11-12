namespace FZApi.Models;

public record ServerRef
{
    public int Id { get; init; }
    public string Name { get; init; } = "";
    public string Slug { get; init; } = "";
}
