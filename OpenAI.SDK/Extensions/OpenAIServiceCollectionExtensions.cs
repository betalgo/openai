using Microsoft.Extensions.DependencyInjection;
using OpenAI.Interfaces;
using OpenAI.Managers;

namespace OpenAI.Extensions;

public static class OpenAIServiceCollectionExtensions
{
    public static IHttpClientBuilder AddOpenAIService(this IServiceCollection services, Action<OpenAiOptions>? setupAction = null)
    {
        var optionsBuilder = services.AddOptions<OpenAiOptions>();
        if (setupAction != null)
        {
            optionsBuilder.Configure(setupAction);
        }
        else
        {
            optionsBuilder.BindConfiguration(OpenAiOptions.SettingKey);
        }

        return services.AddHttpClient<IOpenAIService, OpenAIService>();
    }

    public static IHttpClientBuilder AddOpenAIService<TServiceInterface>(this IServiceCollection services, string name, Action<OpenAiOptions>? setupAction = null)
        where TServiceInterface : class, IOpenAIService
    {
        var optionsBuilder = services.AddOptions<OpenAiOptions>(name);
        if (setupAction != null)
        {
            optionsBuilder.Configure(setupAction);
        }
        else
        {
            optionsBuilder.BindConfiguration($"{OpenAiOptions.SettingKey}:{name}");
        }

        return services.AddHttpClient<TServiceInterface>();
    }
}