# FZApiClient

[![NuGet](https://img.shields.io/nuget/v/FZApiClient.svg)](https://www.nuget.org/packages/FZApiClient)
[![Build and Test](https://github.com/JayPNW/FZApiClient/actions/workflows/build-and-test.yml/badge.svg)](https://github.com/JayPNW/FZApiClient/actions/workflows/build-and-test.yml)

[OpenAPI documentation](openapi.yaml) and a A .NET client for the [Frostreaver.zone](https://frostreaver.zone)/[fangbreaker.zone](https://fangbreaker.zone) API.

## .Net usage example

<!-- snippet: read-todays-bonuses -->
<a id='snippet-read-todays-bonuses'></a>
```cs
using var http = new HttpClient();
var client = new FZApiClient(http);

var today = await client.GetTodaysBonusesAsync("fangbreaker");
foreach (var zone in today.Where(z => z.IsConfirmed && z.Bonus != "none"))
{
    Console.WriteLine($"{zone.Name}: {zone.BonusLabel}");
}
```
<sup><a href='/tests/FZApiClient.Tests/UsageExamples.cs#L15-L24' title='Snippet source file'>snippet source</a> | <a href='#snippet-read-todays-bonuses' title='Start of snippet'>anchor</a></sup>
<!-- endSnippet -->
