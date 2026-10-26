using System.Text.Json.Serialization;

namespace Betalgo.Ranul.OpenAI.Contracts.Types.Content;

/// <summary>
///     An annotation in the text output.
///     <see href="https://platform.openai.com/docs/api-reference/responses">OpenAI API documentation</see>.
/// </summary>
public class Annotation
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="Annotation" /> class.
    /// </summary>
    public Annotation()
    {
    }

    /// <summary>
    ///     Initializes a new instance with parameters.
    /// </summary>
    /// <param name="type">The type of the annotation.</param>
    /// <param name="startIndex">The start index of the annotation in the text.</param>
    /// <param name="endIndex">The end index of the annotation in the text.</param>
    public Annotation(string? type = null, int? startIndex = null, int? endIndex = null)
    {
        Type = type;
        StartIndex = startIndex;
        EndIndex = endIndex;
    }

    /// <summary>
    ///     The type of the annotation.
    /// </summary>
    [JsonPropertyName("type")]
    public string? Type { get; set; }

    /// <summary>
    ///     The start index of the annotation in the text.
    /// </summary>
    [JsonPropertyName("start_index")]
    public int? StartIndex { get; set; }

    /// <summary>
    ///     The end index of the annotation in the text.
    /// </summary>
    [JsonPropertyName("end_index")]
    public int? EndIndex { get; set; }

    /// <summary>
    ///     The URL associated with the annotation (for URL citations).
    /// </summary>
    [JsonPropertyName("url")]
    public string? Url { get; set; }

    /// <summary>
    ///     The title associated with the annotation.
    /// </summary>
    [JsonPropertyName("title")]
    public string? Title { get; set; }

    /// <summary>
    ///     The file ID associated with the annotation (for file citations).
    /// </summary>
    [JsonPropertyName("file_id")]
    public string? FileId { get; set; }

    /// <summary>
    ///     The filename associated with the annotation.
    /// </summary>
    [JsonPropertyName("filename")]
    public string? Filename { get; set; }
}

