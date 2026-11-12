namespace FZApi.Models;

public record GuildCounts
{
    public int Uploads { get; init; }
    public int Encounters { get; init; }
    public int Members { get; init; }

    public int UploadCodes { get; init; }

    public int InviteCodes { get; init; }

    public int BossKills { get; init; }
    public int ObjectiveCompletions { get; init; }
}
