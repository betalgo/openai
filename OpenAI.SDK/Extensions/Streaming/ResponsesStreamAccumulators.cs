using System.Runtime.CompilerServices;
using System.Text;
using Betalgo.Ranul.OpenAI.Contracts.Enums.Responses;
using Betalgo.Ranul.OpenAI.Contracts.Responses.Responses;
using Betalgo.Ranul.OpenAI.Contracts.Types.Streaming;
using Betalgo.Ranul.OpenAI.Contracts.Types.Streaming.Events.Lifecycle;
using Betalgo.Ranul.OpenAI.Contracts.Types.Streaming.Events.Text;

namespace Betalgo.Ranul.OpenAI.Extensions.Streaming;

/// <summary>
///     Extension methods for accumulating data from Responses API streaming events.
/// </summary>
public static class ResponsesStreamAccumulators
{
    /// <summary>
    ///     Accumulates all text deltas and returns text only after a successful completed response.
    /// </summary>
    /// <param name="events">The stream of events.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The accumulated text from all text delta events.</returns>
    /// <example>
    ///     <code>
    /// var text = await openAi.Responses.CreateAsStreamAsync(request).GetTextAsync();
    /// Console.WriteLine(text);
    /// </code>
    /// </example>
    /// <exception cref="ResponseStreamException">The response failed, was incomplete, or emitted an error.</exception>
    /// <exception cref="EndOfStreamException">The stream ended without successful completion.</exception>
    public static async Task<string> GetTextAsync(
        this IAsyncEnumerable<IResponseStreamEvent> events,
        CancellationToken cancellationToken = default)
    {
        var sb = new StringBuilder();
        var completed = false;

        await foreach (var evt in events.WithCancellation(cancellationToken))
        {
            completed |= ValidateTextCompletion(evt);
            if (evt is ResponseTextDeltaEvent textDelta)
            {
                sb.Append(textDelta.Delta);
            }
        }

        RequireCompletion(completed);
        return sb.ToString();
    }

    /// <summary>
    ///     Gets the final Response object when the stream completes.
    /// </summary>
    /// <param name="events">The stream of events.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The terminal Response object (completed, failed or incomplete), or null if not found.</returns>
    /// <example>
    ///     <code>
    /// var response = await openAi.Responses.CreateAsStreamAsync(request).GetFinalResponseAsync();
    /// Console.WriteLine($"Tokens used: {response?.Usage?.TotalTokens}");
    /// </code>
    /// </example>
    public static async Task<Response?> GetFinalResponseAsync(
        this IAsyncEnumerable<IResponseStreamEvent> events,
        CancellationToken cancellationToken = default)
    {
        Response? finalResponse = null;

        await foreach (var evt in events.WithCancellation(cancellationToken))
        {
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

    /// <summary>
    ///     Accumulates text while also yielding events for custom handling.
    ///     Useful when you want both the accumulated text AND to process individual events.
    /// </summary>
    /// <param name="events">The stream of events.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>An async enumerable of tuples containing the event and accumulated text so far.</returns>
    /// <example>
    ///     <code>
    /// await foreach (var (evt, text) in stream.WithTextAccumulation())
    /// {
    ///     Console.WriteLine($"Current text length: {text.Length}");
    ///     if (evt is ResponseCompletedEvent) Console.WriteLine($"Final: {text}");
    /// }
    /// </code>
    /// </example>
    public static async IAsyncEnumerable<(IResponseStreamEvent Event, string AccumulatedText)> WithTextAccumulation(
        this IAsyncEnumerable<IResponseStreamEvent> events,
        [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        var sb = new StringBuilder();

        await foreach (var evt in events.WithCancellation(cancellationToken))
        {
            if (evt is ResponseTextDeltaEvent textDelta)
            {
                sb.Append(textDelta.Delta);
            }

            yield return (evt, sb.ToString());
        }
    }

    /// <summary>
    ///     Accumulates reasoning text while also yielding events for custom handling.
    ///     Useful for models that emit reasoning content (o1/o3 models).
    /// </summary>
    /// <param name="events">The stream of events.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>An async enumerable of tuples containing the event and accumulated reasoning text so far.</returns>
    public static async IAsyncEnumerable<(IResponseStreamEvent Event, string AccumulatedReasoning)> WithReasoningAccumulation(
        this IAsyncEnumerable<IResponseStreamEvent> events,
        [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        var sb = new StringBuilder();

        await foreach (var evt in events.WithCancellation(cancellationToken))
        {
            if (evt is ResponseReasoningTextDeltaEvent reasoningDelta)
            {
                sb.Append(reasoningDelta.Delta);
            }

            yield return (evt, sb.ToString());
        }
    }

    /// <summary>
    ///     Gets text from text done events only after a successful completed response.
    /// </summary>
    /// <param name="events">The stream of events.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The final accumulated text from TextDone events.</returns>
    /// <exception cref="ResponseStreamException">The response failed, was incomplete, or emitted an error.</exception>
    /// <exception cref="EndOfStreamException">The stream ended without successful completion.</exception>
    public static async Task<string> GetFinalTextAsync(
        this IAsyncEnumerable<IResponseStreamEvent> events,
        CancellationToken cancellationToken = default)
    {
        var sb = new StringBuilder();
        var completed = false;

        await foreach (var evt in events.WithCancellation(cancellationToken))
        {
            completed |= ValidateTextCompletion(evt);
            if (evt is ResponseTextDoneEvent textDone)
            {
                sb.Append(textDone.Text);
            }
        }

        RequireCompletion(completed);
        return sb.ToString();
    }
    private static bool ValidateTextCompletion(IResponseStreamEvent evt)
    {
        switch (evt)
        {
            case ResponseFailedEvent failed:
                throw new ResponseStreamException("Response generation failed.", failed.Response);
            case ResponseIncompleteEvent incomplete:
                throw new ResponseStreamException("Response generation is incomplete.", incomplete.Response);
            case ResponseErrorEvent error:
                throw new ResponseStreamException(error.Message ?? "Response streaming failed.", errorEvent: error);
            case ResponseCompletedEvent completed:
                if (completed.Response == null || completed.Response.Error != null ||
                    completed.Response.Status != ResponsesStatus.Completed)
                    throw new ResponseStreamException("The terminal response did not complete successfully.", completed.Response);
                return true;
            default:
                return false;
        }
    }

    private static void RequireCompletion(bool completed)
    {
        if (!completed)
            throw new EndOfStreamException("The response stream ended without a completed response. Text may be incomplete.");
    }

}
