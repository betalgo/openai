using Betalgo.OpenAI.Interfaces;
using Betalgo.OpenAI.Managers;
using Microsoft.Extensions.DependencyInjection;

namespace Betalgo.OpenAI.Extensions;

public static class OpenAIServiceCollectionExtensions
{
    public static IHttpClientBuilder AddOpenAIService(this IServiceCollection services, Action<OpenAIOptions>? setupAction = null)
    {
        var optionsBuilder = services.AddOptions<OpenAIOptions>();
        optionsBuilder.BindConfiguration(OpenAIOptions.SettingKey);
        if (setupAction != null)
        {
            optionsBuilder.Configure(setupAction);
        }

        return services.AddHttpClient<IOpenAIService, OpenAIService>();
    }

    public static IHttpClientBuilder AddOpenAIService<TServiceInterface>(this IServiceCollection services, string name, Action<OpenAIOptions>? setupAction = null) where TServiceInterface : class, IOpenAIService
    {
        var optionsBuilder = services.AddOptions<OpenAIOptions>(name);
        optionsBuilder.BindConfiguration($"{OpenAIOptions.SettingKey}:{name}");
        if (setupAction != null)
        {
            optionsBuilder.Configure(setupAction);
        }

        return services.AddHttpClient<TServiceInterface>();
    }
}