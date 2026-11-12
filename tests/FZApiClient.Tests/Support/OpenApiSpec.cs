using System.Globalization;
using System.Text.Json.Nodes;
using System.Text.RegularExpressions;
using YamlDotNet.Core;
using YamlDotNet.RepresentationModel;

namespace FZApi.Tests.Support;

internal static partial class OpenApiSpec
{
    private static readonly string[] Methods = ["get", "put", "post", "delete", "patch"];

    private static readonly Lazy<IReadOnlyDictionary<string, SpecOperation>> LazyOperations = new(Load);

    public static IReadOnlyDictionary<string, SpecOperation> Operations => LazyOperations.Value;

    private static IReadOnlyDictionary<string, SpecOperation> Load()
    {
        var yaml = new YamlStream();
        using (var reader = File.OpenText(Path.Combine(AppContext.BaseDirectory, "openapi.yaml")))
        {
            yaml.Load(reader);
        }

        var root = (YamlMappingNode)yaml.Documents[0].RootNode;
        var paths = (YamlMappingNode)root[new YamlScalarNode("paths")];
        var operations = new Dictionary<string, SpecOperation>();
        foreach (var (pathKey, pathValue) in paths.Children)
        {
            var path = ((YamlScalarNode)pathKey).Value!;
            foreach (var (methodKey, methodValue) in ((YamlMappingNode)pathValue).Children)
            {
                var method = ((YamlScalarNode)methodKey).Value!;
                if (!Methods.Contains(method))
                {
                    continue;
                }

                var operation = (YamlMappingNode)methodValue;
                var id = ((YamlScalarNode)operation[new YamlScalarNode("operationId")]).Value!;
                var example = Find(operation, "responses", "200", "content", "application/json", "example");
                operations.Add(id, new SpecOperation(id, method.ToUpperInvariant(), path, example is null ? null : ToJson(example)));
            }
        }

        return operations;
    }

    private static YamlNode? Find(YamlNode node, params string[] keys)
    {
        foreach (var key in keys)
        {
            if (node is not YamlMappingNode mapping || !mapping.Children.TryGetValue(new YamlScalarNode(key), out var child))
            {
                return null;
            }

            node = child;
        }

        return node;
    }

    private static JsonNode? ToJson(YamlNode node) => node switch
    {
        YamlMappingNode mapping => new JsonObject(
            mapping.Children.Select(c => KeyValuePair.Create(((YamlScalarNode)c.Key).Value!, ToJson(c.Value)))),
        YamlSequenceNode sequence => new JsonArray([.. sequence.Children.Select(ToJson)]),
        YamlScalarNode scalar => ScalarToJson(scalar),
        _ => throw new NotSupportedException(node.GetType().Name),
    };

    private static JsonNode? ScalarToJson(YamlScalarNode scalar)
    {
        var value = scalar.Value ?? "";
        if (scalar.Style != ScalarStyle.Plain)
        {
            return JsonValue.Create(value);
        }

        if (value is "" or "~" or "null")
        {
            return null;
        }

        if (value is "true" or "false")
        {
            return JsonValue.Create(value == "true");
        }

        if (IntegerPattern().IsMatch(value))
        {
            return JsonValue.Create(long.Parse(value, CultureInfo.InvariantCulture));
        }

        if (FloatPattern().IsMatch(value))
        {
            return JsonValue.Create(double.Parse(value, CultureInfo.InvariantCulture));
        }

        return JsonValue.Create(value);
    }

    [GeneratedRegex(@"^-?\d+$")]
    private static partial Regex IntegerPattern();

    [GeneratedRegex(@"^-?\d+(\.\d+)?([eE][-+]?\d+)?$")]
    private static partial Regex FloatPattern();
}
