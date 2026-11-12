using System.Runtime.CompilerServices;

namespace FZApi.Tests;

public static class ModuleInitializer
{
    [ModuleInitializer]
    public static void Initialize()
    {
        // The snapshots come from the spec's fixed examples, so the real values are more useful than placeholders.
        VerifierSettings.DontScrubGuids();
        VerifierSettings.DontScrubDateTimes();
    }
}
