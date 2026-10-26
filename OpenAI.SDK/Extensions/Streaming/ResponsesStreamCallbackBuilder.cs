using Betalgo.Ranul.OpenAI.Contracts.Responses.Responses;
using Betalgo.Ranul.OpenAI.Contracts.Types.Streaming;
using Betalgo.Ranul.OpenAI.Contracts.Types.Streaming.Events;
using Betalgo.Ranul.OpenAI.Contracts.Types.Streaming.Events.FunctionCalls;
using Betalgo.Ranul.OpenAI.Contracts.Types.Streaming.Events.Lifecycle;
using Betalgo.Ranul.OpenAI.Contracts.Types.Streaming.Events.Text;
using Betalgo.Ranul.OpenAI.Contracts.Types.Streaming.Events.Tools;

namespace Betalgo.Ranul.OpenAI.Extensions.Streaming;

/// <summary>
///     Extension methods for creating fluent callback handlers for Responses API streaming.
/// </summary>
public static class ResponsesStreamCallbackExtensions
{
    /// <summary>
    ///     Creates a fluent callback builder starting with a text delta handler.
    /// </summary>
    /// <param name="events">The stream of events.</param>
    /// <param name="handler">Handler for text delta strings.</param>
    /// <returns>A ResponsesStreamHandler for chaining additional callbacks.</returns>
    /// <example>
    ///     <code>
    /// await openAi.Responses.CreateAsStreamAsync(request)
    ///     .OnTextDelta(delta => Console.Write(delta))
    ///     .OnFunctionCallDone(call => HandleFunction(call))
    ///     .OnCompleted(response => SaveToDatabase(response))
    ///     .ExecuteAsync();
    /// </code>
    /// </example>
    public static ResponsesStreamHandler OnTextDelta(
        this IAsyncEnumerable<IResponseStreamEvent> events,
        Action<string> handler)
    {
        return new ResponsesStreamHandler(events).OnTextDelta(handler);
    }

    /// <summary>
    ///     Creates a fluent callback builder starting with any event handler.
    /// </summary>
    /// <param name="events">The stream of events.</param>
    /// <param name="handler">Handler for all events.</param>
    /// <returns>A ResponsesStreamHandler for chaining additional callbacks.</returns>
    public static ResponsesStreamHandler OnAnyEvent(
        this IAsyncEnumerable<IResponseStreamEvent> events,
        Action<IResponseStreamEvent> handler)
    {
        return new ResponsesStreamHandler(events).OnAnyEvent(handler);
    }
}

/// <summary>
///     Fluent builder for handling Responses API streaming events with callbacks.
/// </summary>
public class ResponsesStreamHandler
{
    private readonly IAsyncEnumerable<IResponseStreamEvent> _events;
    private Action<string>? _textDeltaHandler;
    private Action<string>? _textDoneHandler;
    private Action<ResponseFunctionCallArgumentsDeltaEvent>? _functionCallDeltaHandler;
    private Action<ResponseFunctionCallArgumentsDoneEvent>? _functionCallDoneHandler;
    private Action<string>? _reasoningDeltaHandler;
    private Action<string>? _reasoningDoneHandler;
    private Action<IResponseStreamEvent>? _toolCallInProgressHandler;
    private Action<IResponseStreamEvent>? _toolCallCompletedHandler;
    private Action<ResponseErrorEvent>? _errorHandler;
    private Action<Response>? _completedHandler;
    private Action<Response>? _failedHandler;
    private Action<UnknownResponseStreamEvent>? _unknownEventHandler;
    private Action<IResponseStreamEvent>? _anyEventHandler;

    /// <summary>
    ///     Creates a new ResponsesStreamHandler for the given event stream.
    /// </summary>
    /// <param name="events">The stream of events to handle.</param>
    public ResponsesStreamHandler(IAsyncEnumerable<IResponseStreamEvent> events)
    {
        _events = events;
    }

