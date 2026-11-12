using System.Net;
using FZApi.Models;
using FZApi.Tests.Support;

namespace FZApi.Tests;

[Trait("Category", "Live")]
public class LiveApiTests
{
    private readonly LiveApi _api = new();

    public static TheoryData<string> ServerSlugs => new("fangbreaker", "frostreaver");

    [Fact]
    public async Task Health()
    {
        var health = await _api.Call(c => c.GetHealthAsync(TestContext.Current.CancellationToken), "$");

        Assert.True(health.Ok);
        Assert.Equal("healthy", health.Status);
    }

    [Fact]
    public async Task Servers()
    {
        var servers = await _api.Call(c => c.GetServersAsync(TestContext.Current.CancellationToken), "data.servers");

        Assert.Contains(servers, s => s.Slug == "fangbreaker");
        Assert.Contains(servers, s => s.Slug == "frostreaver");
    }

    [Theory]
    [MemberData(nameof(ServerSlugs))]
    public async Task Bonuses(string server)
    {
        var ct = TestContext.Current.CancellationToken;

        var types = await _api.Call(c => c.GetBonusTypesAsync(server, ct), "data");
        Assert.Contains(types, t => t.Key == "none");
        Assert.DoesNotContain(types, t => t.Key == "unconfirmed");

        var zones = await _api.Call(c => c.GetZonesAsync(server, ct), "data");
        Assert.NotEmpty(zones);

        var today = await _api.Call(c => c.GetTodaysBonusesAsync(server, ct), "data");
        Assert.NotEmpty(today);
        var zoneIds = zones.Select(z => z.Id).ToHashSet();
        Assert.All(today, bonus => Assert.Contains(bonus.ZoneId, zoneIds));
        var todayDate = Assert.Single(today.Select(b => b.BonusDate).Distinct());
        Assert.InRange(todayDate, BonusDay.Today.AddDays(-1), BonusDay.Today.AddDays(1));

        var yesterday = BonusDay.Today.AddDays(-1);
        var onDate = await _api.Call(c => c.GetBonusesOnDateAsync(server, yesterday, ct), "data");
        Assert.All(onDate, bonus => Assert.Equal(yesterday, bonus.BonusDate));

        var actions = await _api.Call(c => c.GetBonusActionsOnDateAsync(server, yesterday, ct), "data");
        Assert.All(actions, action => Assert.Equal(yesterday, action.BonusDay));

        var zone = zones[0];
        var history = await _api.Call(c => c.GetZoneBonusHistoryAsync(server, zone.Slug, ct), "data");
        Assert.Equal(zone.Id, history.Zone.Id);

        var expansion = zone.Expansion;
        var trends = await _api.Call(
            c => c.GetZoneBonusTrendsAsync(server, [expansion], yesterday.AddDays(-30), yesterday, ct), "data");
        Assert.NotEmpty(trends.Zones);
        Assert.All(trends.Zones, z => Assert.Equal(expansion, z.Expansion));
    }

    [Fact]
    public async Task UnknownServerIsNotFound()
    {
        var exception = await Assert.ThrowsAsync<FZApiException>(
            () => _api.Client.GetZonesAsync("no-such-server", TestContext.Current.CancellationToken));

        Assert.Equal(HttpStatusCode.NotFound, exception.StatusCode);
        Assert.Equal("Server not found", exception.Error);
    }

    [Fact]
    public async Task InvalidApiKeyIsRejected()
    {
        var client = new FZApiClient(new HttpClient(), new FZApiClientOptions { ApiKey = "fz_live_not_a_real_key" });

        var exception = await Assert.ThrowsAsync<FZApiException>(() => client.ReportBonusAsync(
            new BonusReport { ServerName = "Fangbreaker", ZoneName = "Befallen", BonusKey = "none" },
            TestContext.Current.CancellationToken));

        Assert.Equal(HttpStatusCode.Unauthorized, exception.StatusCode);
    }

    [Fact]
    public async Task NobodyIsSignedIn()
    {
        var user = await _api.Call(c => c.GetCurrentUserAsync(TestContext.Current.CancellationToken), "data.user");

        Assert.Null(user);
    }

