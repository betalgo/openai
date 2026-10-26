using System.Text.Json.Serialization;
using Betalgo.Ranul.OpenAI.Contracts.Enums.Responses;
using Betalgo.Ranul.OpenAI.Contracts.Responses.Responses;

namespace Betalgo.Ranul.OpenAI.Contracts.Types.Streaming.Events.Lifecycle;

/// <summary>
///     Emitted when the model response is complete.
///     <see href="https://platform.openai.com/docs/api-reference/responses-streaming">OpenAI API documentation</see>.
///     <see href="https://github.com/betalgo/openai/blob/master/Docs/openapi-split/components/schemas/responsecompletedevent.yml">
///         Source Definition
///     </see>
/// </summary>
public sealed class ResponseCompletedEvent : IResponseStreamEvent
{
    /// <summary>
    ///     Initializes a new instance for deserialization.
    /// </summary>
    public ResponseCompletedEvent()
    {
    }

    /// <inheritdoc />
    [JsonPropertyName("type")]
    public string Type => ResponseStreamEventType.ResponseCompleted;

    /// <inheritdoc />
    [JsonPropertyName("sequence_number")]
    public int SequenceNumber { get; set; }

    /// <summary>
    ///     The response object.
    /// </summary>
    [JsonPropertyName("response")]
    public Response Response { get; set; } = null!;
}