    /// <summary>
    ///     Registers a handler for text delta events.
    /// </summary>
    public ResponsesStreamHandler OnTextDelta(Action<string> handler)
    {
        _textDeltaHandler = handler;
        return this;
    }

    /// <summary>
    ///     Registers a handler for text done events.
    /// </summary>
    public ResponsesStreamHandler OnTextDone(Action<string> handler)
    {
        _textDoneHandler = handler;
        return this;
    }

    /// <summary>
    ///     Registers a handler for function call argument delta events.
    /// </summary>
    public ResponsesStreamHandler OnFunctionCallDelta(Action<ResponseFunctionCallArgumentsDeltaEvent> handler)
    {
        _functionCallDeltaHandler = handler;
        return this;
    }

    /// <summary>
    ///     Registers a handler for function call argument done events.
    /// </summary>
    public ResponsesStreamHandler OnFunctionCallDone(Action<ResponseFunctionCallArgumentsDoneEvent> handler)
    {
        _functionCallDoneHandler = handler;
        return this;
    }

    /// <summary>
    ///     Registers a handler for reasoning text delta events (for o1/o3 models).
    /// </summary>
    public ResponsesStreamHandler OnReasoningDelta(Action<string> handler)
    {
        _reasoningDeltaHandler = handler;
        return this;
    }

    /// <summary>
    ///     Registers a handler for reasoning text done events (for o1/o3 models).
    /// </summary>
    public ResponsesStreamHandler OnReasoningDone(Action<string> handler)
    {
        _reasoningDoneHandler = handler;
        return this;
    }

    /// <summary>
    ///     Registers a handler for tool call in progress events.
    /// </summary>
    public ResponsesStreamHandler OnToolCallInProgress(Action<IResponseStreamEvent> handler)
    {
        _toolCallInProgressHandler = handler;
        return this;
    }

    /// <summary>
    ///     Registers a handler for tool call completed events.
    /// </summary>
    public ResponsesStreamHandler OnToolCallCompleted(Action<IResponseStreamEvent> handler)
    {
        _toolCallCompletedHandler = handler;
        return this;
    }

    /// <summary>
    ///     Registers a handler for error events.
    /// </summary>
    public ResponsesStreamHandler OnError(Action<ResponseErrorEvent> handler)
    {
        _errorHandler = handler;
        return this;
    }

    /// <summary>
    ///     Registers a handler for response completed events.
    /// </summary>
    public ResponsesStreamHandler OnCompleted(Action<Response> handler)
    {
        _completedHandler = handler;
        return this;
    }

    /// <summary>
    ///     Registers a handler for response failed events.
    /// </summary>
    public ResponsesStreamHandler OnFailed(Action<Response> handler)
    {
        _failedHandler = handler;
        return this;
    }

    /// <summary>
    ///     Registers a handler for unknown/unrecognized event types.
    ///     This is useful for handling new event types before the SDK is updated.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         When OpenAI introduces new streaming event types, they will be captured as
    ///         <see cref="UnknownResponseStreamEvent" /> instances until the SDK is updated.
    ///     </para>
    ///     <para>
    ///         You can access the raw JSON via <see cref="UnknownResponseStreamEvent.RawJson" />
    ///         to manually parse or log new event types.
    ///     </para>
    /// </remarks>
    public ResponsesStreamHandler OnUnknownEvent(Action<UnknownResponseStreamEvent> handler)
    {
        _unknownEventHandler = handler;
        return this;
    }

    /// <summary>
    ///     Registers a handler for any event not handled by specific callbacks.
    /// </summary>
    public ResponsesStreamHandler OnAnyEvent(Action<IResponseStreamEvent> handler)
    {
        _anyEventHandler = handler;
        return this;
    }

    /// <summary>
    ///     Executes the stream, invoking registered callbacks for each event.
    /// </summary>
    /// <param name="cancellationToken">Cancellation token.</param>
    public async Task ExecuteAsync(CancellationToken cancellationToken = default)
    {
        await foreach (var evt in _events.WithCancellation(cancellationToken))
        {
            ProcessEvent(evt);
        }
    }

