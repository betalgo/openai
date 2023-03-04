using OpenAI.GPT3.ObjectModels.RequestModels;
using OpenAI.GPT3.ObjectModels.ResponseModels;

namespace OpenAI.GPT3.Interfaces;

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
}