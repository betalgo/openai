using System.Text.Json.Serialization;

namespace Betalgo.Ranul.OpenAI.Contracts.Types.Streaming.Events;

/// <summary>
///     Represents an unknown or unrecognized streaming event from the Responses API.
///     This provides forward compatibility - when OpenAI introduces new event types,
///     existing code continues to work instead of throwing exceptions.
/// </summary>
/// <remarks>
///     Developers can:
///     <list type="bullet">
///         <item>Check for unknown events using pattern matching: <c>case UnknownResponseStreamEvent unknown:</c></item>
///         <item>Access the raw JSON via <see cref="RawJson" /> to handle new events before SDK updates</item>
///         <item>Log unknown event types for debugging and SDK update requests</item>
///     </list>
/// </remarks>
public class UnknownResponseStreamEvent : IResponseStreamEvent
{
    /// <summary>
    ///     Creates a new unknown event with the specified type and raw JSON.
    /// </summary>
    /// <param name="type">The event type string from the API.</param>
    /// <param name="sequenceNumber">The sequence number of the event.</param>
    /// <param name="rawJson">The complete raw JSON of the event.</param>
    public UnknownResponseStreamEvent(string type, int sequenceNumber, string rawJson)
    {
        Type = type;
        SequenceNumber = sequenceNumber;
        RawJson = rawJson;
    }

    /// <inheritdoc />
    [JsonPropertyName("type")]
    public string Type { get; set; }

    /// <inheritdoc />
    [JsonPropertyName("sequence_number")]
    public int SequenceNumber { get; set; }

    /// <summary>
    ///     The complete raw JSON of the unknown event.
    ///     Use this to manually deserialize or inspect new event types
    ///     that the SDK doesn't yet support.
    /// </summary>
    public string RawJson { get; }
}
