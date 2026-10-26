using System.Text.Json.Serialization;
using Betalgo.Ranul.OpenAI.Contracts.Enums.Responses;
using Betalgo.Ranul.OpenAI.Contracts.Types.Content;

namespace Betalgo.Ranul.OpenAI.Contracts.Types.Streaming.Events.Output;

/// <summary>
///     Emitted when a new content part is added.
///     <see href="https://platform.openai.com/docs/api-reference/responses-streaming">OpenAI API documentation</see>.
///     <see href="https://github.com/betalgo/openai/blob/master/Docs/openapi-split/components/schemas/responsecontentpartaddedevent.yml">
///         Source Definition
///     </see>
/// </summary>
public sealed class ResponseContentPartAddedEvent : IResponseStreamEvent
{
    /// <summary>
    ///     Initializes a new instance for deserialization.
    /// </summary>
    public ResponseContentPartAddedEvent()
    {
    }

    /// <inheritdoc />
    [JsonPropertyName("type")]
    public string Type => ResponseStreamEventType.ContentPartAdded;

    /// <inheritdoc />
    [JsonPropertyName("sequence_number")]
    public int SequenceNumber { get; set; }

    /// <summary>
    ///     The ID of the output item that the content part was added to.
    /// </summary>
    [JsonPropertyName("item_id")]
    public string ItemId { get; set; } = null!;

    /// <summary>
    ///     The index of the output item that the content part was added to.
    /// </summary>
    [JsonPropertyName("output_index")]
    public int OutputIndex { get; set; }

    /// <summary>
    ///     The index of the content part that was added.
    /// </summary>
    [JsonPropertyName("content_index")]
    public int ContentIndex { get; set; }

    /// <summary>
    ///     The content part that was added.
    /// </summary>
    [JsonPropertyName("part")]
    public IContent Part { get; set; } = null!;
}
