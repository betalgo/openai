using Betalgo.Ranul.OpenAI;
using Betalgo.Ranul.OpenAI.Extensions;
using Betalgo.Ranul.OpenAI.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace OpenAI.IntegrationTests.Fixtures;

/// <summary>
/// Shared test fixture that provides a configured OpenAI service for integration tests.
/// This fixture is shared across all tests in a collection to avoid creating multiple HTTP clients.
/// </summary>
/// <remarks>
/// Live tests require OPENAI_RUN_LIVE_TESTS=1 in addition to credentials.
/// Configuration providers, from lowest to highest precedence: appsettings.json,
/// appsettings.Development.json, user secrets, environment variables.
/// The API key is selected from OPENAI_API_KEY, OpenAIServiceOptions:ApiKey,
/// then the legacy OpenAI:ApiKey setting, using the first nonempty value.
/// Prefer an environment variable or user secrets; never commit credentials.
/// </remarks>
public class OpenAITestFixture : IDisposable
{
    private readonly ServiceProvider _serviceProvider;

    public OpenAITestFixture() : this(LoadConfiguration())
    {
    }

    internal OpenAITestFixture(IConfiguration configuration, HttpMessageHandler? handler = null)
    {
        var apiKey = new[]
        {
            configuration["OPENAI_API_KEY"],
            configuration[$"{OpenAIOptions.SettingKey}:ApiKey"],
            configuration["OpenAI:ApiKey"]
        }.FirstOrDefault(value => !string.IsNullOrWhiteSpace(value));
        if (apiKey == null)
        {
            throw new InvalidOperationException(
                "OpenAI API key is not configured. Set OPENAI_API_KEY or use " +
                "dotnet user-secrets set OpenAIServiceOptions:ApiKey <key> --project OpenAI.IntegrationTests.");
        }

        DefaultModel = FirstNonempty(configuration["OPENAI_TEST_MODEL"]) ?? "gpt-6-luna";
        _reasoningModel = FirstNonempty(configuration["OPENAI_TEST_REASONING_MODEL"]);
        _webSearchModel = FirstNonempty(configuration["OPENAI_TEST_WEB_SEARCH_MODEL"]);
        _codeInterpreterModel = FirstNonempty(configuration["OPENAI_TEST_CODE_INTERPRETER_MODEL"]);
        var maxOutputTokens = configuration["OPENAI_TEST_MAX_OUTPUT_TOKENS"] ?? "256";
        if (!int.TryParse(maxOutputTokens, out var limit) || limit < 1 || limit > 4096)
        {
            throw new InvalidOperationException("OPENAI_TEST_MAX_OUTPUT_TOKENS must be between 1 and 4096.");
        }
        DefaultMaxOutputTokens = limit;

        var services = new ServiceCollection();
        services.AddSingleton(configuration);
        var builder = services.AddOpenAIService(options =>
        {
            // Validate and supply the same key rather than validating one section
            // while AddOpenAIService binds a different section.
            options.ApiKey = apiKey;
        }).ConfigureHttpClient(client => client.Timeout = TimeSpan.FromSeconds(60));
        if (handler != null)
        {
            builder.ConfigurePrimaryHttpMessageHandler(() => handler);
        }

        _serviceProvider = services.BuildServiceProvider();
        OpenAI = _serviceProvider.GetRequiredService<IOpenAIService>();
    }

    private static string? FirstNonempty(params string?[] values) =>
        values.FirstOrDefault(value => !string.IsNullOrWhiteSpace(value));

    private static IConfiguration LoadConfiguration()
    {
        if (!LiveFactAttribute.IsEnabled)
        {
            throw new InvalidOperationException("Live integration tests require OPENAI_RUN_LIVE_TESTS=1.");
        }

        return new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile("appsettings.json", optional: true)
            .AddJsonFile("appsettings.Development.json", optional: true)
            .AddUserSecrets<OpenAITestFixture>(optional: true)
            .AddEnvironmentVariables()
            .Build();
    }

    /// <summary>
    /// The configured OpenAI service instance.
    /// </summary>
    public IOpenAIService OpenAI { get; }

    /// <summary>
    /// The reviewed core model; optional overrides require review before a paid run.
    /// </summary>
    public string DefaultModel { get; }

    /// <summary>
    /// The reasoning model to use for reasoning tests.
    /// </summary>
    private readonly string? _reasoningModel;
    private readonly string? _webSearchModel;
    private readonly string? _codeInterpreterModel;
    public string ReasoningModel => RequireCapabilityModel(_reasoningModel, "OPENAI_TEST_REASONING_MODEL");
    public string WebSearchModel => RequireCapabilityModel(_webSearchModel, "OPENAI_TEST_WEB_SEARCH_MODEL");
    public string CodeInterpreterModel => RequireCapabilityModel(_codeInterpreterModel, "OPENAI_TEST_CODE_INTERPRETER_MODEL");

    private static string RequireCapabilityModel(string? value, string setting) => value ??
        throw new InvalidOperationException($"Set {setting} to an explicitly reviewed capability model ID.");

    /// <summary>
    /// Output cap used by streaming tests that previously had no explicit token limit.
    /// Tests with specific token requirements keep their own limits.
    /// </summary>
    public int DefaultMaxOutputTokens { get; }

    public void Dispose()
    {
        _serviceProvider.Dispose();
        GC.SuppressFinalize(this);
    }
}

/// <summary>
/// Collection definition for sharing the OpenAI fixture across test classes.
/// </summary>
[CollectionDefinition("OpenAI")]
public class OpenAICollection : ICollectionFixture<OpenAITestFixture>
{
    // This class has no code, it's just used to define the collection.
}

