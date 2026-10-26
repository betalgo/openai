using System.Text.Json.Serialization;
using Betalgo.Ranul.OpenAI.Contracts.Enums.Responses;

namespace Betalgo.Ranul.OpenAI.Contracts.Types.Streaming.Events.Tools;

/// <summary>
///     Emitted when a partial image is available during image generation.
///     <see href="https://platform.openai.com/docs/api-reference/responses-streaming">OpenAI API documentation</see>.
///     <see href="https://github.com/betalgo/openai/blob/master/Docs/openapi-split/components/schemas/responseimagegencallpartialimageevent.yml">
///         Source Definition
///     </see>
/// </summary>
public sealed class ResponseImageGenCallPartialImageEvent : IResponseStreamEvent
{
    /// <summary>
    ///     Initializes a new instance for deserialization.
    /// </summary>
    public ResponseImageGenCallPartialImageEvent()
    {
    }

    /// <inheritdoc />
    [JsonPropertyName("type")]
    public string Type => ResponseStreamEventType.ImageGenCallPartialImage;

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

    /// <summary>
    ///     The index of this partial image within the generation sequence.
    /// </summary>
    [JsonPropertyName("partial_image_index")]
    public int PartialImageIndex { get; set; }

    /// <summary>
    ///     The base64-encoded partial image data.
    /// </summary>
    [JsonPropertyName("partial_image_b64")]
    public string PartialImageB64 { get; set; } = null!;
}
