using Betalgo.Ranul.OpenAI.Builders;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Betalgo.Ranul.OpenAI.Extensions;

/// <summary>
/// Contains extension methods for configuring OpenAI Realtime services in an ASP.NET Core application.
/// </summary>
/// <remarks>
/// The OpenAI Realtime service enables real-time communication with a GPT-4 class model over WebSocket,
/// supporting both audio and text transcriptions.
/// </remarks>
public static class OpenAIRealtimeServiceCollectionExtensions
{
    /// <summary>
    /// Adds OpenAI Realtime services to the specified <see cref="IServiceCollection"/> using configuration-based setup.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> to add services to.</param>
    /// <returns>An <see cref="OpenAIRealtimeServiceBuilder"/> that can be used to further configure the OpenAI Realtime services.</returns>
    /// <remarks>
    /// This method configures the OpenAI Realtime service using the application's configuration system,
    /// looking for settings under the OpenAIOptions.SettingKey section.
    /// </remarks>
    public static OpenAIRealtimeServiceBuilder AddOpenAIRealtimeService(this IServiceCollection services)
    {
        var optionsBuilder = services.AddOptions<OpenAIOptions>();
        optionsBuilder.BindConfiguration(OpenAIOptions.SettingKey);
        var builder = new OpenAIRealtimeServiceBuilder(services);
        builder.Build();
        return builder;
    }

    /// <summary>
    /// Adds OpenAI Realtime services to the specified <see cref="IServiceCollection"/> using a configuration action.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> to add services to.</param>
    /// <param name="configureOptions">A delegate to configure the <see cref="OpenAIOptions"/>.</param>
    /// <returns>An <see cref="OpenAIRealtimeServiceBuilder"/> that can be used to further configure the OpenAI Realtime services.</returns>
    /// <remarks>
    /// This method allows direct configuration of OpenAI Realtime options through a delegate,
    /// providing programmatic control over service settings such as modalities, voice settings,
    /// and audio format configurations.
    /// </remarks>
    public static OpenAIRealtimeServiceBuilder AddOpenAIRealtimeService(this IServiceCollection services, Action<OpenAIOptions> configureOptions)
    {
        services.Configure(configureOptions);
        var builder = new OpenAIRealtimeServiceBuilder(services);
        builder.Build();
        return builder;
    }

    /// <summary>
    /// Adds OpenAI Realtime services to the specified <see cref="IServiceCollection"/> using an <see cref="IConfiguration"/> section.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> to add services to.</param>
    /// <param name="configuration">The configuration section containing OpenAI Realtime settings.</param>
    /// <returns>An <see cref="OpenAIRealtimeServiceBuilder"/> that can be used to further configure the OpenAI Realtime services.</returns>
    /// <remarks>
    /// This method configures the OpenAI Realtime service using a specific configuration section,
    /// allowing for flexible configuration management through different configuration providers.
    /// The configuration can include settings for:
    /// - Audio input/output formats (pcm16, g711_ulaw, g711_alaw)
    /// - Voice selection (alloy, echo, shimmer)
    /// - Modalities (text, audio)
    /// - Turn detection settings
    /// - Audio transcription options
    /// </remarks>
    public static OpenAIRealtimeServiceBuilder AddOpenAIRealtimeService(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<OpenAIOptions>(configuration.GetSection(OpenAIOptions.SettingKey));
        var builder = new OpenAIRealtimeServiceBuilder(services);
        builder.Build();
        return builder;
    }
}