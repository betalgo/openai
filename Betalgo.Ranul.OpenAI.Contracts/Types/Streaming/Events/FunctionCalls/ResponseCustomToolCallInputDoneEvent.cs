using System.Text.Json.Serialization;
using Betalgo.Ranul.OpenAI.Contracts.Enums.Responses;

namespace Betalgo.Ranul.OpenAI.Contracts.Types.Streaming.Events.FunctionCalls;

/// <summary>
///     Emitted when custom tool call input is finalized.
///     <see href="https://platform.openai.com/docs/api-reference/responses-streaming">OpenAI API documentation</see>.
///     <see href="https://github.com/betalgo/openai/blob/master/Docs/openapi-split/components/schemas/responsecustomtoolcallinputdoneevent.yml">
///         Source Definition
///     </see>
/// </summary>
public sealed class ResponseCustomToolCallInputDoneEvent : IResponseStreamEvent
{
    /// <summary>
    ///     Initializes a new instance for deserialization.
    /// </summary>
    public ResponseCustomToolCallInputDoneEvent()
    {
    }

    /// <inheritdoc />
    [JsonPropertyName("type")]
    public string Type => ResponseStreamEventType.CustomToolCallInputDone;

    /// <inheritdoc />
    [JsonPropertyName("sequence_number")]
    public int SequenceNumber { get; set; }

    /// <summary>
    ///     The index of the output item that the custom tool call input belongs to.
    /// </summary>
    [JsonPropertyName("output_index")]
    public int OutputIndex { get; set; }

    /// <summary>
    ///     The ID of the output item that the custom tool call input belongs to.
    /// </summary>
    [JsonPropertyName("item_id")]
    public string ItemId { get; set; } = null!;

    /// <summary>
    ///     The finalized custom tool call input.
    /// </summary>
    [JsonPropertyName("input")]
    public string Input { get; set; } = null!;
}
