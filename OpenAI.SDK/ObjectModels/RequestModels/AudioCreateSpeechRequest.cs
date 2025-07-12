using System.Text.Json.Serialization;
using Betalgo.Ranul.OpenAI.Contracts.Enums;
using Betalgo.Ranul.OpenAI.ObjectModels.SharedModels;

namespace Betalgo.Ranul.OpenAI.ObjectModels.RequestModels;

public record AudioCreateSpeechRequest : IOpenAIModels.IModel
{
    /// <summary>
    ///     The text to generate audio for. The maximum length is 4096 characters.
    /// </summary>
    [JsonPropertyName("input")]
    public string Input { get; set; }

    /// <summary>
    ///     The voice to use when generating the audio. Supported voices are alloy, echo, fable, onyx, nova, and shimmer
    /// </summary>
    [JsonPropertyName("voice")]
    public VoiceEnum Voice { get; set; }

    /// <summary>
    ///     The format to audio in. Supported formats are mp3, opus, aac, and flac
    ///     Defaults to mp3
    /// </summary>
    [JsonPropertyName("response_format")]
    public CreateSpeechResponseFormatEnum? ResponseFormat { get; set; }

    /// <summary>
    ///     The speed of the generated audio. Select a value from 0.25 to 4.0.
    ///     Defaults to 1.0
    /// </summary>
    [JsonPropertyName("speed")]
    public float? Speed { get; set; }

    /// <summary>
    ///     ID of the model to use. One of the available TTS models: tts-1 or tts-1-hd
    /// </summary>
    [JsonPropertyName("model")]
    public string Model { get; set; }
}