using System.Text.Json;
using FZApi.Json;
using FZApi.Models;

namespace FZApi;

public partial class FZApiClient
{
    /// <summary>Not paged.</summary>
    public async Task<IReadOnlyList<Guild>> ListGuildsAsync(
        int? serverId = null, string? serverSlug = null, CancellationToken cancellationToken = default)
        => (await GetDataAsync<GuildsData>(
            "/api/guilds" + new QueryString().Add("serverId", serverId).Add("serverSlug", serverSlug),
            cancellationToken).ConfigureAwait(false)).Guilds;

    public async Task<Guild> CreateGuildAsync(CreateGuildRequest guild, CancellationToken cancellationToken = default)
        => (await SendForDataAsync<GuildData>(HttpMethod.Post, "/api/guilds", Json(guild), Auth.Session, cancellationToken)
            .ConfigureAwait(false)).Guild;

    public Task<GuildPage> GetGuildAsync(Guid guild, CancellationToken cancellationToken = default)
        => GetDataAsync<GuildPage>($"/api/guilds/{Segment(guild)}", cancellationToken);

    /// <summary>Needs a guild officer role.</summary>
    public async Task<Guild> UpdateGuildAsync(
        Guid guild, UpdateGuildRequest details, CancellationToken cancellationToken = default)
        => (await SendForDataAsync<GuildData>(
            HttpMethod.Put, $"/api/guilds/{Segment(guild)}", Json(details), Auth.Session, cancellationToken)
            .ConfigureAwait(false)).Guild;

    /// <param name="days">Must be positive.</param>
    public Task<GuildRoster> GetGuildRosterAsync(
        Guid guild,
        int? days = null,
        int? limit = null,
        bool? includePets = null,
        CancellationToken cancellationToken = default)
        => GetDataAsync<GuildRoster>(
            $"/api/guilds/{Segment(guild)}/roster" + new QueryString()
                .Add("days", days)
                .Add("limit", limit)
                .Add("includePets", includePets),
            cancellationToken);

    public Task<JsonElement> GetGuildClassModerationAsync(Guid guild, CancellationToken cancellationToken = default)
        => GetDataAsync<JsonElement>($"/api/guilds/{Segment(guild)}/class-moderation", cancellationToken, Auth.Session);

    public async Task<IReadOnlyList<Report>> ListGuildReportsAsync(
        Guid guild, ReportQuery? query = null, CancellationToken cancellationToken = default)
        => (await GetDataAsync<ReportsData>(
            $"/api/guilds/{Segment(guild)}/reports" + ReportQueryString(query), cancellationToken)
            .ConfigureAwait(false)).Reports;

    public async Task<Guild> JoinGuildAsync(string code, CancellationToken cancellationToken = default)
        => (await SendForDataAsync<GuildData>(
            HttpMethod.Post, "/api/guilds/join", Json(new { code }), Auth.Session, cancellationToken)
            .ConfigureAwait(false)).Guild;

    public Task LeaveGuildAsync(Guid guild, CancellationToken cancellationToken = default)
        => SendAsync(HttpMethod.Post, $"/api/guilds/{Segment(guild)}/leave", null, Auth.Session, cancellationToken);

    /// <summary>Needs a guild officer role.</summary>
    public async Task<GuildMember> SetGuildMemberAsync(
        Guid guild, string userId, GuildRole role, CancellationToken cancellationToken = default)
        => (await SendForDataAsync<MemberData>(
            HttpMethod.Put,
            $"/api/guilds/{Segment(guild)}/members",
            Json(new { userId, role = role.ToString().ToUpperInvariant() }),
            Auth.Session,
            cancellationToken).ConfigureAwait(false)).Member;

    public Task RemoveGuildMemberAsync(Guid guild, Guid member, CancellationToken cancellationToken = default)
        => SendAsync(
            HttpMethod.Delete,
            $"/api/guilds/{Segment(guild)}/members/{Segment(member)}",
            null,
            Auth.Session,
            cancellationToken);

    public async Task<IReadOnlyList<UploadCode>> ListUploadCodesAsync(
        Guid guild, CancellationToken cancellationToken = default)
        => (await GetDataAsync<UploadCodesData>(
            $"/api/guilds/{Segment(guild)}/upload-codes", cancellationToken, Auth.Session).ConfigureAwait(false))
        .UploadCodes;

