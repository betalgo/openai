namespace OpenAI.IntegrationTests.Fixtures;

/// <summary>
/// Paid API tests are skipped unless explicitly enabled, even when credentials exist.
/// </summary>
public sealed class LiveFactAttribute : FactAttribute
{
    internal static bool IsEnabled => IsOptedIn(Environment.GetEnvironmentVariable("OPENAI_RUN_LIVE_TESTS"));

    internal static bool IsOptedIn(string? value) => string.Equals(value, "1", StringComparison.Ordinal);

    public LiveFactAttribute(string? modelEnvironmentVariable = null)
    {
        if (!IsEnabled)
        {
            Skip = "Live API tests require OPENAI_RUN_LIVE_TESTS=1 and a configured API key.";
        }
        else if (modelEnvironmentVariable != null && string.IsNullOrWhiteSpace(Environment.GetEnvironmentVariable(modelEnvironmentVariable)))
        {
            Skip = $"This paid capability test additionally requires an explicitly reviewed {modelEnvironmentVariable} model ID.";
        }
    }
}
