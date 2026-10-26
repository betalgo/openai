using System.Text.Json.Serialization;

namespace Betalgo.Ranul.OpenAI.Contracts.Types.Content;

/// <summary>
///     An audio input to the model.
///     <see href="https://platform.openai.com/docs/api-reference/responses">OpenAI API documentation</see>.
///     <see href="https://github.com/betalgo/openai/blob/master/Docs/openapi-split/components/schemas/inputaudio.yml">
///         Source Definition
///     </see>
/// </summary>
public class InputAudioContent : IContent
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="InputAudioContent" /> class.
    /// </summary>
    public InputAudioContent()
    {
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="InputAudioContent" /> class with audio data.
    /// </summary>
    /// <param name="data">Base64-encoded audio data.</param>
    /// <param name="format">The format of the audio data (<c>mp3</c> or <c>wav</c>).</param>
    public InputAudioContent(string data, string format)
    {
        InputAudio = new InputAudioData(data, format);
    }

    /// <summary>
    ///     The type of the input item. Always <c>input_audio</c>.
    /// </summary>
    [JsonPropertyName("type")]
    public string Type => "input_audio";

    /// <summary>
    ///     The audio data.
    /// </summary>
    [JsonPropertyName("input_audio")]
    public InputAudioData? InputAudio { get; set; }
}

/// <summary>
///     Audio data for input.
/// </summary>
public class InputAudioData
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="InputAudioData" /> class.
    /// </summary>
    public InputAudioData()
    {
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="InputAudioData" /> class with parameters.
    /// </summary>
    /// <param name="data">Base64-encoded audio data.</param>
    /// <param name="format">The format of the audio data.</param>
    public InputAudioData(string data, string format)
    {
        Data = data;
        Format = format;
    }

    /// <summary>
    ///     Base64-encoded audio data.
    /// </summary>
    [JsonPropertyName("data")]
    public string Data { get; set; } = null!;

    /// <summary>
    ///     The format of the audio data. Currently supported formats are <c>mp3</c> and <c>wav</c>.
    /// </summary>
    [JsonPropertyName("format")]
    public string Format { get; set; } = null!;
}




