using System.Net;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using OpenAI.GPT3.Interfaces;
using OpenAI.GPT3.Managers;

namespace OpenAI.GPT3.Extensions;

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

        var serviceProvider = optionsBuilder.Services.BuildServiceProvider();
        var httpProxy = serviceProvider.GetRequiredService<IOptions<OpenAiOptions>>().Value.HttpProxy;

        return httpProxy != null
            ? services.AddHttpClient<IOpenAIService, OpenAIService>().ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler {Proxy = new WebProxy(httpProxy), UseProxy = true})
            : services.AddHttpClient<IOpenAIService, OpenAIService>();
    }
}