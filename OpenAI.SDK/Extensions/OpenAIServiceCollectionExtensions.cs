using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OpenAI.GPT3.Interfaces;
using OpenAI.GPT3.Managers;

namespace OpenAI.GPT3.Extensions;

public static class OpenAIServiceCollectionExtensions
{
    public static IHttpClientBuilder AddOpenAIService(this IServiceCollection services, Action<OpenAiOptions>? setupAction = null)
    {
        if (setupAction == null)
        {
            services.AddOptions<OpenAiOptions>();
        }
        else
        {
            services.AddOptions<OpenAiOptions>().Configure(setupAction);
        }

        var configuration = services.BuildServiceProvider().GetRequiredService<IConfiguration>();
        services.Configure<OpenAiOptions>(configuration.GetSection(OpenAiOptions.SettingKey));

        return services.AddHttpClient<IOpenAIService, OpenAIService>();
    }
}