namespace FZApi.Models;

public record RosterFilters
{
    public int Days { get; init; }
    public bool IncludePets { get; init; }
    public int Limit { get; init; }
}
