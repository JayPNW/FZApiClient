namespace FZApi.Models;

public record BonusType
{
    /// <summary>The same on every server.</summary>
    public Guid Id { get; init; }

    /// <summary>Get the list from the API rather than hard-coding it.</summary>
    public string Key { get; init; } = "";

    public string Label { get; init; } = "";

    /// <summary>Lower numbers come first when displayed.</summary>
    public int SortOrder { get; init; }
}
