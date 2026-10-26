using System.Text.Json.Serialization;
using Betalgo.Ranul.OpenAI.Contracts.Enums.Responses;

namespace Betalgo.Ranul.OpenAI.Contracts.Types.Streaming.Events.Text;

/// <summary>
///     Emitted when there is partial refusal text.
///     <see href="https://platform.openai.com/docs/api-reference/responses-streaming">OpenAI API documentation</see>.
///     <see href="https://github.com/betalgo/openai/blob/master/Docs/openapi-split/components/schemas/responserefusaldeltaevent.yml">
///         Source Definition
///     </see>
/// </summary>
public sealed class ResponseRefusalDeltaEvent : IResponseStreamEvent
{
    /// <summary>
    ///     Initializes a new instance for deserialization.
    /// </summary>
    public ResponseRefusalDeltaEvent()
    {
    }

    /// <inheritdoc />
    [JsonPropertyName("type")]
    public string Type => ResponseStreamEventType.RefusalDelta;

    /// <inheritdoc />
    [JsonPropertyName("sequence_number")]
    public int SequenceNumber { get; set; }

    /// <summary>
    ///     The ID of the output item that the refusal text is added to.
    /// </summary>
    [JsonPropertyName("item_id")]
    public string ItemId { get; set; } = null!;

    /// <summary>
    ///     The index of the output item that the refusal text is added to.
    /// </summary>
    [JsonPropertyName("output_index")]
    public int OutputIndex { get; set; }

    /// <summary>
    ///     The index of the content part that the refusal text is added to.
    /// </summary>
    [JsonPropertyName("content_index")]
    public int ContentIndex { get; set; }

    /// <summary>
    ///     The refusal text that is added.
    /// </summary>
    [JsonPropertyName("delta")]
    public string Delta { get; set; } = null!;
}
