using System.Text.Json;
using System.Text.Json.Serialization;

namespace FZApi.Json;

internal class EmptyObjectAsNullConverter<T> : JsonConverter<T?> where T : class
{
    public override bool HandleNull => true;

    public override T? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.Null)
        {
            return null;
        }

        using var document = JsonDocument.ParseValue(ref reader);
        var element = document.RootElement;
        if (element.ValueKind == JsonValueKind.Object && !element.EnumerateObject().Any())
        {
            return null;
        }

        return element.Deserialize<T>(options);
    }

    public override void Write(Utf8JsonWriter writer, T? value, JsonSerializerOptions options)
    {
        if (value is null)
        {
            writer.WriteStartObject();
            writer.WriteEndObject();
        }
        else
        {
            JsonSerializer.Serialize(writer, value, options);
        }
    }
}
