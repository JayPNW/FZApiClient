// The examples are written the way callers would write them, without a test's cancellation token.
#pragma warning disable xUnit1051

using FZApi.Models;
using FZApi.Tests.Support;

namespace FZApi.Tests;

public class UsageExamples
{
    [Fact]
    [Trait("Category", "Live")]
    public async Task ReadTodaysBonuses()
    {
        // begin-snippet: read-todays-bonuses
        using var http = new HttpClient();
        var client = new FZApiClient(http);

        var today = await client.GetTodaysBonusesAsync("fangbreaker");
        foreach (var zone in today.Where(z => z.IsConfirmed && z.Bonus != "none"))
        {
            Console.WriteLine($"{zone.Name}: {zone.BonusLabel}");
        }
        // end-snippet

        Assert.NotNull(today);
    }

    [Fact]
    public async Task ReportABonus()
    {
        const string apiKey = "fz_live_example";
        using var http = new HttpClient(new StubHandler(body: """{"ok":true,"data":{"updated":true,"zoneBonus":{}}}"""));

        // begin-snippet: report-a-bonus
        var client = new FZApiClient(http, new FZApiClientOptions { ApiKey = apiKey });

        var result = await client.ReportBonusAsync(new BonusReport
        {
            ServerName = "Fangbreaker",
            ZoneName = "Befallen",
            BonusKey = "experience",
        });
        // end-snippet

        Assert.True(result.Updated);
    }

    [Fact]
    public async Task HandleErrors()
    {
        using var http = new HttpClient(new StubHandler(
            System.Net.HttpStatusCode.NotFound, """{"ok":false,"error":"Server not found"}"""));
        var client = new FZApiClient(http);
        string? message = null;

        // begin-snippet: handle-errors
        try
        {
            await client.GetZonesAsync("no-such-server");
        }
        catch (FZApiException e) when (e.StatusCode == System.Net.HttpStatusCode.NotFound)
        {
            message = e.Error; // "Server not found"
        }
        // end-snippet

        Assert.Equal("Server not found", message);
    }
}
