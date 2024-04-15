using System.Text.Json.Serialization;

namespace OpenAI.ObjectModels.SharedModels;

/// <summary>
///     The text content that is part of a message.
/// </summary>
public record MessageText
{
    /// <summary>
    ///     The data that makes up the text.
    /// </summary>
    [JsonPropertyName("value")]
    public string Value { get; set; }

    /// <summary>
    ///     annotations
    /// </summary>
    [JsonPropertyName("annotations")]
    public List<MessageAnnotation> Annotations { get; set; }
}