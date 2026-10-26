using System.Runtime.CompilerServices;
using System.Text.Json;
using Betalgo.Ranul.OpenAI.Contracts.Interfaces;

namespace Betalgo.Ranul.OpenAI.Extensions.Streaming;

/// <summary>
///     Core SSE stream parser. Works with any IStreamEvent implementation.
///     <see href="https://platform.openai.com/docs/api-reference/streaming">OpenAI Streaming documentation</see>.
/// </summary>
public static class StreamParser
{
    /// <summary>
    ///     Parses Server-Sent Events from an HTTP response into strongly-typed events.
    /// </summary>
    /// <typeparam name="TEvent">The type of event to deserialize to.</typeparam>
    /// <param name="response">The HTTP response containing the SSE stream.</param>
    /// <param name="options">Optional JSON serializer options.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>An async enumerable of parsed events.</returns>
    public static async IAsyncEnumerable<TEvent> ParseSseStreamAsync<TEvent>(
        this HttpResponseMessage response,
        JsonSerializerOptions? options = null,
        [EnumeratorCancellation] CancellationToken cancellationToken = default)
        where TEvent : IStreamEvent
    {
#if NET6_0_OR_GREATER
        await using var stream = await response.Content.ReadAsStreamAsync(cancellationToken);
#else
        using var stream = await response.Content.ReadAsStreamAsync();
#endif
#if !NET7_0_OR_GREATER
        // Older StreamReader APIs cannot cancel a pending read. Disposing the owned
        // response stream interrupts it instead of leaving an abandoned read running.
        using var cancellationRegistration = cancellationToken.Register(() => stream.Dispose());
#endif
        using var reader = new StreamReader(stream);
        var dataLines = new List<string>();

        while (true)
        {
            cancellationToken.ThrowIfCancellationRequested();

#if NET7_0_OR_GREATER
            var line = await reader.ReadLineAsync(cancellationToken);
#else
            string? line;
            try
            {
                line = await reader.ReadLineAsync();
            }
            catch (Exception exception) when (cancellationToken.IsCancellationRequested &&
                                              (exception is IOException || exception is ObjectDisposedException || exception is OperationCanceledException))
            {
                throw new OperationCanceledException("The event stream read was canceled.", exception, cancellationToken);
            }
            cancellationToken.ThrowIfCancellationRequested();
#endif

            if (line == null)
            {
                if (TryDeserializeEventBlock<TEvent>(dataLines, options, out var evt, out var shouldStop) && evt != null)
                {
                    yield return evt;
                }

                if (shouldStop)
                {
                    yield break;
                }

                yield break;
            }

            if (string.IsNullOrEmpty(line))
            {
                if (TryDeserializeEventBlock<TEvent>(dataLines, options, out var evt, out var shouldStop) && evt != null)
                {
                    yield return evt;
                }

                if (shouldStop)
                {
                    yield break;
                }

                continue;
            }

            if (line.StartsWith(":", StringComparison.Ordinal))
            {
                continue;
            }

            if (line.StartsWith("event:", StringComparison.OrdinalIgnoreCase))
            {
                continue;
            }

            if (line.StartsWith("data:", StringComparison.OrdinalIgnoreCase))
            {
                dataLines.Add(line.Substring(5).TrimStart());
            }
        }
    }

    /// <summary>
    ///     Parses Server-Sent Events from an HTTP response, passing through HTTP metadata.
    /// </summary>
    /// <typeparam name="TEvent">The type of event to deserialize to.</typeparam>
    /// <param name="response">The HTTP response containing the SSE stream.</param>
    /// <param name="options">Optional JSON serializer options.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>An async enumerable of parsed events with HTTP status code.</returns>
    public static async IAsyncEnumerable<(TEvent Event, System.Net.HttpStatusCode StatusCode)> ParseSseStreamWithStatusAsync<TEvent>(
        this HttpResponseMessage response,
        JsonSerializerOptions? options = null,
        [EnumeratorCancellation] CancellationToken cancellationToken = default)
        where TEvent : IStreamEvent
    {
        var statusCode = response.StatusCode;

        await foreach (var evt in response.ParseSseStreamAsync<TEvent>(options, cancellationToken))
        {
            yield return (evt, statusCode);
        }
    }

    private static bool TryDeserializeEventBlock<TEvent>(List<string> dataLines, JsonSerializerOptions? options, out TEvent? evt, out bool shouldStop)
        where TEvent : IStreamEvent
    {
        evt = default;
        shouldStop = false;

        if (dataLines.Count == 0)
        {
            return false;
        }

        var data = string.Join("\n", dataLines);
        dataLines.Clear();

        if (data == "[DONE]")
        {
            shouldStop = true;
            return false;
        }

        // Malformed payloads must surface to the caller: silently skipping one
        // can turn a corrupted response into apparently successful partial output.
        evt = JsonSerializer.Deserialize<TEvent>(data, options);

        if (evt is null)
        {
            throw new JsonException("An event payload must contain an event object, not null.");
        }
        if (string.IsNullOrWhiteSpace(evt.Type))
        {
            throw new JsonException("An event payload must contain a nonempty type discriminator.");
        }
        return true;
    }
}
