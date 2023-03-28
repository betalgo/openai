using System.Net;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using OpenAI.GPT3.Interfaces;
using OpenAI.GPT3.Managers;

namespace OpenAI.GPT3.Extensions;

public static class OpenAIServiceCollectionExtensions
{
    public static IServiceCollection AddOpenAIService(this IServiceCollection services)
    {
        services.AddOptions<OpenAiOptions>();
        var configuration = services.BuildServiceProvider().GetRequiredService<IConfiguration>();
        services.Configure<OpenAiOptions>(configuration.GetSection(OpenAiOptions.SettingKey));
        services.AddHttpClient<IOpenAIService, OpenAIService>()
            .ConfigurePrimaryHttpMessageHandler(s=> new HttpClientHandler
            {
                Proxy = s.GetService<IOptions<OpenAiOptions>>()!.Value.Proxy is not null ? new WebProxy(s.GetService<IOptions<OpenAiOptions>>()!.Value.Proxy) : null,
            });
        return services;
    }

    public static IServiceCollection AddOpenAIService(this IServiceCollection services, Action<OpenAiOptions> setupAction)
    {
        services.AddOptions<OpenAiOptions>().Configure(setupAction);
        services.AddHttpClient<IOpenAIService, OpenAIService>()
            .ConfigurePrimaryHttpMessageHandler(s=> new HttpClientHandler
            {
                Proxy = s.GetService<IOptions<OpenAiOptions>>()!.Value.Proxy is not null ? new WebProxy(s.GetService<IOptions<OpenAiOptions>>()!.Value.Proxy) : null,
            });
        return services;
    }
}