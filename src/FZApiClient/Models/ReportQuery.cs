namespace FZApi.Models;

public record ReportQuery
{
    public int? Limit { get; init; }

    public string? ServerSlug { get; init; }
    public int? ServerId { get; init; }
    public Guid? GuildId { get; init; }
}
