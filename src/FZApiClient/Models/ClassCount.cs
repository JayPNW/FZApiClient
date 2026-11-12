namespace FZApi.Models;

public record ClassCount
{
    public string? ClassName { get; init; }
    public int Count { get; init; }
    public string Label { get; init; } = "";
}