    [Theory]
    [MemberData(nameof(ServerSlugs))]
    public async Task ProgressionAndRankings(string server)
    {
        var ct = TestContext.Current.CancellationToken;

        var progression = await _api.Call(c => c.GetServerProgressionAsync(server, cancellationToken: ct), "data");
        Assert.Equal(server, progression.Server.Slug);
        Assert.NotEmpty(progression.Progression.Expansions);

        var partition = progression.RaidContent.ActiveRankingPartition;
        Assert.SkipWhen(partition is null, $"{server} has no active ranking partition.");

        var query = new AllStarQuery
        {
            ServerId = progression.Server.Id,
            RankingPartitionId = partition.RankingPartitionId,
            Limit = 5,
        };
        var allStars = await _api.Call(c => c.GetAllStarRankingsAsync(query, ct), "data");
        Assert.Equal(partition.RankingPartitionId, allStars.Filters.RankingPartitionId);

        var included = partition.IncludedExpansions.Select(e => e.Id).ToHashSet();
        var boss = progression.Progression.Expansions
            .Where(e => included.Contains(e.Id))
            .SelectMany(e => e.Tiers)
            .SelectMany(t => t.Objectives)
            .SelectMany(o => o.RequiredBosses)
            .Select(b => b.RaidBoss)
            .FirstOrDefault();
        Assert.SkipWhen(boss is null, $"{server}'s active ranking partition has no bosses.");

        var bossStars = await _api.Call(c => c.GetAllStarBossRankingsAsync(boss.Id, query, ct), "data");
        Assert.Equal(partition.RankingPartitionId, bossStars.Filters.RankingPartitionId);

        await _api.Call(
            c => c.GetBossRankingsAsync(
                boss.Id,
                new BossRankingQuery
                {
                    ServerId = progression.Server.Id,
                    RankingPartitionId = partition.RankingPartitionId,
                    Limit = 5,
                },
                ct),
            "data.rankings");
    }

    [Fact]
    public async Task GuildsReportsEncountersAndCharacters()
    {
        var ct = TestContext.Current.CancellationToken;

        var guilds = await _api.Call(c => c.ListGuildsAsync(serverSlug: "fangbreaker", cancellationToken: ct), "data.guilds");
        Assert.NotEmpty(guilds);
        Assert.All(guilds, g => Assert.Equal("fangbreaker", g.Server.Slug));

        var guild = guilds.MaxBy(g => g.Counts.Encounters)!;
        var page = await _api.Call(c => c.GetGuildAsync(guild.Id, ct), "data");
        Assert.Equal(guild.Id, page.Guild.Id);

        var roster = await _api.Call(c => c.GetGuildRosterAsync(guild.Id, days: 30, limit: 10, cancellationToken: ct), "data");
        Assert.Equal(30, roster.Filters.Days);

        var guildProgression = await _api.Call(c => c.GetGuildProgressionAsync(guild.Id, ct), "data.progression");
        Assert.Equal(guild.Id, guildProgression.Guild.Id);

        var reports = await _api.Call(
            c => c.ListGuildReportsAsync(guild.Id, new ReportQuery { Limit = 3 }, ct), "data.reports");
        Assert.SkipWhen(reports.Count == 0, $"{guild.Name} has no reports.");

        var report = reports.MinBy(r => r.EncounterCount)!;
        var reportWithEncounters = await _api.Call(c => c.GetReportAsync(report.Id, ct), "data");
        Assert.Equal(report.Id, reportWithEncounters.Report.Id);

        var encounters = await _api.Call(c => c.GetReportEncountersAsync(report.Id, ct), "data.encounters");
        Assert.Equal(reportWithEncounters.Encounters.Count, encounters.Count);

        await _api.Call(c => c.GetReportParticipantsAsync(report.Id, ct), "data.participants");

        Assert.SkipWhen(encounters.Count == 0, $"Report {report.Id} has no encounters.");
        ParticipantCharacter? character = null;
        foreach (var summary in encounters.OrderByDescending(e => e.PlayerCount).Take(5))
        {
            var encounter = await _api.Call(c => c.GetEncounterAsync(summary.Id, ct), "data.encounter");
            Assert.Equal(summary.Id, encounter.Id);
            character = encounter.Participants.Select(p => p.Character).FirstOrDefault(c => c is not null);
            if (character is not null)
            {
                break;
            }
        }

        Assert.SkipWhen(character is null, $"No encounter in report {report.Id} has a matched character.");

        var characterPage = await _api.Call(c => c.GetCharacterAsync(character.Id, ParseMetric.Healing, ct), "data");
        Assert.Equal(character.Id, characterPage.Character.Id);
        Assert.Equal("HEALING", characterPage.Metric);

        var progression = await _api.Client.GetServerProgressionAsync("fangbreaker", cancellationToken: ct);
        var boss = progression.Progression.Expansions
            .SelectMany(e => e.Tiers)
            .SelectMany(t => t.Objectives)
            .SelectMany(o => o.RequiredBosses)
            .Select(b => b.RaidBoss)
            .First();
        var parses = await _api.Call(c => c.GetCharacterBossParsesAsync(character.Id, boss.Id, cancellationToken: ct), "data");
        Assert.Equal(boss.Id, parses.RaidBoss.Id);
    }
}
