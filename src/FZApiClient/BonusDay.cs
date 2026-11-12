namespace FZApi;

public static class BonusDay
{
    private static readonly TimeZoneInfo Pacific = TimeZoneInfo.FindSystemTimeZoneById("America/Los_Angeles");

    public static DateOnly Today => At(DateTimeOffset.UtcNow);

    public static DateOnly At(DateTimeOffset instant)
        => DateOnly.FromDateTime(TimeZoneInfo.ConvertTime(instant, Pacific).DateTime);
}
