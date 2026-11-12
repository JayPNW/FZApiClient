using System.Text.Json.Nodes;
using System.Text.RegularExpressions;

namespace FZApi.Tests.Support;

internal record SpecOperation(string Id, string Method, string PathTemplate, JsonNode? Example)
{
    public bool Matches(string path)
        => new Regex("^" + Regex.Replace(Regex.Escape(PathTemplate), @"\\\{[^/]+?}", "[^/]+") + "$").IsMatch(path);
}
