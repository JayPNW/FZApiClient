// These tests call an in-memory stub that answers at once, so there's nothing for cancellation to cut short.
#pragma warning disable xUnit1051

using System.Net;
using FZApi.Models;
using FZApi.Tests.Support;

namespace FZApi.Tests;

public class ClientBehaviorTests
{
    private static (FZApiClient Client, StubHandler Handler) Create(
        string body, HttpStatusCode status = HttpStatusCode.OK, FZApiClientOptions? options = null, Uri? baseAddress = null)
    {
        var handler = new StubHandler(status, body);
        var http = new HttpClient(handler) { BaseAddress = baseAddress };
        return (new FZApiClient(http, options), handler);
    }

    [Fact]
    public async Task ErrorEnvelopeBecomesFzApiException()
    {
        var (client, _) = Create("""{"ok":false,"error":"Server not found"}""", HttpStatusCode.NotFound);

        var exception = await Assert.ThrowsAsync<FZApiException>(() => client.GetZonesAsync("nope"));

        Assert.Equal(HttpStatusCode.NotFound, exception.StatusCode);
        Assert.Equal("Server not found", exception.Error);
    }

    [Fact]
    public async Task NonJsonErrorKeepsTheBody()
    {
        var (client, _) = Create("<html>Bad gateway</html>", HttpStatusCode.BadGateway);

        var exception = await Assert.ThrowsAsync<FZApiException>(() => client.GetServersAsync());

        Assert.Equal(HttpStatusCode.BadGateway, exception.StatusCode);
        Assert.Equal("<html>Bad gateway</html>", exception.Error);
    }

    [Fact]
    public async Task OkFalseWithSuccessStatusStillThrows()
    {
        var (client, _) = Create("""{"ok":false,"error":"Something odd"}""");

        var exception = await Assert.ThrowsAsync<FZApiException>(() => client.GetServersAsync());

        Assert.Equal("Something odd", exception.Error);
    }

    [Fact]
    public async Task ApiKeyOperationWithoutKeyFailsBeforeSending()
    {
        var (client, handler) = Create("");

        await Assert.ThrowsAsync<InvalidOperationException>(() => client.ReportBonusAsync(
            new BonusReport { ServerName = "Fangbreaker", ZoneName = "Befallen", BonusKey = "none" }));

        Assert.Empty(handler.Requests);
    }

    [Fact]
    public async Task SessionOperationWithoutCookieFailsBeforeSending()
    {
        var (client, handler) = Create("");

        await Assert.ThrowsAsync<InvalidOperationException>(() => client.LogoutAsync());

        Assert.Empty(handler.Requests);
    }

    [Fact]
    public async Task PublicOperationsSendNoCredentials()
    {
        var (client, handler) = Create(
            """{"ok":true,"data":{"servers":[]}}""",
            options: new FZApiClientOptions { ApiKey = "fz_live_secret", SessionCookie = "secret" });

        await client.GetServersAsync();

        Assert.Null(handler.LastRequest.Header("X-API-Key"));
        Assert.Null(handler.LastRequest.Header("Cookie"));
    }

    [Fact]
    public async Task CurrentUserSendsTheSessionWhenThereIsOne()
    {
        var (client, handler) = Create(
            """{"ok":true,"data":{"user":{"id":"u1","username":"Example","role":"MEMBER"}}}""",
            options: new FZApiClientOptions { SessionCookie = "abc" });

        var user = await client.GetCurrentUserAsync();

        Assert.Equal("fz.sid=abc", handler.LastRequest.Header("Cookie"));
        Assert.Equal("Example", user?.Username);
    }

    [Fact]
    public async Task CurrentUserIsNullWithoutASession()
    {
        var (client, _) = Create("""{"ok":true,"data":{"user":null}}""");

        Assert.Null(await client.GetCurrentUserAsync());
    }

    [Fact]
    public async Task UsesFrostreaverByDefault()
    {
        var (client, handler) = Create("""{"ok":true,"status":"healthy"}""");

        await client.GetHealthAsync();

        Assert.Equal("https://frostreaver.zone/api/health", handler.LastRequest.Uri.ToString());
    }