    /// <summary>
    ///     Executes the stream and returns the final Response.
    /// </summary>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The final Response object, or null if not found.</returns>
    public async Task<Response?> ExecuteAndGetResponseAsync(CancellationToken cancellationToken = default)
    {
        Response? finalResponse = null;

        await foreach (var evt in _events.WithCancellation(cancellationToken))
        {
            ProcessEvent(evt);

            switch (evt)
            {
                case ResponseCompletedEvent completed:
                    finalResponse = completed.Response;
                    break;
                case ResponseFailedEvent failed:
                    finalResponse = failed.Response;
                    break;
                case ResponseIncompleteEvent incomplete:
                    finalResponse = incomplete.Response;
                    break;
            }
        }

        return finalResponse;
    }

    private void ProcessEvent(IResponseStreamEvent evt)
    {
        var handled = false;

        switch (evt)
        {
            case ResponseTextDeltaEvent textDelta:
                _textDeltaHandler?.Invoke(textDelta.Delta);
                handled = _textDeltaHandler != null;
                break;

            case ResponseTextDoneEvent textDone:
                _textDoneHandler?.Invoke(textDone.Text);
                handled = _textDoneHandler != null;
                break;

            case ResponseFunctionCallArgumentsDeltaEvent funcDelta:
                _functionCallDeltaHandler?.Invoke(funcDelta);
                handled = _functionCallDeltaHandler != null;
                break;

            case ResponseFunctionCallArgumentsDoneEvent funcDone:
                _functionCallDoneHandler?.Invoke(funcDone);
                handled = _functionCallDoneHandler != null;
                break;

            case ResponseReasoningTextDeltaEvent reasoningDelta:
                _reasoningDeltaHandler?.Invoke(reasoningDelta.Delta);
                handled = _reasoningDeltaHandler != null;
                break;

            case ResponseReasoningTextDoneEvent reasoningDone:
                _reasoningDoneHandler?.Invoke(reasoningDone.Text);
                handled = _reasoningDoneHandler != null;
                break;

            case ResponseErrorEvent error:
                _errorHandler?.Invoke(error);
                handled = _errorHandler != null;
                break;

            case ResponseCompletedEvent completed:
                _completedHandler?.Invoke(completed.Response);
                handled = _completedHandler != null;
                break;

            case ResponseFailedEvent failed:
                _failedHandler?.Invoke(failed.Response);
                handled = _failedHandler != null;
                break;

            case UnknownResponseStreamEvent unknown:
                _unknownEventHandler?.Invoke(unknown);
                handled = _unknownEventHandler != null;
                break;
        }

        if (IsToolCallInProgressEvent(evt))
        {
            _toolCallInProgressHandler?.Invoke(evt);
            handled = handled || _toolCallInProgressHandler != null;
        }
        else if (IsToolCallCompletedEvent(evt))
        {
            _toolCallCompletedHandler?.Invoke(evt);
            handled = handled || _toolCallCompletedHandler != null;
        }

        // Always call the any event handler if registered
        if (!handled || _anyEventHandler != null)
        {
            _anyEventHandler?.Invoke(evt);
        }
    }

    private static bool IsToolCallInProgressEvent(IResponseStreamEvent evt)
    {
        return evt is ResponseCodeInterpreterCallInProgressEvent
            or ResponseFileSearchCallInProgressEvent
            or ResponseImageGenCallInProgressEvent
            or ResponseMCPCallInProgressEvent
            or ResponseMCPListToolsInProgressEvent
            or ResponseWebSearchCallInProgressEvent;
    }

    private static bool IsToolCallCompletedEvent(IResponseStreamEvent evt)
    {
        return evt is ResponseCodeInterpreterCallCompletedEvent
            or ResponseFileSearchCallCompletedEvent
            or ResponseImageGenCallCompletedEvent
            or ResponseMCPCallCompletedEvent
            or ResponseMCPListToolsCompletedEvent
            or ResponseWebSearchCallCompletedEvent;
    }
}
