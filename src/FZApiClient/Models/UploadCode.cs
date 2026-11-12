using System.Text.Json.Serialization;

namespace FZApi.Models;

public record UploadCode
{
    public string Id { get; init; } = "";
    public string? Label { get; init; }
    public bool IsActive { get; init; }
    public DateTimeOffset CreatedAt { get; init; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public DateTimeOffset? LastUsedAt { get; init; }

    /// <summary>The code itself. Present only when the code is created.</summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Code { get; init; }
}
