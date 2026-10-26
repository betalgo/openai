using System.Text.Json.Serialization;
using Betalgo.Ranul.OpenAI.Contracts.Enums.Responses;

namespace Betalgo.Ranul.OpenAI.Contracts.Types.Streaming.Events.Text;

/// <summary>
///     Emitted when a delta is added to a reasoning text.
///     <see href="https://platform.openai.com/docs/api-reference/responses-streaming">OpenAI API documentation</see>.
///     <see href="https://github.com/betalgo/openai/blob/master/Docs/openapi-split/components/schemas/responsereasoningtextdeltaevent.yml">
///         Source Definition
///     </see>
/// </summary>
public sealed class ResponseReasoningTextDeltaEvent : IResponseStreamEvent
{
    /// <summary>
    ///     Initializes a new instance for deserialization.
    /// </summary>
    public ResponseReasoningTextDeltaEvent()
    {
    }

    /// <inheritdoc />
    [JsonPropertyName("type")]
    public string Type => ResponseStreamEventType.ReasoningTextDelta;

    /// <inheritdoc />
    [JsonPropertyName("sequence_number")]
    public int SequenceNumber { get; set; }

    /// <summary>
    ///     The ID of the item this reasoning text delta is associated with.
    /// </summary>
    [JsonPropertyName("item_id")]
    public string ItemId { get; set; } = null!;

    /// <summary>
    ///     The index of the output item this reasoning text delta is associated with.
    /// </summary>
    [JsonPropertyName("output_index")]
    public int OutputIndex { get; set; }

    /// <summary>
    ///     The index of the reasoning content part this delta is associated with.
    /// </summary>
    [JsonPropertyName("content_index")]
    public int ContentIndex { get; set; }

    /// <summary>
    ///     The text delta that was added to the reasoning content.
    /// </summary>
    [JsonPropertyName("delta")]
    public string Delta { get; set; } = null!;
}
