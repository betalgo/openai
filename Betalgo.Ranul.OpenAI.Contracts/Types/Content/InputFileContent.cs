using System.Text.Json.Serialization;

namespace Betalgo.Ranul.OpenAI.Contracts.Types.Content;

/// <summary>
///     A file input to the model.
///     <see href="https://platform.openai.com/docs/guides/pdf-files">File Inputs Guide</see>.
///     <see href="https://github.com/betalgo/openai/blob/master/Docs/openapi-split/components/schemas/inputfilecontent.yml">
///         Source Definition
///     </see>
/// </summary>
public class InputFileContent : IContent
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="InputFileContent" /> class.
    /// </summary>
    public InputFileContent()
    {
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="InputFileContent" /> class with a file URL.
    /// </summary>
    /// <param name="fileUrl">The URL of the file.</param>
    public InputFileContent(string fileUrl)
    {
        FileUrl = fileUrl;
    }

    /// <summary>
    ///     The type of the input item. Always <c>input_file</c>.
    /// </summary>
    [JsonPropertyName("type")]
    public string Type => "input_file";

    /// <summary>
    ///     The ID of the file to be sent to the model.
    /// </summary>
    [JsonPropertyName("file_id")]
    public string? FileId { get; set; }

    /// <summary>
    ///     The name of the file to be sent to the model.
    /// </summary>
    [JsonPropertyName("filename")]
    public string? Filename { get; set; }

    /// <summary>
    ///     The URL of the file to be sent to the model.
    /// </summary>
    [JsonPropertyName("file_url")]
    public string? FileUrl { get; set; }

    /// <summary>
    ///     The content of the file to be sent to the model (base64 encoded).
    /// </summary>
    [JsonPropertyName("file_data")]
    public string? FileData { get; set; }
}




