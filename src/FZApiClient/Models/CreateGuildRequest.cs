using System.Text.Json.Serialization;

namespace FZApi.Models;

public record CreateGuildRequest
{
    public required int ServerId { get; init; }

    public required string Name { get; init; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Description { get; init; }
}
