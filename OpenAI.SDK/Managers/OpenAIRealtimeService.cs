using System.Buffers;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization.Metadata;
using Betalgo.Ranul.OpenAI.Builders;
using Betalgo.Ranul.OpenAI.ObjectModels;
using Betalgo.Ranul.OpenAI.ObjectModels.RealtimeModels;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
#if !NET6_0_OR_GREATER
using System.Text.Json.Serialization;
#endif

namespace Betalgo.Ranul.OpenAI.Managers;

/// <summary>
/// WebSocket client wrapper for OpenAI Realtime API connections.
/// </summary>
public class OpenAIWebSocketClient
{
    /// <summary>
    /// Gets the underlying WebSocket client instance.
    /// </summary>
    public ClientWebSocket WebSocket { get; } = new();

    /// <summary>
    /// Configures the WebSocket client with custom settings.
    /// </summary>
    /// <param name="configure">Action to configure the WebSocket client.</param>
    public void ConfigureWebSocket(Action<ClientWebSocket> configure)
    {
        configure(WebSocket);
    }
}
/// <summary>
/// Service interface for interacting with the OpenAI Realtime API over WebSocket.
/// Provides real-time communication capabilities for text and audio interactions.
/// </summary>
public interface IOpenAIRealtimeService : IDisposable, IAsyncDisposable
{
    /// <summary>
    /// Gets a value indicating whether the service is currently connected to the OpenAI Realtime API.
    /// </summary>
    /// <value>True if connected to the WebSocket server, otherwise false.</value>
    bool IsConnected { get; }

    /// <summary>
    /// Gets the client events interface for sending events to the OpenAI Realtime API.
    /// These events include session updates, audio buffer operations, conversation management, and response generation.
    /// </summary>
    IOpenAIRealtimeServiceClientEvents ClientEvents { get; }

    /// <summary>
    /// Gets the server events interface for receiving events from the OpenAI Realtime API.
    /// These events include status updates, content streaming, and error notifications.
    /// </summary>
    IOpenAIRealtimeServiceServerEvents ServerEvents { get; }

    /// <summary>
    /// Establishes a WebSocket connection to the OpenAI Realtime API.
    /// </summary>
    /// <param name="cancellationToken">Optional cancellation token to cancel the connection attempt.</param>
    /// <returns>A task that represents the asynchronous connection operation.</returns>
    /// <exception cref="InvalidOperationException">Thrown when already connected or when connection fails.</exception>
    /// <exception cref="OperationCanceledException">Thrown when the operation is canceled.</exception>
    Task ConnectAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Gracefully closes the WebSocket connection to the OpenAI Realtime API.
    /// </summary>
    /// <param name="cancellationToken">Optional cancellation token to cancel the disconnection attempt.</param>
    /// <returns>A task that represents the asynchronous disconnection operation.</returns>
    /// <exception cref="OperationCanceledException">Thrown when the operation is canceled.</exception>
    Task DisconnectAsync(CancellationToken cancellationToken = default);
}

/// <summary>
/// Main implementation of the OpenAI Realtime service providing WebSocket-based communication.
/// Supports real-time text and audio interactions with GPT-4 and related models.
/// </summary>
public partial class OpenAIRealtimeService : IOpenAIRealtimeService
{
    /// <summary>
    /// Initializes a new instance of the OpenAIRealtimeService with dependency injection support.
    /// </summary>
    /// <param name="settings">OpenAI API configuration options.</param>
    /// <param name="logger">Logger instance for service diagnostics.</param>
    /// <param name="webSocketClient">WebSocket client for API communication.</param>
    public OpenAIRealtimeService(IOptions<OpenAIOptions> settings, ILogger<OpenAIRealtimeService> logger, OpenAIWebSocketClient webSocketClient)
    {
        _openAIOptions = settings.Value;
        _logger = logger;
        _webSocketClient = webSocketClient;
        _clientEvents = new(this);
        _serverEvents = new();
        ConfigureBaseWebSocket();
    }

    /// <summary>
    /// Initializes a new instance of the OpenAIRealtimeService with minimal configuration.
    /// </summary>
    /// <param name="options">OpenAI API configuration options.</param>
    public OpenAIRealtimeService(OpenAIOptions options) : this(Options.Create(options), NullLogger<OpenAIRealtimeService>.Instance, new())
    {
    }

    /// <summary>
    /// Initializes a new instance of the OpenAIRealtimeService with logging support.
    /// </summary>
    /// <param name="options">OpenAI API configuration options.</param>
    /// <param name="logger">Logger instance for service diagnostics.</param>
    public OpenAIRealtimeService(OpenAIOptions options, ILogger<OpenAIRealtimeService> logger) : this(Options.Create(options), logger, new())
    {
    }

