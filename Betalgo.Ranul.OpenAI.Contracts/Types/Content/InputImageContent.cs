using System.Text.Json.Serialization;

namespace Betalgo.Ranul.OpenAI.Contracts.Types.Content;

/// <summary>
///     An image input to the model.
///     <see href="https://platform.openai.com/docs/guides/vision">Vision Guide</see>.
///     <see href="https://github.com/betalgo/openai/blob/master/Docs/openapi-split/components/schemas/inputimagecontent.yml">
///         Source Definition
///     </see>
/// </summary>
public class InputImageContent : IContent
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="InputImageContent" /> class.
    /// </summary>
    public InputImageContent()
    {
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="InputImageContent" /> class with an image URL.
    /// </summary>
    /// <param name="imageUrl">The URL of the image.</param>
    /// <param name="detail">The detail level for the image. Default is <c>auto</c>.</param>
    public InputImageContent(string imageUrl, string detail = "auto")
    {
        ImageUrl = imageUrl;
        Detail = detail;
    }

    /// <summary>
    ///     The type of the input item. Always <c>input_image</c>.
    /// </summary>
    [JsonPropertyName("type")]
    public string Type => "input_image";

    /// <summary>
    ///     The URL of the image to be sent to the model.
    ///     A fully qualified URL or base64 encoded image in a data URL.
    /// </summary>
    [JsonPropertyName("image_url")]
    public string? ImageUrl { get; set; }

    /// <summary>
    ///     The ID of the file to be sent to the model.
    /// </summary>
    [JsonPropertyName("file_id")]
    public string? FileId { get; set; }

    /// <summary>
    ///     The detail level for the image. One of <c>low</c>, <c>high</c>, or <c>auto</c>.
    /// </summary>
    [JsonPropertyName("detail")]
    public string Detail { get; set; } = "auto";
}




