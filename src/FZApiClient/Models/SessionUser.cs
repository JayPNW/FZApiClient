using System.Text.Json.Serialization;

namespace FZApi.Models;

public record SessionUser
{
    public string Id { get; init; } = "";
    public string Username { get; init; } = "";

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? DisplayName { get; init; }

    public string Role { get; init; } = "";
}
