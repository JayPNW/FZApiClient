namespace FZApi.Models;

public record ExpansionRef
{
    public Guid Id { get; init; }
    public string Slug { get; init; } = "";
    public string Name { get; init; } = "";
    public int SortOrder { get; init; }
}