    /// <summary>The code itself is in <see cref="UploadCode.Code"/>, only this once.</summary>
    public async Task<UploadCode> CreateUploadCodeAsync(
        Guid guild, string? label = null, CancellationToken cancellationToken = default)
        => (await SendForDataAsync<UploadCodeData>(
            HttpMethod.Post, $"/api/guilds/{Segment(guild)}/upload-codes", Json(new { label }), Auth.Session,
            cancellationToken).ConfigureAwait(false)).UploadCode;

    public async Task<UploadCode> RevokeUploadCodeAsync(
        Guid guild, string codeId, CancellationToken cancellationToken = default)
        => (await SendForDataAsync<UploadCodeData>(
            HttpMethod.Delete, $"/api/guilds/{Segment(guild)}/upload-codes/{Segment(codeId)}", null, Auth.Session,
            cancellationToken).ConfigureAwait(false)).UploadCode;

    public async Task<IReadOnlyList<InviteCode>> ListInviteCodesAsync(
        Guid guild, CancellationToken cancellationToken = default)
        => (await GetDataAsync<InviteCodesData>(
            $"/api/guilds/{Segment(guild)}/invite-codes", cancellationToken, Auth.Session).ConfigureAwait(false))
        .InviteCodes;

    /// <summary>The code itself is in <see cref="InviteCode.Code"/>, only this once.</summary>
    public async Task<InviteCode> CreateInviteCodeAsync(
        Guid guild, string? label = null, CancellationToken cancellationToken = default)
        => (await SendForDataAsync<InviteCodeData>(
            HttpMethod.Post, $"/api/guilds/{Segment(guild)}/invite-codes", Json(new { label }), Auth.Session,
            cancellationToken).ConfigureAwait(false)).InviteCode;

    public async Task<InviteCode> RevokeInviteCodeAsync(
        Guid guild, string codeId, CancellationToken cancellationToken = default)
        => (await SendForDataAsync<InviteCodeData>(
            HttpMethod.Delete, $"/api/guilds/{Segment(guild)}/invite-codes/{Segment(codeId)}", null, Auth.Session,
            cancellationToken).ConfigureAwait(false)).InviteCode;

    public Uri GetDiscordLoginUri() => new(_baseAddress, "/api/auth/discord");

    public async Task<SessionUser?> GetCurrentUserAsync(CancellationToken cancellationToken = default)
    {
        var auth = string.IsNullOrEmpty(_options.SessionCookie) ? Auth.None : Auth.Session;
        return (await GetDataAsync<UserData>("/api/auth/me", cancellationToken, auth).ConfigureAwait(false)).User;
    }

    public Task LogoutAsync(CancellationToken cancellationToken = default)
        => SendAsync(HttpMethod.Post, "/api/auth/logout", null, Auth.Session, cancellationToken);

    public Task<ApiKeyStatus> GetApiKeyStatusAsync(CancellationToken cancellationToken = default)
        => GetDataAsync<ApiKeyStatus>("/api/me/api-key", cancellationToken, Auth.Session);

    /// <summary>This is the only time the full key is shown.</summary>
    public async Task<string> CreateApiKeyAsync(CancellationToken cancellationToken = default)
        => (await SendForDataAsync<ApiKeyData>(HttpMethod.Post, "/api/me/api-key", null, Auth.Session, cancellationToken)
            .ConfigureAwait(false)).ApiKey;

    public Task RevokeApiKeyAsync(CancellationToken cancellationToken = default)
        => SendAsync(HttpMethod.Delete, "/api/me/api-key", null, Auth.Session, cancellationToken);

    public async Task<IReadOnlyList<GuildMembership>> GetMyGuildsAsync(CancellationToken cancellationToken = default)
        => (await GetDataAsync<MembershipsData>("/api/me/guilds", cancellationToken, Auth.Session).ConfigureAwait(false))
        .Memberships;

    private record GuildsData(IReadOnlyList<Guild> Guilds);

    private record GuildData(Guild Guild);

    private record MemberData(GuildMember Member);

    private record UploadCodesData(IReadOnlyList<UploadCode> UploadCodes);

    private record UploadCodeData(UploadCode UploadCode);

    private record InviteCodesData(IReadOnlyList<InviteCode> InviteCodes);

    private record InviteCodeData(InviteCode InviteCode);

    private record UserData(SessionUser? User);

    private record ApiKeyData(string ApiKey);

    private record MembershipsData(IReadOnlyList<GuildMembership> Memberships);
}
