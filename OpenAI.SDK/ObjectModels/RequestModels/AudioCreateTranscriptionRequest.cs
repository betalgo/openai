using OpenAI.ObjectModels.SharedModels;

namespace OpenAI.ObjectModels.RequestModels;

public record AudioCreateTranscriptionRequest : IOpenAiModels.IModel, IOpenAiModels.ITemperature, IOpenAiModels.IFile
{
    /// <summary>
    ///     An optional text to guide the model's style or continue a previous audio segment. The prompt should match the audio
    ///     language.
    /// </summary>
    public string? Prompt { get; set; }

    /// <summary>
    ///     The format of the transcript output, in one of these options: json, text, srt, verbose_json, or vtt.
    /// </summary>
    public string? ResponseFormat { get; set; }

    /// <summary>
    ///     The language of the input audio. Supplying the input language in
    ///     <a href="https://en.wikipedia.org/wiki/List_of_ISO_639-1_codes">ISO-639-1</a> format will improve accuracy and
    ///     latency.
    /// </summary>
    public string? Language { get; set; }

    /// <summary>
    ///     The timestamp granularities to populate for this transcription. response_format must be set verbose_json to use
    ///     timestamp granularities. Either or both of these options are supported:
    ///     <a cref="StaticValues.AudioStatics.TimestampGranularity.Word">word</a>, or
    ///     <a cref="StaticValues.AudioStatics.TimestampGranularity.Segment">segment</a>. Note: There is no
    ///     additional latency for segment timestamps, but generating word timestamps incurs additional latency.
    /// </summary>
    public List<string>? TimestampGranularities { get; set; }

    /// <summary>
    ///     The audio file to transcribe, in one of these formats: mp3, mp4, mpeg, mpga, m4a, wav, or webm.
    /// </summary>
    public byte[]? File { get; set; }

    /// <summary>
    ///     The stream of the audio file to transcribe, in one of these formats: mp3, mp4, mpeg, mpga, m4a, wav, or webm.
    /// </summary>
    public Stream? FileStream { get; set; }

    /// <summary>
    ///     FileName
    /// </summary>
    public string FileName { get; set; }

    /// <summary>
    ///     ID of the model to use. Only whisper-1 is currently available.
    /// </summary>
    public string Model { get; set; }

    /// <summary>
    ///     The sampling temperature, between 0 and 1. Higher values like 0.8 will make the output more random, while lower
    ///     values like 0.2 will make it more focused and deterministic. If set to 0, the model will use log probability to
    ///     automatically increase the temperature until certain thresholds are hit.
    /// </summary>
    public float? Temperature { get; set; }
}