namespace OpenAI.ObjectModels.ResponseModels;

/// <summary>
///     File content response
/// </summary>
/// <typeparam name="T"></typeparam>
public record AudioCreateSpeechResponse<T> : DataBaseResponse<T>
{
    /// <summary>
    ///     The audio file content.
    /// </summary>
    public T? AudioContent { get; set; }
}