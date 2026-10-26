using System.Text.Json.Serialization;
using Betalgo.Ranul.OpenAI.Contracts.Enums.Responses;

namespace Betalgo.Ranul.OpenAI.Contracts.Types.Streaming.Events.Tools;

/// <summary>
///     Emitted when an image generation call is completed.
///     <see href="https://platform.openai.com/docs/api-reference/responses-streaming">OpenAI API documentation</see>.
///     <see href="https://github.com/betalgo/openai/blob/master/Docs/openapi-split/components/schemas/responseimagegencallcompletedevent.yml">
///         Source Definition
///     </see>
/// </summary>
public sealed class ResponseImageGenCallCompletedEvent : IResponseStreamEvent
{
    /// <summary>
    ///     Initializes a new instance for deserialization.
    /// </summary>
    public ResponseImageGenCallCompletedEvent()
    {
    }

    /// <inheritdoc />
    [JsonPropertyName("type")]
    public string Type => ResponseStreamEventType.ImageGenCallCompleted;

    /// <inheritdoc />
    [JsonPropertyName("sequence_number")]
    public int SequenceNumber { get; set; }

    /// <summary>
    ///     The index of the output item.
    /// </summary>
    [JsonPropertyName("output_index")]
    public int OutputIndex { get; set; }

    /// <summary>
    ///     The unique identifier of the image generation tool call item.
    /// </summary>
    [JsonPropertyName("item_id")]
    public string ItemId { get; set; } = null!;
}
