using Microsoft.Extensions.DependencyInjection;
using OpenAI.Interfaces;
using OpenAI.Managers;

namespace OpenAI.Extensions;

public static class OpenAIServiceCollectionExtensions
{
    public static IHttpClientBuilder AddOpenAIService(this IServiceCollection services, Action<OpenAIOptions>? setupAction = null)
    {
        var optionsBuilder = services.AddOptions<OpenAIOptions>();
        if (setupAction != null)
        {
            optionsBuilder.Configure(setupAction);
        }
        else
        {
            optionsBuilder.BindConfiguration(OpenAIOptions.SettingKey);
        }

        return services.AddHttpClient<IOpenAIService, OpenAIService>();
    }

    public static IHttpClientBuilder AddOpenAIService<TServiceInterface>(this IServiceCollection services, string name, Action<OpenAIOptions>? setupAction = null)
        where TServiceInterface : class, IOpenAIService
    {
        var optionsBuilder = services.AddOptions<OpenAIOptions>(name);
        if (setupAction != null)
        {
            optionsBuilder.Configure(setupAction);
        }
        else
        {
            optionsBuilder.BindConfiguration($"{OpenAIOptions.SettingKey}:{name}");
        }

        return services.AddHttpClient<TServiceInterface>();
    }
}