    [Fact]
    public async Task UsesTheBaseAddressFromOptions()
    {
        var (client, handler) = Create(
            """{"ok":true,"status":"healthy"}""", options: new FZApiClientOptions { BaseAddress = FZApiClient.FangbreakerZone });

        await client.GetHealthAsync();

        Assert.Equal("https://fangbreaker.zone/api/health", handler.LastRequest.Uri.ToString());
    }

    [Fact]
    public async Task HttpClientBaseAddressWins()
    {
        var (client, handler) = Create(
            """{"ok":true,"status":"healthy"}""",
            options: new FZApiClientOptions { BaseAddress = FZApiClient.FangbreakerZone },
            baseAddress: new Uri("http://localhost:5000"));

        await client.GetHealthAsync();

        Assert.Equal("http://localhost:5000/api/health", handler.LastRequest.Uri.ToString());
    }

    [Fact]
    public async Task SendsAUserAgent()
    {
        var (client, handler) = Create("""{"ok":true,"status":"healthy"}""");

        await client.GetHealthAsync();

        Assert.Equal("FZApiClient", handler.LastRequest.Header("User-Agent"));
    }

    [Fact]
    public async Task EscapesPathSegments()
    {
        var (client, handler) = Create("""{"ok":true,"data":{"zone":{},"history":[],"stats":[]}}""");

        await client.GetZoneBonusHistoryAsync("fangbreaker", "a/b c");

        Assert.Equal("/api/servers/fangbreaker/bonuses/zones/a%2Fb%20c/history", handler.LastRequest.PathAndQuery);
    }

    [Fact]
    public async Task TrendsLeavesOutUnsetFilters()
    {
        var (client, handler) = Create("""{"ok":true,"data":{"bonusTypes":[],"zones":[]}}""");

        await client.GetZoneBonusTrendsAsync("fangbreaker");

        Assert.Equal("/api/servers/fangbreaker/bonuses/zones/trends", handler.LastRequest.PathAndQuery);
    }

    [Fact]
    public async Task EmptyZoneBonusIsNull()
    {
        var (client, _) = Create(
            """{"ok":true,"data":{"updated":false,"zoneBonus":{}}}""",
            options: new FZApiClientOptions { ApiKey = "fz_live_test" });

        var result = await client.ReportBonusAsync(
            new BonusReport { ServerName = "Fangbreaker", ZoneName = "Befallen", BonusKey = "none" });

        Assert.False(result.Updated);
        Assert.Null(result.ZoneBonus);
    }

    [Fact]
    public async Task BonusDayComesFromTheDatePartOfBonusDate()
    {
        var (client, _) = Create(
            """{"ok":true,"data":{"updated":true,"zoneBonus":{"bonusDate":"2026-11-12T00:00:00.000Z"}}}""",
            options: new FZApiClientOptions { ApiKey = "fz_live_test" });

        var result = await client.ReportBonusAsync(
            new BonusReport { ServerName = "Fangbreaker", ZoneName = "Befallen", BonusKey = "none" });

        Assert.Equal(new DateOnly(2026, 11, 12), result.ZoneBonus?.BonusDay);
    }

    [Fact]
    public async Task UploadWithoutGuildCodeSendsNoGuildHeader()
    {
        var (client, handler) = Create("", options: new FZApiClientOptions { ApiKey = "fz_live_test" });

        await client.UploadEncounterAsync(new EncounterSummaryUpload
        {
            EncounterId = "e1",
            TargetName = "a gnoll",
            Zone = "unknown",
            StartedAt = DateTimeOffset.UnixEpoch,
            EndedAt = DateTimeOffset.UnixEpoch,
            DurationSec = 0,
            Participants = [],
        });

        Assert.Null(handler.LastRequest.Header("X-Guild-Code"));
        Assert.Equal("fz_live_test", handler.LastRequest.Header("X-API-Key"));
    }

    [Theory]
    [InlineData("2026-01-15T07:59:00Z", "2026-01-14")] // Winter: the day starts at 08:00 UTC.
    [InlineData("2026-01-15T08:00:00Z", "2026-01-15")]
    [InlineData("2026-07-15T06:59:00Z", "2026-07-14")] // Summer: the day starts at 07:00 UTC.
    [InlineData("2026-07-15T07:00:00Z", "2026-07-15")]
    public void BonusDayFollowsPacificTime(string instant, string expected)
    {
        Assert.Equal(DateOnly.Parse(expected), BonusDay.At(DateTimeOffset.Parse(instant)));
    }
}
