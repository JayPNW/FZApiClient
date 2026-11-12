using FZApi.Models;

namespace FZApi.Tests.Support;

internal static class OperationCases
{
    public static readonly Guid Id1 = Guid.Parse("11111111-1111-4111-8111-111111111111");
    public static readonly Guid Id2 = Guid.Parse("22222222-2222-4222-8222-222222222222");

    private const string GuildJson = """
        {"id":"00000000-0000-4000-8000-000000000012","name":"Example Guild","slug":"example-guild",
         "description":null,"server":{"id":1,"name":"Fangbreaker","slug":"fangbreaker"},
         "counts":{"uploads":0,"encounters":0,"members":1,"uploadCodes":0,"inviteCodes":0,"bossKills":0,"objectiveCompletions":0},
         "createdAt":"2026-06-09T20:48:36.213Z","updatedAt":"2026-06-09T20:49:30.268Z"}
        """;

    private const string CodeJson = """
        {"id":"00000000-0000-4000-8000-000000000020","label":"Officer code","isActive":true,
         "createdAt":"2026-06-09T20:48:36.213Z","lastUsedAt":null,"code":"abc123"}
        """;

    private const string ReportJson = """
        {"id":"00000000-0000-4000-8000-000000000005","name":"Exampletwo","displayName":"Exampletwo","visibility":"PUBLIC",
         "startedAt":"2026-06-23T17:55:36.000Z","endedAt":"2026-06-23T18:59:04.000Z",
         "firstUploadAt":"2026-06-23T17:55:43.783Z","lastUploadAt":"2026-06-23T18:59:05.615Z",
         "uploadCount":1,"submissionCount":1,"encounterCount":1,
         "server":{"id":1,"name":"Fangbreaker","slug":"fangbreaker"},
         "guild":{"id":"00000000-0000-4000-8000-000000000006","name":"Second Example Guild","slug":"second-example-guild"},
         "createdBy":{"id":"00000000-0000-4000-8000-000000000010","username":"ExampleOfficer"},
         "createdAt":"2026-06-23T17:55:43.796Z","updatedAt":"2026-06-23T18:59:05.625Z","isInvalid":false,"isHidden":false,
         "moderatedAt":null,"moderatedById":null,"moderationReason":null,"deletedAt":null,"deletedById":null,
         "deleteReason":null,"openContentReportCount":0}
        """;

    private static readonly AllStarQuery AllStars = new()
    {
        ServerId = 1,
        RankingPartitionId = Id1,
        ExpansionId = Id2,
        Metric = ParseMetric.Healing,
        SnapshotDate = new DateOnly(2026, 11, 1),
        Limit = 10,
        Page = 2,
        ClassName = "Shadow Knight",
    };

    private static readonly EncounterSummaryUpload Upload = new()
    {
        EncounterId = "5f0c3a3e2f5c4d4fa1f4f8f7f3c7a9b1",
        TargetName = "a gnoll",
        Zone = "Blackburrow",
        CharacterName = "Examplechar",
        ServerName = "Frostreaver",
        CharacterClass = "Shadow Knight",
        ClientVersion = "1.2.6",
        StartedAt = new DateTimeOffset(2026, 11, 12, 3, 0, 0, TimeSpan.Zero),
        EndedAt = new DateTimeOffset(2026, 11, 12, 3, 0, 30, TimeSpan.Zero),
        DurationSec = 30,
        TotalDamage = 600,
        Dps = 20,
        Participants =
        [
            new() { Name = "Examplechar", Damage = 600, Dps = 20, CritPct = 12.5, IsPlayer = true },
            new() { Name = "Gobaner", Damage = 0, Dps = 0, IsPlayer = false, Owner = "Examplechar" },
        ],
        ClientProvenance = new()
        {
            SessionId = "session-1",
            Mode = "Live",
            StartedAtUtc = new DateTimeOffset(2026, 11, 12, 2, 0, 0, TimeSpan.Zero),
            LastObservedAtUtc = new DateTimeOffset(2026, 11, 12, 3, 0, 30, TimeSpan.Zero),
            Eligibility = "LiveEligible",
            IsUploadEligible = true,
        },
    };

