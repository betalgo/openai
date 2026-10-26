using System.Runtime.CompilerServices;
using Betalgo.Ranul.OpenAI.Contracts.Types.Streaming;
using Betalgo.Ranul.OpenAI.Contracts.Types.Streaming.Events;
using Betalgo.Ranul.OpenAI.Contracts.Types.Streaming.Events.Audio;
using Betalgo.Ranul.OpenAI.Contracts.Types.Streaming.Events.FunctionCalls;
using Betalgo.Ranul.OpenAI.Contracts.Types.Streaming.Events.Lifecycle;
using Betalgo.Ranul.OpenAI.Contracts.Types.Streaming.Events.Text;
using Betalgo.Ranul.OpenAI.Contracts.Types.Streaming.Events.Tools;

namespace Betalgo.Ranul.OpenAI.Extensions.Streaming;

/// <summary>
///     Extension methods for filtering Responses API streaming events.
/// </summary>
public static class ResponsesStreamFilters
{
    /// <summary>
    ///     Filters to only text-related events (delta and done).
    /// </summary>
    /// <param name="events">The stream of events.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>An async enumerable containing only text events.</returns>
    /// <example>
    ///     <code>
    /// await foreach (var evt in stream.TextEventsOnly())
    /// {
    ///     // evt is guaranteed to be a text-related event
    /// }
    /// </code>
    /// </example>
    public static async IAsyncEnumerable<IResponseStreamEvent> TextEventsOnly(
        this IAsyncEnumerable<IResponseStreamEvent> events,
        [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        await foreach (var evt in events.WithCancellation(cancellationToken))
        {
            if (evt is ResponseTextDeltaEvent or ResponseTextDoneEvent or ResponseOutputTextAnnotationAddedEvent)
            {
                yield return evt;
            }
        }
    }

    /// <summary>
    ///     Filters to only function call events.
    /// </summary>
    /// <param name="events">The stream of events.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>An async enumerable containing only function call events.</returns>
    public static async IAsyncEnumerable<IResponseStreamEvent> FunctionCallEventsOnly(
        this IAsyncEnumerable<IResponseStreamEvent> events,
        [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        await foreach (var evt in events.WithCancellation(cancellationToken))
        {
            if (evt is ResponseFunctionCallArgumentsDeltaEvent or ResponseFunctionCallArgumentsDoneEvent
                or ResponseCustomToolCallInputDeltaEvent or ResponseCustomToolCallInputDoneEvent)
            {
                yield return evt;
            }
        }
    }

    /// <summary>
    ///     Filters to only tool call events (all built-in tools: code interpreter, file search, web search, etc.).
    /// </summary>
    /// <param name="events">The stream of events.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>An async enumerable containing only tool call events.</returns>
    public static async IAsyncEnumerable<IResponseStreamEvent> ToolCallEventsOnly(
        this IAsyncEnumerable<IResponseStreamEvent> events,
        [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        await foreach (var evt in events.WithCancellation(cancellationToken))
        {
            if (evt is ResponseCodeInterpreterCallInProgressEvent or ResponseCodeInterpreterCallInterpretingEvent
                or ResponseCodeInterpreterCallCodeDeltaEvent or ResponseCodeInterpreterCallCodeDoneEvent
                or ResponseCodeInterpreterCallCompletedEvent
                or ResponseFileSearchCallInProgressEvent or ResponseFileSearchCallSearchingEvent
                or ResponseFileSearchCallCompletedEvent
                or ResponseWebSearchCallInProgressEvent or ResponseWebSearchCallSearchingEvent
                or ResponseWebSearchCallCompletedEvent
                or ResponseImageGenCallInProgressEvent or ResponseImageGenCallGeneratingEvent
                or ResponseImageGenCallPartialImageEvent or ResponseImageGenCallCompletedEvent
                or ResponseMCPCallArgumentsDeltaEvent or ResponseMCPCallArgumentsDoneEvent
                or ResponseMCPCallInProgressEvent or ResponseMCPCallCompletedEvent or ResponseMCPCallFailedEvent
                or ResponseMCPListToolsInProgressEvent or ResponseMCPListToolsCompletedEvent
                or ResponseMCPListToolsFailedEvent)
            {
                yield return evt;
            }
        }
    }

    /// <summary>
    ///     Filters to only reasoning events (for o1/o3 models).
    /// </summary>
    /// <param name="events">The stream of events.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>An async enumerable containing only reasoning events.</returns>
    public static async IAsyncEnumerable<IResponseStreamEvent> ReasoningEventsOnly(
        this IAsyncEnumerable<IResponseStreamEvent> events,
        [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        await foreach (var evt in events.WithCancellation(cancellationToken))
        {
            if (evt is ResponseReasoningTextDeltaEvent or ResponseReasoningTextDoneEvent
                or ResponseReasoningSummaryTextDeltaEvent or ResponseReasoningSummaryTextDoneEvent
                or ResponseReasoningSummaryPartAddedEvent or ResponseReasoningSummaryPartDoneEvent)
            {
                yield return evt;
            }
        }
    }

    /// <summary>
    ///     Filters to only lifecycle events (created, queued, in_progress, completed, failed, incomplete, error).
    /// </summary>
    /// <param name="events">The stream of events.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>An async enumerable containing only lifecycle events.</returns>
    public static async IAsyncEnumerable<IResponseStreamEvent> LifecycleEventsOnly(
        this IAsyncEnumerable<IResponseStreamEvent> events,
        [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        await foreach (var evt in events.WithCancellation(cancellationToken))
        {
            if (evt is ResponseCreatedEvent or ResponseQueuedEvent or ResponseInProgressEvent
                or ResponseCompletedEvent or ResponseFailedEvent or ResponseIncompleteEvent
                or ResponseErrorEvent)
            {
                yield return evt;
            }
        }
    }

    /// <summary>
    ///     Filters to only audio events.
    /// </summary>
    /// <param name="events">The stream of events.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>An async enumerable containing only audio events.</returns>
    public static async IAsyncEnumerable<IResponseStreamEvent> AudioEventsOnly(
        this IAsyncEnumerable<IResponseStreamEvent> events,
        [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        await foreach (var evt in events.WithCancellation(cancellationToken))
        {
            if (evt is ResponseAudioDeltaEvent or ResponseAudioDoneEvent
                or ResponseAudioTranscriptDeltaEvent or ResponseAudioTranscriptDoneEvent)
            {
                yield return evt;
            }
        }
    }

    /// <summary>
    ///     Filters to events matching a predicate.
    /// </summary>
    /// <param name="events">The stream of events.</param>
    /// <param name="predicate">The predicate to filter by.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>An async enumerable containing only events matching the predicate.</returns>
    public static async IAsyncEnumerable<IResponseStreamEvent> Where(
        this IAsyncEnumerable<IResponseStreamEvent> events,
        Func<IResponseStreamEvent, bool> predicate,
        [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        await foreach (var evt in events.WithCancellation(cancellationToken))
        {
            if (predicate(evt))
            {
                yield return evt;
            }
        }
    }

    /// <summary>
    ///     Filters to events of a specific type.
    /// </summary>
    /// <typeparam name="TEvent">The specific event type to filter for.</typeparam>
    /// <param name="events">The stream of events.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>An async enumerable containing only events of the specified type.</returns>
    /// <example>
    ///     <code>
    /// await foreach (var delta in stream.OfEventType&lt;ResponseTextDeltaEvent&gt;())
    /// {
    ///     Console.Write(delta.Delta); // Strongly typed!
    /// }
    /// </code>
    /// </example>
    public static async IAsyncEnumerable<TEvent> OfEventType<TEvent>(
        this IAsyncEnumerable<IResponseStreamEvent> events,
        [EnumeratorCancellation] CancellationToken cancellationToken = default)
        where TEvent : IResponseStreamEvent
    {
        await foreach (var evt in events.WithCancellation(cancellationToken))
        {
            if (evt is TEvent typedEvent)
            {
                yield return typedEvent;
            }
        }
    }

    /// <summary>
    ///     Filters to only delta events (events containing incremental data).
    /// </summary>
    /// <param name="events">The stream of events.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>An async enumerable containing only delta events.</returns>
    public static async IAsyncEnumerable<IResponseStreamEvent> DeltaEventsOnly(
        this IAsyncEnumerable<IResponseStreamEvent> events,
        [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        await foreach (var evt in events.WithCancellation(cancellationToken))
        {
            if (evt.Type.EndsWith(".delta"))
            {
                yield return evt;
            }
        }
    }

    /// <summary>
    ///     Filters to only done events (events indicating completion of a part).
    /// </summary>
    /// <param name="events">The stream of events.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>An async enumerable containing only done events.</returns>
    public static async IAsyncEnumerable<IResponseStreamEvent> DoneEventsOnly(
        this IAsyncEnumerable<IResponseStreamEvent> events,
        [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        await foreach (var evt in events.WithCancellation(cancellationToken))
        {
            if (evt.Type.EndsWith(".done"))
            {
                yield return evt;
            }
        }
    }

    /// <summary>
    ///     Filters to only unknown/unrecognized events.
    ///     Useful for monitoring new event types that the SDK doesn't yet support.
    /// </summary>
    /// <param name="events">The stream of events.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>An async enumerable containing only unknown events.</returns>
    /// <example>
    ///     <code>
    /// await foreach (var unknown in stream.UnknownEventsOnly())
    /// {
    ///     _logger.LogWarning("Unknown event type: {Type}", unknown.Type);
    ///     // Access raw JSON: unknown.RawJson
    /// }
    /// </code>
    /// </example>
    public static async IAsyncEnumerable<UnknownResponseStreamEvent> UnknownEventsOnly(
        this IAsyncEnumerable<IResponseStreamEvent> events,
        [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        await foreach (var evt in events.WithCancellation(cancellationToken))
        {
            if (evt is UnknownResponseStreamEvent unknown)
            {
                yield return unknown;
            }
        }
    }
}
