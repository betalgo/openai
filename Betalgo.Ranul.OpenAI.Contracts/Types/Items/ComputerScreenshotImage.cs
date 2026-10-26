using System.Text.Json.Serialization;

namespace Betalgo.Ranul.OpenAI.Contracts.Types.Items;

/// <summary>
///     A computer screenshot image used with the computer use tool.
///     <see href="https://platform.openai.com/docs/guides/tools-computer-use">Computer Use Guide</see>.
///     <see href="https://github.com/betalgo/openai/blob/master/Docs/openapi-split/components/schemas/computerscreenshotimage.yml">
///         Source Definition
///     </see>
/// </summary>
public class ComputerScreenshotImage
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="ComputerScreenshotImage" /> class.
    /// </summary>
    public ComputerScreenshotImage()
    {
    }

    /// <summary>
    ///     Specifies the event type. For a computer screenshot, this property is always set to <c>computer_screenshot</c>.
    /// </summary>
    [JsonPropertyName("type")]
    public string Type => "computer_screenshot";

    /// <summary>
    ///     The URL of the screenshot image.
    /// </summary>
    [JsonPropertyName("image_url")]
    public string? ImageUrl { get; set; }

    /// <summary>
    ///     The identifier of an uploaded file that contains the screenshot.
    /// </summary>
    [JsonPropertyName("file_id")]
    public string? FileId { get; set; }
}

