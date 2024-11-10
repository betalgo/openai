using Betalgo.Ranul.OpenAI.Builders;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Betalgo.Ranul.OpenAI.Extensions;

public static class OpenAIRealtimeServiceCollectionExtensions
{
    public static OpenAIRealtimeServiceBuilder AddOpenAIRealtimeService(this IServiceCollection services)
    {
        var optionsBuilder = services.AddOptions<OpenAIOptions>();
        optionsBuilder.BindConfiguration(OpenAIOptions.SettingKey);

        var builder = new OpenAIRealtimeServiceBuilder(services);
        builder.Build();
        return builder;
    }

    public static OpenAIRealtimeServiceBuilder AddOpenAIRealtimeService(this IServiceCollection services, Action<OpenAIOptions> configureOptions)
    {
        services.Configure(configureOptions);
        var builder = new OpenAIRealtimeServiceBuilder(services);
        builder.Build();
        return builder;
    }

    public static OpenAIRealtimeServiceBuilder AddOpenAIRealtimeService(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<OpenAIOptions>(configuration.GetSection(OpenAIOptions.SettingKey));

        var builder = new OpenAIRealtimeServiceBuilder(services);
        builder.Build();
        return builder;
    }
}