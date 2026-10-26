using System.Text.Json.Serialization;
using Betalgo.Ranul.OpenAI.Contracts.Enums.Responses;

namespace Betalgo.Ranul.OpenAI.Contracts.Types.Streaming.Events.Text;

/// <summary>
///     Emitted when a delta is added to a reasoning summary text.
///     <see href="https://platform.openai.com/docs/api-reference/responses-streaming">OpenAI API documentation</see>.
///     <see href="https://github.com/betalgo/openai/blob/master/Docs/openapi-split/components/schemas/responsereasoningsummarytextdeltaevent.yml">
///         Source Definition
///     </see>
/// </summary>
public sealed class ResponseReasoningSummaryTextDeltaEvent : IResponseStreamEvent
{
    /// <summary>
    ///     Initializes a new instance for deserialization.
    /// </summary>
    public ResponseReasoningSummaryTextDeltaEvent()
    {
    }

    /// <inheritdoc />
    [JsonPropertyName("type")]
    public string Type => ResponseStreamEventType.ReasoningSummaryTextDelta;

    /// <inheritdoc />
    [JsonPropertyName("sequence_number")]
    public int SequenceNumber { get; set; }

    /// <summary>
    ///     The ID of the item this reasoning summary delta is associated with.
    /// </summary>
    [JsonPropertyName("item_id")]
    public string ItemId { get; set; } = null!;

    /// <summary>
    ///     The index of the output item this reasoning summary delta is associated with.
    /// </summary>
    [JsonPropertyName("output_index")]
    public int OutputIndex { get; set; }

    /// <summary>
    ///     The index of the summary part within the reasoning item.
    /// </summary>
    [JsonPropertyName("summary_index")]
    public int SummaryIndex { get; set; }

    /// <summary>
    ///     The text delta that was added to the summary.
    /// </summary>
    [JsonPropertyName("delta")]
    public string Delta { get; set; } = null!;
}
