using System.Text.Json;
using FZApi.Json;
using FZApi.Models;

namespace FZApi;

public partial class FZApiClient
{
    public async Task<Health> GetHealthAsync(CancellationToken cancellationToken = default)
    {
        var body = await SendAsync(HttpMethod.Get, "/api/health", null, Auth.None, cancellationToken)
            .ConfigureAwait(false);
        return JsonSerializer.Deserialize<Health>(body, FZApiJson.Options) ?? new Health();
    }

    public async Task<IReadOnlyList<Server>> GetServersAsync(CancellationToken cancellationToken = default)
        => (await GetDataAsync<ServersData>("/api/servers", cancellationToken).ConfigureAwait(false)).Servers;

    public Task<IReadOnlyList<BonusType>> GetBonusTypesAsync(
        string server, CancellationToken cancellationToken = default)
        => GetDataAsync<IReadOnlyList<BonusType>>($"/api/servers/{Segment(server)}/bonuses/types", cancellationToken);

    public Task<IReadOnlyList<Zone>> GetZonesAsync(string server, CancellationToken cancellationToken = default)
        => GetDataAsync<IReadOnlyList<Zone>>($"/api/servers/{Segment(server)}/bonuses/zones", cancellationToken);

    /// <summary>A zone may appear more than once.</summary>
    public Task<IReadOnlyList<ZoneBonusToday>> GetTodaysBonusesAsync(
        string server, CancellationToken cancellationToken = default)
        => GetDataAsync<IReadOnlyList<ZoneBonusToday>>($"/api/servers/{Segment(server)}/bonuses/today", cancellationToken);

    public Task<IReadOnlyList<ZoneBonusToday>> GetBonusesOnDateAsync(
        string server, DateOnly date, CancellationToken cancellationToken = default)
        => GetDataAsync<IReadOnlyList<ZoneBonusToday>>(
            $"/api/servers/{Segment(server)}/bonuses/history/date" + new QueryString().Add("date", date),
            cancellationToken);

    /// <summary>Oldest first.</summary>
    public Task<IReadOnlyList<BonusAction>> GetBonusActionsOnDateAsync(
        string server, DateOnly date, CancellationToken cancellationToken = default)
        => GetDataAsync<IReadOnlyList<BonusAction>>(
            $"/api/servers/{Segment(server)}/bonuses/history/logs" + new QueryString().Add("date", date),
            cancellationToken);

    public Task<ZoneBonusHistory> GetZoneBonusHistoryAsync(
        string server, string zoneSlug, CancellationToken cancellationToken = default)
        => GetDataAsync<ZoneBonusHistory>(
            $"/api/servers/{Segment(server)}/bonuses/zones/{Segment(zoneSlug)}/history", cancellationToken);

    /// <param name="expansions">By display name, such as <c>Ruins of Kunark</c>.</param>
    public Task<ZoneBonusTrends> GetZoneBonusTrendsAsync(
        string server,
        IEnumerable<string>? expansions = null,
        DateOnly? startDate = null,
        DateOnly? endDate = null,
        CancellationToken cancellationToken = default)
        => GetDataAsync<ZoneBonusTrends>(
            $"/api/servers/{Segment(server)}/bonuses/zones/trends" + new QueryString()
                .AddEach("expansion", expansions)
                .Add("startDate", startDate)
                .Add("endDate", endDate),
            cancellationToken);

    public Task AddBonusAsync(
        string server, Guid zoneId, Guid bonusTypeId, CancellationToken cancellationToken = default)
        => SendAsync(
            HttpMethod.Post,
            $"/api/servers/{Segment(server)}/bonuses",
            Json(new { zoneId, bonusTypeId }),
            Auth.Session,
            cancellationToken);

    public Task RemoveBonusAsync(string server, Guid zoneBonusId, CancellationToken cancellationToken = default)
        => SendAsync(
            HttpMethod.Post,
            $"/api/servers/{Segment(server)}/bonuses/{Segment(zoneBonusId)}/remove",
            null,
            Auth.Session,
            cancellationToken);

    public Task ReportBonusActionAsync(
        string server, Guid actionId, string? reason = null, CancellationToken cancellationToken = default)
        => SendAsync(
            HttpMethod.Post,
            $"/api/servers/{Segment(server)}/bonuses/actions/{Segment(actionId)}/report",
            Json(new { reason }),
            Auth.Session,
            cancellationToken);

    /// <summary>Sending the same report again is harmless.</summary>
    public Task<BonusReportResult> ReportBonusAsync(BonusReport report, CancellationToken cancellationToken = default)
        => SendForDataAsync<BonusReportResult>(
            HttpMethod.Post, "/api/client/bonuses", Json(report), Auth.ApiKey, cancellationToken);

    /// <param name="guildCode">
    /// A guild's upload code, to credit the encounter to that guild's official logs. Null for personal uploads.
    /// </param>
    public Task UploadEncounterAsync(
        EncounterSummaryUpload encounter, string? guildCode = null, CancellationToken cancellationToken = default)
        => SendAsync(
            HttpMethod.Post,
            "/api/client/encounters/finalized",
            Json(encounter, FZApiJson.SnakeCase),
            Auth.ApiKey,
            cancellationToken,
            request =>
            {
                if (guildCode is not null)
                {
                    request.Headers.Add("X-Guild-Code", guildCode);
                }
            });

    private record ServersData(IReadOnlyList<Server> Servers);
}
