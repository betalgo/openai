using OpenAI.ObjectModels.RequestModels;
using OpenAI.ObjectModels.ResponseModels;

namespace OpenAI.Interfaces;

/// <summary>
///     Learn how to turn audio into text.
///     Related guide: <a href="https://platform.openai.com/docs/guides/speech-to-text">Speech to text</a>
/// </summary>
public interface IAudioService
{
    /// <summary>
    ///     Transcribes audio into the input language.
    /// </summary>
    /// <param name="audioCreateTranscriptionRequest"></param>
    /// <param name="cancellationToken">Propagates notification that operations should be canceled.</param>
    /// <returns></returns>
    Task<AudioCreateTranscriptionResponse> CreateTranscription(AudioCreateTranscriptionRequest audioCreateTranscriptionRequest, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Translates audio into into English.
    /// </summary>
    /// <param name="audioCreateTranscriptionRequest"></param>
    /// <param name="cancellationToken">Propagates notification that operations should be canceled.</param>
    /// <returns></returns>
    Task<AudioCreateTranscriptionResponse> CreateTranslation(AudioCreateTranscriptionRequest audioCreateTranscriptionRequest, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Generates audio from the input text.
    /// </summary>
    /// <param name="audioCreateSpeechRequest"></param>
    /// <param name="cancellationToken">Propagates notification that operations should be canceled.</param>
    /// <returns></returns>
    Task<AudioCreateSpeechResponse<T>> CreateSpeech<T>(AudioCreateSpeechRequest audioCreateSpeechRequest, CancellationToken cancellationToken = default);


}