    /// <summary>
    /// Initializes a new instance of the OpenAIRealtimeService with custom WebSocket configuration.
    /// </summary>
    /// <param name="options">OpenAI API configuration options.</param>
    /// <param name="configureWebSocket">Optional action to configure the WebSocket client.</param>
    public OpenAIRealtimeService(OpenAIOptions options, Action<ClientWebSocket>? configureWebSocket = null) : this(options)
    {
        if (configureWebSocket != null)
        {
            _webSocketClient.ConfigureWebSocket(configureWebSocket);
        }
    }

    /// <summary>
    /// Initializes a new instance of the OpenAIRealtimeService with just an API key.
    /// </summary>
    /// <param name="apiKey">OpenAI API key for authentication.</param>
    public OpenAIRealtimeService(string apiKey) : this(new OpenAIOptions { ApiKey = apiKey })
    {
    }

    /// <summary>
    /// Creates a new OpenAIRealtimeService builder instance using an API key.
    /// </summary>
    /// <param name="apiKey">OpenAI API key for authentication.</param>
    /// <returns>A builder instance for configuring the service.</returns>
    public static OpenAIRealtimeServiceBuilder Create(string apiKey)
    {
        return new(apiKey);
    }

    /// <summary>
    /// Creates a new OpenAIRealtimeService builder instance using configuration options.
    /// </summary>
    /// <param name="options">OpenAI API configuration options.</param>
    /// <returns>A builder instance for configuring the service.</returns>
    public static OpenAIRealtimeServiceBuilder Create(OpenAIOptions options)
    {
        return new(options);
    }

    /// <summary>
    /// Configures the base WebSocket connection with authentication headers.
    /// </summary>
    private void ConfigureBaseWebSocket()
    {
        var headers = new Dictionary<string, string>
        {
            { RealtimeConstants.Headers.Authorization, $"Bearer {_openAIOptions.ApiKey}" }
        };

        if (!string.IsNullOrEmpty(_openAIOptions.Organization))
        {
            headers[RealtimeConstants.Headers.OpenAIOrganization] = _openAIOptions.Organization;
        }

        _webSocketClient.ConfigureWebSocket(ws => WebSocketConfigurationHelper.ConfigureWebSocket(ws, headers: headers));
    }

}


public partial class OpenAIRealtimeService : IOpenAIRealtimeService
{
#if !NET6_0_OR_GREATER
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        ReadCommentHandling = JsonCommentHandling.Disallow, // Disallow comments
        AllowTrailingCommas = false,                        // Disallow trailing commas
        PropertyNameCaseInsensitive = false,                // Case-sensitive property names
        WriteIndented = false                               // Disable indentation for compact output
    };
