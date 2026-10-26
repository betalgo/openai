using System.Text.Json.Serialization;

namespace Betalgo.Ranul.OpenAI.Contracts.Interfaces;

/// <summary>
///     Base interface for all streaming events across OpenAI APIs.
///     Implement this interface for any API that supports Server-Sent Events streaming.
///     <see href="https://platform.openai.com/docs/api-reference/streaming">OpenAI Streaming documentation</see>.
/// </summary>
public interface IStreamEvent
{
    /// <summary>
    ///     The event type discriminator (e.g., "response.output_text.delta").
    ///     Used for polymorphic deserialization and pattern matching.
    /// </summary>
    [JsonPropertyName("type")]
    string Type { get; }

    /// <summary>
    ///     Sequence number for ordering events within a stream.
    ///     Events should be processed in sequence_number order.
    /// </summary>
    [JsonPropertyName("sequence_number")]
    int SequenceNumber { get; }
}
