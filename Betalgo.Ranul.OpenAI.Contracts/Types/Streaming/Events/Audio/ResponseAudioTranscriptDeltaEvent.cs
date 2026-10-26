using System.Text.Json.Serialization;
using Betalgo.Ranul.OpenAI.Contracts.Enums.Responses;

namespace Betalgo.Ranul.OpenAI.Contracts.Types.Streaming.Events.Audio;

/// <summary>
///     Emitted when there is a delta of audio transcript content.
///     <see href="https://platform.openai.com/docs/api-reference/responses-streaming">OpenAI API documentation</see>.
///     <see href="https://github.com/betalgo/openai/blob/master/Docs/openapi-split/components/schemas/responseaudiotranscriptdeltaevent.yml">
///         Source Definition
///     </see>
/// </summary>
public sealed class ResponseAudioTranscriptDeltaEvent : IResponseStreamEvent
{
    /// <summary>
    ///     Initializes a new instance for deserialization.
    /// </summary>
    public ResponseAudioTranscriptDeltaEvent()
    {
    }

    /// <inheritdoc />
    [JsonPropertyName("type")]
    public string Type => ResponseStreamEventType.AudioTranscriptDelta;

    /// <inheritdoc />
    [JsonPropertyName("sequence_number")]
    public int SequenceNumber { get; set; }

    /// <summary>
    ///     The ID of the response that the audio transcript is associated with.
    /// </summary>
    [JsonPropertyName("response_id")]
    public string ResponseId { get; set; } = null!;

    /// <summary>
    ///     The transcript text delta.
    /// </summary>
    [JsonPropertyName("delta")]
    public string Delta { get; set; } = null!;
}
