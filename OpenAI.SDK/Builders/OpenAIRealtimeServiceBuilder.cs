using System.Net.WebSockets;
using Betalgo.Ranul.OpenAI.Managers;
using Betalgo.Ranul.OpenAI.ObjectModels.RealtimeModels;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;

namespace Betalgo.Ranul.OpenAI.Builders;

/// <summary>
/// Builder class for configuring and creating OpenAI Realtime WebSocket services.
/// Provides functionality to establish WebSocket connections for real-time communication
/// with OpenAI's GPT models, supporting both audio and text modalities.
/// </summary>
public class OpenAIRealtimeServiceBuilder
{
    private readonly Dictionary<string, string> _headers = new();
    private readonly OpenAIOptions _options;
    private readonly IServiceCollection? _services;
    private Action<ClientWebSocketOptions>? _configureOptions;
    private Action<ClientWebSocket>? _configureWebSocket;
    private ILogger<OpenAIRealtimeService>? _logger;
    private ServiceLifetime _serviceLifetime = ServiceLifetime.Singleton;

    /// <summary>
    /// Initializes a new instance of the OpenAIRealtimeServiceBuilder with an API key.
    /// </summary>
    /// <param name="apiKey">The OpenAI API key used for authentication.</param>
    public OpenAIRealtimeServiceBuilder(string apiKey) : this(new OpenAIOptions { ApiKey = apiKey })
    {
    }

    /// <summary>
    /// Initializes a new instance of the OpenAIRealtimeServiceBuilder with custom options and optional logger.
    /// </summary>
    /// <param name="options">The OpenAI configuration options.</param>
    /// <param name="logger">Optional logger for service diagnostics.</param>
    public OpenAIRealtimeServiceBuilder(OpenAIOptions options, ILogger<OpenAIRealtimeService>? logger = null)
    {
        _options = options;
        _logger = logger;
    }

    /// <summary>
    /// Initializes a new instance of the OpenAIRealtimeServiceBuilder for dependency injection scenarios.
    /// </summary>
    /// <param name="services">The service collection for dependency injection.</param>
    /// <param name="lifetime">The service lifetime (Singleton by default).</param>
    public OpenAIRealtimeServiceBuilder(IServiceCollection services, ServiceLifetime lifetime = ServiceLifetime.Singleton)
    {
        _services = services;
        _serviceLifetime = lifetime;
    }

    /// <summary>
    /// Configures the WebSocket instance after creation.
    /// </summary>
    /// <param name="configure">Action to configure the WebSocket.</param>
    /// <returns>The builder instance for method chaining.</returns>
    public OpenAIRealtimeServiceBuilder ConfigureWebSocket(Action<ClientWebSocket> configure)
    {
        _configureWebSocket = configure;
        return this;
    }

    /// <summary>
    /// Configures the WebSocket options before connection.
    /// </summary>
    /// <param name="configure">Action to configure the WebSocket options.</param>
    /// <returns>The builder instance for method chaining.</returns>
    public OpenAIRealtimeServiceBuilder ConfigureOptions(Action<ClientWebSocketOptions> configure)
    {
        _configureOptions = configure;
        return this;
    }

    /// <summary>
    /// Adds a custom header to the WebSocket connection request.
    /// </summary>
    /// <param name="name">The header name.</param>
    /// <param name="value">The header value.</param>
    /// <returns>The builder instance for method chaining.</returns>
    public OpenAIRealtimeServiceBuilder AddHeader(string name, string value)
    {
        _headers[name] = value;
        return this;
    }

    /// <summary>
    /// Sets the logger for the service.
    /// </summary>
    /// <param name="logger">The logger instance.</param>
    /// <returns>The builder instance for method chaining.</returns>
    public OpenAIRealtimeServiceBuilder WithLogger(ILogger<OpenAIRealtimeService> logger)
    {
        _logger = logger;
        return this;
    }

    /// <summary>
    /// Sets the service lifetime to Singleton.
    /// </summary>
    /// <returns>The builder instance for method chaining.</returns>
    public OpenAIRealtimeServiceBuilder AsSingleton()
    {
        _serviceLifetime = ServiceLifetime.Singleton;
        return this;
    }

    /// <summary>
    /// Sets the service lifetime to Scoped.
    /// </summary>
    /// <returns>The builder instance for method chaining.</returns>
    public OpenAIRealtimeServiceBuilder AsScoped()
    {
        _serviceLifetime = ServiceLifetime.Scoped;
        return this;
    }

    /// <summary>
    /// Sets the service lifetime to Transient.
    /// </summary>
    /// <returns>The builder instance for method chaining.</returns>
    public OpenAIRealtimeServiceBuilder AsTransient()
    {
        _serviceLifetime = ServiceLifetime.Transient;
        return this;
    }

    /// <summary>
    /// Builds and returns an instance of IOpenAIRealtimeService.
    /// When used with dependency injection, registers the service with the specified lifetime
    /// and returns null. When used standalone, returns the configured service instance.
    /// </summary>
    /// <returns>
    /// The configured IOpenAIRealtimeService instance for standalone usage,
    /// or null when used with dependency injection.
    /// </returns>
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

/// <summary>
/// Helper class for configuring WebSocket connections with OpenAI Realtime API settings.
/// </summary>
public static class WebSocketConfigurationHelper
{
    /// <summary>
    /// Configures a WebSocket instance with OpenAI Realtime API specific settings.
    /// Sets up required subprotocols and applies custom configurations.
    /// </summary>
    /// <param name="webSocket">The WebSocket instance to configure.</param>
    /// <param name="configureOptions">Optional action to configure WebSocket options.</param>
    /// <param name="headers">Optional dictionary of custom headers to add to the connection.</param>
    /// <param name="configureWebSocket">Optional action to perform additional WebSocket configuration.</param>
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