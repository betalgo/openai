using System.Text.Json.Serialization;

namespace OpenAI.ObjectModels.ResponseModels;

/// <summary>
///     File content response
/// </summary>
/// <typeparam name="T"></typeparam>
public class AudioCreateSpeechResponse<T>
{
    /// <summary>
    ///     The audio file content.
    /// </summary>
    public T? AudioContent { get; set; }

    /// <summary>
    ///     return false if there is an error
    /// </summary>
    public bool Successful => Error == null;

    /// <summary>
    ///     Error
    /// </summary>
    [JsonPropertyName("error")]
    public Error? Error { get; set; }
}
