using System.Text.Json.Serialization;

namespace Betalgo.Ranul.OpenAI.Contracts.Types.Items;

/// <summary>
///     A result from a file search.
///     <see href="https://platform.openai.com/docs/guides/tools-file-search">File Search Guide</see>.
/// </summary>
public class FileSearchResult
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="FileSearchResult" /> class.
    /// </summary>
    public FileSearchResult()
    {
    }

    /// <summary>
    ///     Initializes a new instance with parameters.
    /// </summary>
    /// <param name="fileId">The unique ID of the file.</param>
    /// <param name="text">The text that was retrieved from the file.</param>
    /// <param name="filename">The name of the file.</param>
    /// <param name="score">The relevance score.</param>
    public FileSearchResult(string? fileId = null, string? text = null, string? filename = null, double? score = null)
    {
        FileId = fileId;
        Text = text;
        Filename = filename;
        Score = score;
    }

    /// <summary>
    ///     The unique ID of the file.
    /// </summary>
    [JsonPropertyName("file_id")]
    public string? FileId { get; set; }

    /// <summary>
    ///     The text that was retrieved from the file.
    /// </summary>
    [JsonPropertyName("text")]
    public string? Text { get; set; }

    /// <summary>
    ///     The name of the file.
    /// </summary>
    [JsonPropertyName("filename")]
    public string? Filename { get; set; }

    /// <summary>
    ///     The relevance score of the file - a value between 0 and 1.
    /// </summary>
    [JsonPropertyName("score")]
    public double? Score { get; set; }

    /// <summary>
    ///     Additional attributes of the file.
    /// </summary>
    [JsonPropertyName("attributes")]
    public Dictionary<string, object>? Attributes { get; set; }
}

