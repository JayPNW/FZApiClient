namespace FZApi.Models;

public record Server
{
    public int Id { get; init; }

    /// <summary>The name to use as <see cref="BonusReport.ServerName"/> when reporting.</summary>
    public string Name { get; init; } = "";

    /// <summary>The name to use in paths, such as the <c>server</c> argument of most <see cref="FZApiClient"/> methods.</summary>
    public string Slug { get; init; } = "";

    public IReadOnlyList<string> Aliases { get; init; } = [];
    public bool IsActive { get; init; }
    public DateTimeOffset CreatedAt { get; init; }
    public DateTimeOffset UpdatedAt { get; init; }
}