    public static readonly IReadOnlyDictionary<string, OperationCase> All = new Dictionary<string, OperationCase>
    {
        // Status and servers
        ["getHealth"] = new(c => R(c.GetHealthAsync()), "$"),
        ["getServers"] = new(c => R(c.GetServersAsync()), "data.servers"),

        // Bonuses
        ["getBonusTypes"] = new(c => R(c.GetBonusTypesAsync("fangbreaker")), "data"),
        ["getZones"] = new(c => R(c.GetZonesAsync("fangbreaker")), "data"),
        ["getTodaysBonuses"] = new(c => R(c.GetTodaysBonusesAsync("Frostreaver")), "data"),
        ["getBonusesOnDate"] = new(c => R(c.GetBonusesOnDateAsync("fangbreaker", new DateOnly(2026, 11, 11))), "data"),
        ["getBonusActionsOnDate"] = new(
            c => R(c.GetBonusActionsOnDateAsync("fangbreaker", new DateOnly(2026, 11, 10))), "data"),
        ["getZoneBonusHistory"] = new(c => R(c.GetZoneBonusHistoryAsync("fangbreaker", "befallen")), "data"),
        ["getZoneBonusTrends"] = new(
            c => R(c.GetZoneBonusTrendsAsync(
                "fangbreaker", ["Classic", "Ruins of Kunark"], new DateOnly(2026, 6, 1), new DateOnly(2026, 6, 30))),
            "data"),

        // Bonus moderation
        ["addBonus"] = new(c => N(c.AddBonusAsync("fangbreaker", Id1, Id2))),
        ["removeBonus"] = new(c => N(c.RemoveBonusAsync("fangbreaker", Id1))),
        ["reportBonusAction"] = new(c => N(c.ReportBonusActionAsync("fangbreaker", Id1, "Wrong zone"))),

        // Client
        ["reportBonus"] = new(
            c => R(c.ReportBonusAsync(new BonusReport
            {
                ServerName = "Fangbreaker",
                ZoneName = "Bastion of Thunder",
                BonusKey = "experience",
            })),
            "data"),
        ["uploadEncounter"] = new(c => N(c.UploadEncounterAsync(Upload, "guild-code-1"))),

        // Reports
        ["getEncounter"] = new(c => R(c.GetEncounterAsync(Id1)), "data.encounter"),
        ["getReport"] = new(c => R(c.GetReportAsync(Id1)), "data"),
        ["getReportEncounters"] = new(c => R(c.GetReportEncountersAsync(Id1)), "data.encounters"),
        ["getReportParticipants"] = new(c => R(c.GetReportParticipantsAsync(Id1)), "data.participants"),
        ["renameReport"] = new(c => N(c.RenameReportAsync(Id1, "Tuesday raid"))),
        ["deleteReport"] = new(c => N(c.DeleteReportAsync(Id1, "Duplicate"))),
        ["flagReport"] = new(c => N(c.FlagReportAsync(Id1, "Edited log"))),
        ["listMyReports"] = new(
            c => R(c.ListMyReportsAsync(new ReportQuery { Limit = 5, ServerSlug = "fangbreaker", GuildId = Id2 })),
            "data.reports",
            Data("reports", "[" + ReportJson + "]")),

        // Rankings
        ["getAllStarRankings"] = new(c => R(c.GetAllStarRankingsAsync(AllStars)), "data"),
        ["getAllStarBossRankings"] = new(c => R(c.GetAllStarBossRankingsAsync(Id2, AllStars)), "data"),
        ["getBossRankings"] = new(
            c => R(c.GetBossRankingsAsync(Id1, new BossRankingQuery
            {
                ServerId = 2,
                RankingPartitionId = Id2,
                CharacterClass = "Cleric",
                OfficialOnly = true,
                GuildId = Id1,
                Limit = 25,
                Metric = ParseMetric.Damage,
            })),
            "data.rankings",
            """{"ok":true,"data":{"rankings":[{"averagePercentile":97.5}]}}"""),

        // Characters
        ["getCharacter"] = new(c => R(c.GetCharacterAsync(Id1, ParseMetric.Damage)), "data"),
        ["getCharacterBossParses"] = new(c => R(c.GetCharacterBossParsesAsync(Id1, Id2)), "data"),
        ["setCharacterClass"] = new(c => N(c.SetCharacterClassAsync(Id1, "Cleric"))),

        // Progression
        ["getServerProgression"] = new(c => R(c.GetServerProgressionAsync("fangbreaker", Id1)), "data"),
        ["getGuildProgression"] = new(c => R(c.GetGuildProgressionAsync(Id1)), "data.progression"),

        // Guilds
        ["listGuilds"] = new(c => R(c.ListGuildsAsync(serverSlug: "fangbreaker")), "data.guilds"),
        ["createGuild"] = new(
            c => R(c.CreateGuildAsync(new CreateGuildRequest { ServerId = 1, Name = "Example Guild" })),
            "data.guild",
            Data("guild", GuildJson)),
        ["getGuild"] = new(c => R(c.GetGuildAsync(Id1)), "data"),
        ["updateGuild"] = new(
            c => R(c.UpdateGuildAsync(
                Id1, new UpdateGuildRequest { Name = "Example Guild", Slug = "example-guild", Description = "Raids" })),
            "data.guild",
            Data("guild", GuildJson)),
        ["getGuildRoster"] = new(c => R(c.GetGuildRosterAsync(Id1, days: 30, limit: 100, includePets: false)), "data"),
        ["getGuildClassModeration"] = new(
            c => R(c.GetGuildClassModerationAsync(Id1)), "data", """{"ok":true,"data":{"characters":[]}}"""),
        ["listGuildReports"] = new(c => R(c.ListGuildReportsAsync(Id1, new ReportQuery { Limit = 20 })), "data.reports"),
        ["joinGuild"] = new(
            c => R(c.JoinGuildAsync("invite-1")), "data.guild", Data("guild", GuildJson)),
        ["leaveGuild"] = new(c => N(c.LeaveGuildAsync(Id1))),
        ["setGuildMember"] = new(
            c => R(c.SetGuildMemberAsync(Id1, "user-1", GuildRole.Officer)),
            "data.member",
            """
            {"ok":true,"data":{"member":{"id":"00000000-0000-4000-8000-000000000013","role":"OFFICER",
             "user":{"id":"00000000-0000-4000-8000-000000000010","username":"ExampleOfficer","role":"MEMBER"},
             "createdAt":"2026-06-07T06:32:55.078Z","updatedAt":"2026-06-07T06:32:55.078Z"}}}
            """),
        ["removeGuildMember"] = new(c => N(c.RemoveGuildMemberAsync(Id1, Id2))),
        ["listUploadCodes"] = new(
            c => R(c.ListUploadCodesAsync(Id1)), "data.uploadCodes", Data("uploadCodes", "[" + CodeJson + "]")),
        ["createUploadCode"] = new(
            c => R(c.CreateUploadCodeAsync(Id1, "Officer code")),
            "data.uploadCode",
            Data("uploadCode", CodeJson)),
        ["revokeUploadCode"] = new(
            c => R(c.RevokeUploadCodeAsync(Id1, "00000000-0000-4000-8000-000000000020")),
            "data.uploadCode",
            Data("uploadCode", CodeJson)),
        ["listInviteCodes"] = new(
            c => R(c.ListInviteCodesAsync(Id1)), "data.inviteCodes", Data("inviteCodes", "[" + CodeJson + "]")),
        ["createInviteCode"] = new(
            c => R(c.CreateInviteCodeAsync(Id1)), "data.inviteCode", Data("inviteCode", CodeJson)),
        ["revokeInviteCode"] = new(
            c => R(c.RevokeInviteCodeAsync(Id1, "00000000-0000-4000-8000-000000000020")),
            "data.inviteCode",
            Data("inviteCode", CodeJson)),

        // Auth
        ["startDiscordLogin"] = new(c => Task.FromResult<object?>(c.GetDiscordLoginUri())),
        ["getCurrentUser"] = new(c => R(c.GetCurrentUserAsync()), "data.user"),
        ["logout"] = new(c => N(c.LogoutAsync())),

        // Account
        ["getApiKeyStatus"] = new(
            c => R(c.GetApiKeyStatusAsync()), "data", """{"ok":true,"data":{"hasApiKey":true,"maskedApiKey":"fz_live_****abcd"}}"""),
        ["createApiKey"] = new(
            c => R(c.CreateApiKeyAsync()), "data.apiKey", """{"ok":true,"data":{"apiKey":"fz_live_0123456789abcdef"}}"""),
        ["revokeApiKey"] = new(c => N(c.RevokeApiKeyAsync())),
        ["getMyGuilds"] = new(
            c => R(c.GetMyGuildsAsync()),
            "data.memberships",
            Data("memberships", "[{\"id\":\"m-1\",\"role\":\"OWNER\",\"guild\":" + GuildJson + "}]")),
    };

    private static string Data(string name, string json) => "{\"ok\":true,\"data\":{\"" + name + "\":" + json + "}}";

    private static async Task<object?> R<T>(Task<T> task) => await task;

    private static async Task<object?> N(Task task)
    {
        await task;
        return null;
    }
}
