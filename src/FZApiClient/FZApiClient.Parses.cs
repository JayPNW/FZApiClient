using System.Text.Json;
using FZApi.Json;
using FZApi.Models;

namespace FZApi;

public partial class FZApiClient
{
    public async Task<EncounterDetail> GetEncounterAsync(Guid id, CancellationToken cancellationToken = default)
        => (await GetDataAsync<EncounterData>($"/api/encounters/{Segment(id)}", cancellationToken).ConfigureAwait(false))
        .Encounter;

    public Task<ReportWithEncounters> GetReportAsync(Guid id, CancellationToken cancellationToken = default)
        => GetDataAsync<ReportWithEncounters>($"/api/reports/{Segment(id)}", cancellationToken);

    public async Task<IReadOnlyList<EncounterSummary>> GetReportEncountersAsync(
        Guid id, CancellationToken cancellationToken = default)
        => (await GetDataAsync<EncountersData>($"/api/reports/{Segment(id)}/encounters", cancellationToken)
            .ConfigureAwait(false)).Encounters;

    public async Task<IReadOnlyList<JsonElement>> GetReportParticipantsAsync(
        Guid id, CancellationToken cancellationToken = default)
        => (await GetDataAsync<ParticipantsData>($"/api/reports/{Segment(id)}/participants", cancellationToken)
            .ConfigureAwait(false)).Participants;

    public Task RenameReportAsync(Guid id, string name, CancellationToken cancellationToken = default)
        => SendAsync(HttpMethod.Patch, $"/api/reports/{Segment(id)}", Json(new { name }), Auth.Session, cancellationToken);

    public Task DeleteReportAsync(Guid id, string? reason = null, CancellationToken cancellationToken = default)
        => SendAsync(
            HttpMethod.Delete, $"/api/reports/{Segment(id)}", Json(new { reason }), Auth.Session, cancellationToken);

    public Task FlagReportAsync(Guid id, string? reason = null, CancellationToken cancellationToken = default)
        => SendAsync(
            HttpMethod.Post, $"/api/reports/{Segment(id)}/report", Json(new { reason }), Auth.Session, cancellationToken);

    public async Task<IReadOnlyList<Report>> ListMyReportsAsync(
        ReportQuery? query = null, CancellationToken cancellationToken = default)
        => (await GetDataAsync<ReportsData>("/api/me/reports" + ReportQueryString(query), cancellationToken, Auth.Session)
            .ConfigureAwait(false)).Reports;

    public Task<AllStarStandings> GetAllStarRankingsAsync(
        AllStarQuery query, CancellationToken cancellationToken = default)
        => GetDataAsync<AllStarStandings>("/api/rankings/all-stars" + AllStarQueryString(query), cancellationToken);

    public Task<AllStarBossStandings> GetAllStarBossRankingsAsync(
        Guid boss, AllStarQuery query, CancellationToken cancellationToken = default)
        => GetDataAsync<AllStarBossStandings>(
            $"/api/rankings/all-stars/boss/{Segment(boss)}" + AllStarQueryString(query), cancellationToken);

    public async Task<IReadOnlyList<JsonElement>> GetBossRankingsAsync(
        Guid boss, BossRankingQuery query, CancellationToken cancellationToken = default)
        => (await GetDataAsync<RankingsData>(
            $"/api/rankings/boss/{Segment(boss)}" + new QueryString()
                .Add("serverId", query.ServerId)
                .Add("rankingPartitionId", query.RankingPartitionId)
                .Add("characterClass", query.CharacterClass)
                .Add("officialOnly", query.OfficialOnly)
                .Add("guildId", query.GuildId)
                .Add("limit", query.Limit)
                .Add("metric", query.Metric),
            cancellationToken).ConfigureAwait(false)).Rankings ?? [];

    public Task<CharacterPage> GetCharacterAsync(
        Guid id, ParseMetric? metric = null, CancellationToken cancellationToken = default)
        => GetDataAsync<CharacterPage>(
            $"/api/characters/{Segment(id)}" + new QueryString().Add("metric", metric), cancellationToken);

    public Task<CharacterBossParses> GetCharacterBossParsesAsync(
        Guid id, Guid bossId, ParseMetric? metric = null, CancellationToken cancellationToken = default)
        => GetDataAsync<CharacterBossParses>(
            $"/api/characters/{Segment(id)}/bosses/{Segment(bossId)}/parses" + new QueryString().Add("metric", metric),
            cancellationToken);

    public Task SetCharacterClassAsync(Guid id, string className, CancellationToken cancellationToken = default)
        => SendAsync(
            HttpMethod.Patch,
            $"/api/characters/{Segment(id)}/class",
            Json(new { className }),
            Auth.Session,
            cancellationToken);

    public Task<ServerProgression> GetServerProgressionAsync(
        string server, Guid? guildId = null, CancellationToken cancellationToken = default)
        => GetDataAsync<ServerProgression>(
            $"/api/servers/{Segment(server)}/progression" + new QueryString().Add("guildId", guildId),
            cancellationToken);

    public async Task<GuildProgression> GetGuildProgressionAsync(
        Guid guild, CancellationToken cancellationToken = default)
        => (await GetDataAsync<ProgressionData>($"/api/guilds/{Segment(guild)}/progression", cancellationToken)
            .ConfigureAwait(false)).Progression;

    private static QueryString ReportQueryString(ReportQuery? query)
        => new QueryString()
            .Add("limit", query?.Limit)
            .Add("serverSlug", query?.ServerSlug)
            .Add("serverId", query?.ServerId)
            .Add("guildId", query?.GuildId);

    private static QueryString AllStarQueryString(AllStarQuery query)
        => new QueryString()
            .Add("serverId", query.ServerId)
            .Add("rankingPartitionId", query.RankingPartitionId)
            .Add("expansionId", query.ExpansionId)
            .Add("metric", query.Metric)
            .Add("snapshotDate", query.SnapshotDate)
            .Add("limit", query.Limit)
            .Add("page", query.Page)
            .Add("className", query.ClassName);

    private record EncounterData(EncounterDetail Encounter);

    private record EncountersData(IReadOnlyList<EncounterSummary> Encounters);

    private record ParticipantsData(IReadOnlyList<JsonElement> Participants);

    private record ReportsData(IReadOnlyList<Report> Reports);

    private record RankingsData(IReadOnlyList<JsonElement>? Rankings);

    private record ProgressionData(GuildProgression Progression);
}
