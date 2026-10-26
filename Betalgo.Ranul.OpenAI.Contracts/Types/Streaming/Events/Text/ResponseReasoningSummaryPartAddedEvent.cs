using System.Text.Json.Serialization;
using Betalgo.Ranul.OpenAI.Contracts.Enums.Responses;

namespace Betalgo.Ranul.OpenAI.Contracts.Types.Streaming.Events.Text;

/// <summary>
///     Emitted when a new reasoning summary part is added.
///     <see href="https://platform.openai.com/docs/api-reference/responses-streaming">OpenAI API documentation</see>.
///     <see href="https://github.com/betalgo/openai/blob/master/Docs/openapi-split/components/schemas/responsereasoningsummarypartaddedevent.yml">
///         Source Definition
///     </see>
/// </summary>
public sealed class ResponseReasoningSummaryPartAddedEvent : IResponseStreamEvent
{
    /// <summary>
    ///     Initializes a new instance for deserialization.
    /// </summary>
    public ResponseReasoningSummaryPartAddedEvent()
    {
    }

    /// <inheritdoc />
    [JsonPropertyName("type")]
    public string Type => ResponseStreamEventType.ReasoningSummaryPartAdded;

    /// <inheritdoc />
    [JsonPropertyName("sequence_number")]
    public int SequenceNumber { get; set; }

    /// <summary>
    ///     The ID of the item this summary part is associated with.
    /// </summary>
    [JsonPropertyName("item_id")]
    public string ItemId { get; set; } = null!;

    /// <summary>
    ///     The index of the output item this summary part is associated with.
    /// </summary>
    [JsonPropertyName("output_index")]
    public int OutputIndex { get; set; }

    /// <summary>
    ///     The index of this summary part within the reasoning item.
    /// </summary>
    [JsonPropertyName("summary_index")]
    public int SummaryIndex { get; set; }

    /// <summary>
    ///     The summary part that was added.
    /// </summary>
    [JsonPropertyName("part")]
    public SummaryTextPart Part { get; set; } = null!;
}
