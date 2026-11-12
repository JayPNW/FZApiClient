namespace FZApi.Models;

public record IdSlugName
{
    public Guid Id { get; init; }
    public string Name { get; init; } = "";
    public string Slug { get; init; } = "";
}
