using System.Text.Json.Serialization;
using Betalgo.Ranul.OpenAI.Contracts.Enums.Responses;

namespace Betalgo.Ranul.OpenAI.Contracts.Types.Streaming.Events.Tools;

/// <summary>
///     Emitted when a code interpreter call is completed.
///     <see href="https://platform.openai.com/docs/api-reference/responses-streaming">OpenAI API documentation</see>.
///     <see href="https://github.com/betalgo/openai/blob/master/Docs/openapi-split/components/schemas/responsecodeinterpretercallcompletedevent.yml">
///         Source Definition
///     </see>
/// </summary>
public sealed class ResponseCodeInterpreterCallCompletedEvent : IResponseStreamEvent
{
    /// <summary>
    ///     Initializes a new instance for deserialization.
    /// </summary>
    public ResponseCodeInterpreterCallCompletedEvent()
    {
    }

    /// <inheritdoc />
    [JsonPropertyName("type")]
    public string Type => ResponseStreamEventType.CodeInterpreterCallCompleted;

    /// <inheritdoc />
    [JsonPropertyName("sequence_number")]
    public int SequenceNumber { get; set; }

    /// <summary>
    ///     The index of the output item.
    /// </summary>
    [JsonPropertyName("output_index")]
    public int OutputIndex { get; set; }

    /// <summary>
    ///     The unique identifier of the code interpreter tool call item.
    /// </summary>
    [JsonPropertyName("item_id")]
    public string ItemId { get; set; } = null!;
}
