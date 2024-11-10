using System.Net.WebSockets;
using Betalgo.Ranul.OpenAI.Managers;
using Betalgo.Ranul.OpenAI.ObjectModels.RealtimeModels;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;

namespace Betalgo.Ranul.OpenAI.Builders;

public class OpenAIRealtimeServiceBuilder
{
    private readonly Dictionary<string, string> _headers = new();
    private readonly OpenAIOptions _options;
    private readonly IServiceCollection? _services;
    private Action<ClientWebSocketOptions>? _configureOptions;
    private Action<ClientWebSocket>? _configureWebSocket;
    private ILogger<OpenAIRealtimeService>? _logger;
    private ServiceLifetime _serviceLifetime = ServiceLifetime.Singleton;

    // Constructors for standalone and DI scenarios
    public OpenAIRealtimeServiceBuilder(string apiKey) : this(new OpenAIOptions { ApiKey = apiKey })
    {
    }

    public OpenAIRealtimeServiceBuilder(OpenAIOptions options, ILogger<OpenAIRealtimeService>? logger = null)
    {
        _options = options;
        _logger = logger;
    }

    public OpenAIRealtimeServiceBuilder(IServiceCollection services, ServiceLifetime lifetime = ServiceLifetime.Singleton)
    {
        _services = services;
        _serviceLifetime = lifetime;
    }

    public OpenAIRealtimeServiceBuilder ConfigureWebSocket(Action<ClientWebSocket> configure)
    {
        _configureWebSocket = configure;
        return this;
    }

    public OpenAIRealtimeServiceBuilder ConfigureOptions(Action<ClientWebSocketOptions> configure)
    {
        _configureOptions = configure;
        return this;
    }

    public OpenAIRealtimeServiceBuilder AddHeader(string name, string value)
    {
        _headers[name] = value;
        return this;
    }

    public OpenAIRealtimeServiceBuilder WithLogger(ILogger<OpenAIRealtimeService> logger)
    {
        _logger = logger;
        return this;
    }

    public OpenAIRealtimeServiceBuilder AsSingleton()
    {
        _serviceLifetime = ServiceLifetime.Singleton;
        return this;
    }

    public OpenAIRealtimeServiceBuilder AsScoped()
    {
        _serviceLifetime = ServiceLifetime.Scoped;
        return this;
    }

    public OpenAIRealtimeServiceBuilder AsTransient()
    {
        _serviceLifetime = ServiceLifetime.Transient;
        return this;
    }

    public IOpenAIRealtimeService? Build()
    {
        if (_services == null)
        {
            // Standalone configuration
            var client = new OpenAIWebSocketClient();
            ConfigureClient(client);
            return new OpenAIRealtimeService(Options.Create(_options), _logger ?? NullLogger<OpenAIRealtimeService>.Instance, client);
        }
        else
        {
            switch (_serviceLifetime)
            {
                case ServiceLifetime.Singleton:
                    _services.AddSingleton(sp =>
                    {
                        var client = new OpenAIWebSocketClient();
                        ConfigureClient(client);
                        return client;
                    });
                    _services.AddSingleton<IOpenAIRealtimeService, OpenAIRealtimeService>();
                    break;
                case ServiceLifetime.Scoped:
                    _services.AddScoped(sp =>
                    {
                        var client = new OpenAIWebSocketClient();
                        ConfigureClient(client);
                        return client;
                    });
                    _services.AddScoped<IOpenAIRealtimeService, OpenAIRealtimeService>();
                    break;
                case ServiceLifetime.Transient:
                    _services.AddTransient(sp =>
                    {
                        var client = new OpenAIWebSocketClient();
                        ConfigureClient(client);
                        return client;
                    });
                    _services.AddTransient<IOpenAIRealtimeService, OpenAIRealtimeService>();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        return null;
    }

    private void ConfigureClient(OpenAIWebSocketClient client)
    {
        client.ConfigureWebSocket(ws => WebSocketConfigurationHelper.ConfigureWebSocket(ws, _configureOptions, _headers, _configureWebSocket));
    }
}

public static class WebSocketConfigurationHelper
{
    public static void ConfigureWebSocket(ClientWebSocket webSocket, Action<ClientWebSocketOptions>? configureOptions = null, IReadOnlyDictionary<string, string>? headers = null, Action<ClientWebSocket>? configureWebSocket = null)
    {
        try
        {
            webSocket.Options.AddSubProtocol(RealtimeConstants.SubProtocols.Realtime);
            webSocket.Options.AddSubProtocol(RealtimeConstants.SubProtocols.Beta);
        }
        catch (ArgumentException)
        {
        }

        configureOptions?.Invoke(webSocket.Options);

        if (headers != null)
        {
            foreach (var header in headers)
            {
                try
                {
                    webSocket.Options.SetRequestHeader(header.Key, header.Value);
                }
                catch (ArgumentException)
                {
                }
            }
        }

        configureWebSocket?.Invoke(webSocket);
    }
}