#endif

    private readonly CancellationTokenSource _disposeCts = new();
    private readonly ILogger<OpenAIRealtimeService> _logger;
    private readonly OpenAIOptions _openAIOptions;
    private readonly SemaphoreSlim _sendLock = new(1, 1);
    private Task? _receiveTask;
    private ClientWebSocket? _webSocket;
    private readonly object _webSocketLock = new();

    private readonly ClientEventsImplementation _clientEvents;
    private readonly OpenAIRealtimeServiceServerEvents _serverEvents;
    private readonly OpenAIWebSocketClient _webSocketClient;


    private static readonly Dictionary<string, Action<OpenAIRealtimeService, JsonElement>> EventHandlers = InitializeEventHandlers();

    /// <inheritdoc />
    public bool IsConnected
    {
        get
        {
            lock (_webSocketLock)
            {
                return _webSocket?.State == WebSocketState.Open;
            }
        }
    }

    private static Dictionary<string, Action<OpenAIRealtimeService, JsonElement>> InitializeEventHandlers()
    {
        return new()
        {
            // Error
            { RealtimeEventTypes.Server.Error, (service, element) => service._serverEvents.RaiseOnError(service, service.DeserializeEvent<ErrorEvent>(element)) },

            // Session events
            { RealtimeEventTypes.Server.Session.Created, (service, element) => service._serverEvents.SessionImpl.RaiseOnCreated(service, service.DeserializeEvent<SessionEvent>(element)) },
            { RealtimeEventTypes.Server.Session.Updated, (service, element) => service._serverEvents.SessionImpl.RaiseOnUpdated(service, service.DeserializeEvent<SessionEvent>(element)) },

            // Conversation events
            { RealtimeEventTypes.Server.Conversation.Created, (service, element) => service._serverEvents.ConversationImpl.RaiseOnCreated(service, service.DeserializeEvent<ConversationCreatedEvent>(element)) },
            { RealtimeEventTypes.Server.Conversation.Item.Created, (service, element) => service._serverEvents.ConversationImpl.ItemImpl.RaiseOnCreated(service, service.DeserializeEvent<ConversationItemCreatedEvent>(element)) },
            { RealtimeEventTypes.Server.Conversation.Item.Truncated, (service, element) => service._serverEvents.ConversationImpl.ItemImpl.RaiseOnTruncated(service, service.DeserializeEvent<ConversationItemTruncatedEvent>(element)) },
            { RealtimeEventTypes.Server.Conversation.Item.Deleted, (service, element) => service._serverEvents.ConversationImpl.ItemImpl.RaiseOnDeleted(service, service.DeserializeEvent<ConversationItemDeletedEvent>(element)) },
            {
                RealtimeEventTypes.Server.Conversation.Item.InputAudioTranscription.Completed,
                (service, element) => service._serverEvents.ConversationImpl.ItemImpl.InputAudioTranscriptionImpl.RaiseOnCompleted(service, service.DeserializeEvent<InputAudioTranscriptionCompletedEvent>(element))
            },
            {
                RealtimeEventTypes.Server.Conversation.Item.InputAudioTranscription.Failed,
                (service, element) => service._serverEvents.ConversationImpl.ItemImpl.InputAudioTranscriptionImpl.RaiseOnFailed(service, service.DeserializeEvent<InputAudioTranscriptionFailedEvent>(element))
            },

            // InputAudioBuffer events
            { RealtimeEventTypes.Server.InputAudioBuffer.Committed, (service, element) => service._serverEvents.InputAudioBufferImpl.RaiseOnCommitted(service, service.DeserializeEvent<AudioBufferCommittedEvent>(element)) },
            { RealtimeEventTypes.Server.InputAudioBuffer.Cleared, (service, element) => service._serverEvents.InputAudioBufferImpl.RaiseOnCleared(service, service.DeserializeEvent<AudioBufferClearedEvent>(element)) },
            { RealtimeEventTypes.Server.InputAudioBuffer.SpeechStarted, (service, element) => service._serverEvents.InputAudioBufferImpl.RaiseOnSpeechStarted(service, service.DeserializeEvent<AudioBufferSpeechStartedEvent>(element)) },
            { RealtimeEventTypes.Server.InputAudioBuffer.SpeechStopped, (service, element) => service._serverEvents.InputAudioBufferImpl.RaiseOnSpeechStopped(service, service.DeserializeEvent<AudioBufferSpeechStoppedEvent>(element)) },

            // Response events
            { RealtimeEventTypes.Server.Response.Created, (service, element) => service._serverEvents.ResponseImpl.RaiseOnCreated(service, service.DeserializeEvent<ResponseEvent>(element)) },
            { RealtimeEventTypes.Server.Response.Done, (service, element) => service._serverEvents.ResponseImpl.RaiseOnDone(service, service.DeserializeEvent<ResponseEvent>(element)) },

            // Response OutputItem events
            { RealtimeEventTypes.Server.Response.OutputItem.Added, (service, element) => service._serverEvents.ResponseImpl.OutputItemImpl.RaiseOnAdded(service, service.DeserializeEvent<ResponseOutputItemAddedEvent>(element)) },
            { RealtimeEventTypes.Server.Response.OutputItem.Done, (service, element) => service._serverEvents.ResponseImpl.OutputItemImpl.RaiseOnDone(service, service.DeserializeEvent<ResponseOutputItemDoneEvent>(element)) },

            // Response ContentPart events
            { RealtimeEventTypes.Server.Response.ContentPart.Added, (service, element) => service._serverEvents.ResponseImpl.ContentPartImpl.RaiseOnAdded(service, service.DeserializeEvent<ResponseContentPartEvent>(element)) },
            { RealtimeEventTypes.Server.Response.ContentPart.Done, (service, element) => service._serverEvents.ResponseImpl.ContentPartImpl.RaiseOnDone(service, service.DeserializeEvent<ResponseContentPartEvent>(element)) },

            // Response Text events
            { RealtimeEventTypes.Server.Response.Text.Delta, (service, element) => service._serverEvents.ResponseImpl.TextImpl.RaiseOnDelta(service, service.DeserializeEvent<TextStreamEvent>(element)) },
            { RealtimeEventTypes.Server.Response.Text.Done, (service, element) => service._serverEvents.ResponseImpl.TextImpl.RaiseOnDone(service, service.DeserializeEvent<TextStreamEvent>(element)) },

            // Response AudioTranscript events
            { RealtimeEventTypes.Server.Response.AudioTranscript.Delta, (service, element) => service._serverEvents.ResponseImpl.AudioTranscriptImpl.RaiseOnDelta(service, service.DeserializeEvent<AudioTranscriptStreamEvent>(element)) },
            { RealtimeEventTypes.Server.Response.AudioTranscript.Done, (service, element) => service._serverEvents.ResponseImpl.AudioTranscriptImpl.RaiseOnDone(service, service.DeserializeEvent<AudioTranscriptStreamEvent>(element)) },

            // Response Audio events
            { RealtimeEventTypes.Server.Response.Audio.Delta, (service, element) => service._serverEvents.ResponseImpl.AudioImpl.RaiseOnDelta(service, service.DeserializeEvent<AudioStreamEvent>(element)) },
            { RealtimeEventTypes.Server.Response.Audio.Done, (service, element) => service._serverEvents.ResponseImpl.AudioImpl.RaiseOnDone(service, service.DeserializeEvent<AudioStreamEvent>(element)) },

            // Response FunctionCallArguments events
            {
                RealtimeEventTypes.Server.Response.FunctionCallArguments.Delta,
                (service, element) => service._serverEvents.ResponseImpl.FunctionCallArgumentsImpl.RaiseOnDelta(service, service.DeserializeEvent<FunctionCallStreamEvent>(element))
            },
            {
                RealtimeEventTypes.Server.Response.FunctionCallArguments.Done,
                (service, element) => service._serverEvents.ResponseImpl.FunctionCallArgumentsImpl.RaiseOnDone(service, service.DeserializeEvent<FunctionCallStreamEvent>(element))
            },

            // RateLimits events
            { RealtimeEventTypes.Server.RateLimits.Updated, (service, element) => service._serverEvents.RateLimitsImpl.RaiseOnUpdated(service, service.DeserializeEvent<RateLimitsEvent>(element)) }
        };
    }

    public async Task ConnectAsync(CancellationToken cancellationToken = default)
    {
        using var linkedCts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken, _disposeCts.Token);
        linkedCts.Token.ThrowIfCancellationRequested();

        if (IsConnected)
            throw new InvalidOperationException("Already connected to Realtime API.");

        try
        {
            var webSocket = _webSocketClient.WebSocket;
            var url = new Uri($"{_openAIOptions.BaseRealTimeSocketUrl}?model={_openAIOptions.DefaultModelId ?? Models.Gpt_4o_realtime_preview_2024_10_01}");
            await webSocket.ConnectAsync(url, linkedCts.Token).ConfigureAwait(false);

            if (webSocket.State != WebSocketState.Open)
            {
                throw new InvalidOperationException($"WebSocket connection failed. Current state: {webSocket.State}");
            }

            lock (_webSocketLock)
            {
                _webSocket = webSocket;
            }

            _logger.LogInformation("Successfully connected to Realtime API");
            _receiveTask = StartReceiving(linkedCts.Token);
        }
        catch (OperationCanceledException)
        {
            _logger.LogInformation("Connection canceled.");
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to connect to Realtime API");
            throw;
        }
    }

    /// <inheritdoc />
    public async Task DisconnectAsync(CancellationToken cancellationToken = default)
    {
        using var linkedCts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken, _disposeCts.Token);
        linkedCts.Token.ThrowIfCancellationRequested();

        ClientWebSocket? webSocket;
        lock (_webSocketLock)
        {
            webSocket = _webSocket;
        }

        if (webSocket == null || webSocket.State != WebSocketState.Open) return;

        try
        {
            await webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Disconnecting", linkedCts.Token).ConfigureAwait(false);
        }
        catch (OperationCanceledException)
        {
            _logger.LogInformation("Disconnection canceled.");
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during disconnection");
            throw;
        }
        finally
        {
            CleanupWebSocket();
        }
    }

    private Task HandleMessage(string message)
    {
        _serverEvents.RaiseOnAll(this, message);

        try
        {
            using var doc = JsonDocument.Parse(message);
            var rootElement = doc.RootElement;

            if (!rootElement.TryGetProperty("type", out var typeElement))
            {
                _logger.LogWarning("Received message without type");
                return Task.CompletedTask;
            }

            var type = typeElement.GetString();

            if (type != null && EventHandlers.TryGetValue(type, out var handler))
            {
                handler(this, rootElement);
            }
            else
            {
                _logger.LogWarning("Received unknown event type: {Type}", type);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing message");
            var errorEvent = new ErrorEvent
            {
                Error = new()
                {
                    Type = "Ranul.OpenAI_processing_error",
                    MessageObject = ex.Message
                }
            };
            _serverEvents.RaiseOnError(this, errorEvent);
        }

        return Task.CompletedTask;
    }

    private async Task SendEvent<T>(T message, CancellationToken cancellationToken) where T : class
    {
        try
        {
            await _sendLock.WaitAsync(cancellationToken).ConfigureAwait(false);
            try
            {
#if NET6_0_OR_GREATER
                var json = JsonSerializer.Serialize(message, message.GetType(), RealtimeServiceJsonContext.Default);
#else
                var json = JsonSerializer.Serialize(message, JsonOptions);
#endif
                var buffer = Encoding.UTF8.GetBytes(json);

                ClientWebSocket? webSocket;
                lock (_webSocketLock)
                {
                    webSocket = _webSocket;
                }

                if (webSocket == null)
                    throw new InvalidOperationException("WebSocket is not initialized.");


                await webSocket.SendAsync(new(buffer), WebSocketMessageType.Text, true, cancellationToken).ConfigureAwait(false);
            }
            finally
            {
                _sendLock.Release();
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error sending message");
            throw;
        }
    }

    private T DeserializeEvent<T>(JsonElement jsonElement) where T : class
    {
#if NET6_0_OR_GREATER
        var typeInfo = RealtimeServiceJsonContext.Default.GetTypeInfo(typeof(T));
        return typeInfo != null ? jsonElement.Deserialize((JsonTypeInfo<T>)typeInfo)! : jsonElement.Deserialize<T>()!;
#else
        // For earlier versions, JsonSerializer does not support deserializing directly from JsonElement.
        // We need to use GetRawText(), which does incur some overhead but avoids reparsing the entire message.
        return JsonSerializer.Deserialize<T>(jsonElement.GetRawText(), JsonOptions)!;
#endif
    }

    private async Task StartReceiving(CancellationToken cancellationToken)
    {
        ClientWebSocket? webSocket;
        lock (_webSocketLock)
        {
            webSocket = _webSocket;
        }

        if (webSocket == null)
            throw new InvalidOperationException("WebSocket is not initialized.");

        var buffer = ArrayPool<byte>.Shared.Rent(16 * 1);
        var textBuffer = new StringBuilder();

        try
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                WebSocketReceiveResult result;
                try
                {
                    result = await webSocket.ReceiveAsync(new(buffer), cancellationToken).ConfigureAwait(false);
                }
                catch (WebSocketException ex) when (ex.WebSocketErrorCode == WebSocketError.ConnectionClosedPrematurely)
                {
                    _logger.LogWarning("WebSocket connection closed prematurely.");
                    break;
                }
                catch (ObjectDisposedException)
                {
                    break;
                }

                if (result.MessageType == WebSocketMessageType.Close)
                {
                    await HandleCloseMessage().ConfigureAwait(false);
                    break;
                }

                if (result.MessageType == WebSocketMessageType.Text)
                {
                    textBuffer.Append(Encoding.UTF8.GetString(buffer, 0, result.Count));

                    if (result.EndOfMessage)
                    {
                        await HandleMessage(textBuffer.ToString()).ConfigureAwait(false);
                        textBuffer.Clear();
                    }
                }
            }
        }
        catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
        {
            // Normal cancellation, do nothing
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in receive loop");
            _serverEvents.RaiseOnError(this, new()
            {
                Error = new()
                {
                    MessageObject = ex.Message,
                    Type = "Ranul.OpenAI_receive_error"
                }
            });
        }
        finally
        {
            ArrayPool<byte>.Shared.Return(buffer);
        }
    }

    private async Task HandleCloseMessage()
    {
        await DisconnectAsync().ConfigureAwait(false);
    }

    private void CleanupWebSocket()
    {
        ClientWebSocket? webSocket;
        lock (_webSocketLock)
        {
            webSocket = _webSocket;
            _webSocket = null;
        }

        if (webSocket != null)
        {
            if (webSocket.State == WebSocketState.Open)
            {
                try
                {
                    webSocket.Abort();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error aborting WebSocket");
                }
            }

            webSocket.Dispose();
        }
    }

    /// <inheritdoc />
    public void Dispose()
    {
        _disposeCts.Cancel();
        CleanupWebSocket();
        _sendLock.Dispose();
        _disposeCts.Dispose();
    }

    /// <inheritdoc />
    public async ValueTask DisposeAsync()
    {
        _disposeCts.Cancel();

        if (_receiveTask != null)
        {
            await _receiveTask.ConfigureAwait(false);
        }

        CleanupWebSocket();
        _sendLock.Dispose();
        _disposeCts.Dispose();
    }
}
