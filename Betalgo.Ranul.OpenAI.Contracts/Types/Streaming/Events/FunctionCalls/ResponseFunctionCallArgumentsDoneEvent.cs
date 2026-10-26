using System.Text.Json.Serialization;
using Betalgo.Ranul.OpenAI.Contracts.Enums.Responses;

namespace Betalgo.Ranul.OpenAI.Contracts.Types.Streaming.Events.FunctionCalls;

/// <summary>
///     Emitted when function call arguments are finalized.
///     <see href="https://platform.openai.com/docs/api-reference/responses-streaming">OpenAI API documentation</see>.
///     <see href="https://github.com/betalgo/openai/blob/master/Docs/openapi-split/components/schemas/responsefunctioncallargumentsdoneevent.yml">
///         Source Definition
///     </see>
/// </summary>
public sealed class ResponseFunctionCallArgumentsDoneEvent : IResponseStreamEvent
{
    /// <summary>
    ///     Initializes a new instance for deserialization.
    /// </summary>
    public ResponseFunctionCallArgumentsDoneEvent()
    {
    }

    /// <inheritdoc />
    [JsonPropertyName("type")]
    public string Type => ResponseStreamEventType.FunctionCallArgumentsDone;

    /// <inheritdoc />
    [JsonPropertyName("sequence_number")]
    public int SequenceNumber { get; set; }

    /// <summary>
    ///     The ID of the output item that the function call arguments belong to.
    /// </summary>
    [JsonPropertyName("item_id")]
    public string ItemId { get; set; } = null!;

    /// <summary>
    ///     The index of the output item that the function call arguments belong to.
    /// </summary>
    [JsonPropertyName("output_index")]
    public int OutputIndex { get; set; }

    /// <summary>
    ///     The name of the function that was called.
    /// </summary>
    [JsonPropertyName("name")]
    public string Name { get; set; } = null!;

    /// <summary>
    ///     The finalized function call arguments (JSON string).
    /// </summary>
    [JsonPropertyName("arguments")]
    public string Arguments { get; set; } = null!;
}
