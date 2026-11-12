using System.Globalization;
using System.Text;
using FZApi.Models;

namespace FZApi.Json;

internal class QueryString
{
    private readonly StringBuilder _builder = new();

    public QueryString Add(string name, string? value)
    {
        if (value is null)
        {
            return this;
        }

        _builder.Append(_builder.Length == 0 ? '?' : '&');
        _builder.Append(Uri.EscapeDataString(name)).Append('=').Append(Uri.EscapeDataString(value));
        return this;
    }

    public QueryString Add(string name, int? value)
        => Add(name, value?.ToString(CultureInfo.InvariantCulture));

    public QueryString Add(string name, bool? value)
        => Add(name, value switch { null => null, true => "true", false => "false" });

    public QueryString Add(string name, Guid? value)
        => Add(name, value?.ToString("D"));

    public QueryString Add(string name, DateOnly? value)
        => Add(name, value?.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture));

    public QueryString Add(string name, ParseMetric? value)
        => Add(name, value?.ToString().ToUpperInvariant());

    public QueryString AddEach(string name, IEnumerable<string>? values)
    {
        foreach (var value in values ?? [])
        {
            Add(name, value);
        }

        return this;
    }

    public override string ToString() => _builder.ToString();
}
