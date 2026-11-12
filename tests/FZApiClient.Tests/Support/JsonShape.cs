using System.Text.Json;
using System.Text.Json.Nodes;
using FZApi.Json;

namespace FZApi.Tests.Support;

internal static class JsonShape
{
    public static JsonNode? Serialize(object? model)
        => model is null ? null : JsonSerializer.SerializeToNode(model, model.GetType(), FZApiJson.Options);

    public static IReadOnlyList<string> Differences(JsonNode? api, object? model)
    {
        var differences = new List<string>();
        Compare(api, Serialize(model), "$", differences);
        return differences;
    }

    public static void AssertCovers(string responseBody, string dataPath, object? model)
        => AssertCovers(At(responseBody.Length == 0 ? null : JsonNode.Parse(responseBody), dataPath), model);

    public static void AssertCovers(JsonNode? api, object? model)
    {
        var differences = Differences(api, model);
        Assert.True(differences.Count == 0, "The model and the API response differ:\n" + string.Join("\n", differences));
    }

    private static void Compare(JsonNode? api, JsonNode? model, string path, List<string> differences)
    {
        switch (api, model)
        {
            case (JsonObject apiObject, JsonObject modelObject):
                foreach (var (name, value) in apiObject)
                {
                    if (modelObject.ContainsKey(name))
                    {
                        Compare(value, modelObject[name], $"{path}.{name}", differences);
                    }
                    else if (value is not null)
                    {
                        differences.Add($"{path}.{name}: sent by the API, but not in the model");
                    }
                }

                foreach (var (name, _) in modelObject)
                {
                    if (!apiObject.ContainsKey(name))
                    {
                        differences.Add($"{path}.{name}: in the model, but not sent by the API");
                    }
                }

                break;

            case (JsonArray apiArray, JsonArray modelArray):
                if (apiArray.Count != modelArray.Count)
                {
                    differences.Add($"{path}: the API sent {apiArray.Count} items, the model has {modelArray.Count}");
                }

                for (var i = 0; i < Math.Min(apiArray.Count, modelArray.Count); i++)
                {
                    Compare(apiArray[i], modelArray[i], $"{path}[{i}]", differences);
                }

                break;
        }
    }

    public static JsonNode? At(JsonNode? root, string path)
    {
        if (path == "$")
        {
            return root;
        }

        var node = root;
        foreach (var part in path.Split('.'))
        {
            node = node?[part];
        }

        return node;
    }
}
