using System.Text.Json;
using System.Text.Json.Serialization;
using Betalgo.Ranul.OpenAI.Contracts.Enums.Responses;

namespace Betalgo.Ranul.OpenAI.Contracts.Types.Streaming.Events.Text;

/// <summary>
///     Emitted when an annotation is added to output text content.
///     <see href="https://platform.openai.com/docs/api-reference/responses-streaming">OpenAI API documentation</see>.
///     <see href="https://github.com/betalgo/openai/blob/master/Docs/openapi-split/components/schemas/responseoutputtextannotationaddedevent.yml">
///         Source Definition
///     </see>
/// </summary>
public sealed class ResponseOutputTextAnnotationAddedEvent : IResponseStreamEvent
{
    /// <summary>
    ///     Initializes a new instance for deserialization.
    /// </summary>
    public ResponseOutputTextAnnotationAddedEvent()
    {
    }

    /// <inheritdoc />
    [JsonPropertyName("type")]
    public string Type => ResponseStreamEventType.OutputTextAnnotationAdded;

    /// <inheritdoc />
    [JsonPropertyName("sequence_number")]
    public int SequenceNumber { get; set; }

    /// <summary>
    ///     The unique identifier of the item to which the annotation is being added.
    /// </summary>
    [JsonPropertyName("item_id")]
    public string ItemId { get; set; } = null!;

    /// <summary>
    ///     The index of the output item in the response's output array.
    /// </summary>
    [JsonPropertyName("output_index")]
    public int OutputIndex { get; set; }

    /// <summary>
    ///     The index of the content part within the output item.
    /// </summary>
    [JsonPropertyName("content_index")]
    public int ContentIndex { get; set; }

    /// <summary>
    ///     The index of the annotation within the content part.
    /// </summary>
    [JsonPropertyName("annotation_index")]
    public int AnnotationIndex { get; set; }

    /// <summary>
    ///     The annotation object being added.
    /// </summary>
    [JsonPropertyName("annotation")]
    public JsonElement Annotation { get; set; }
}
