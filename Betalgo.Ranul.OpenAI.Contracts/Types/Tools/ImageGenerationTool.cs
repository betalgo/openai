using System.Text.Json.Serialization;

namespace Betalgo.Ranul.OpenAI.Contracts.Types.Tools;

/// <summary>
///     An image generation tool that allows the model to generate images.
///     <see href="https://platform.openai.com/docs/guides/tools">OpenAI Tools Guide</see>.
///     <see href="https://github.com/betalgo/openai/blob/master/Docs/openapi-split/components/schemas/imagegentool.yml">
///         Source Definition
///     </see>
/// </summary>
public class ImageGenerationTool : ITool
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="ImageGenerationTool" /> class.
    /// </summary>
    public ImageGenerationTool()
    {
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="ImageGenerationTool" /> class with parameters.
    /// </summary>
    /// <param name="quality">The quality of the generated images.</param>
    /// <param name="size">The size of the generated images.</param>
    public ImageGenerationTool(string? quality = null, string? size = null)
    {
        Quality = quality;
        Size = size;
    }

    /// <summary>
    ///     The type of the image generation tool. Always <c>image_generation</c>.
    /// </summary>
    [JsonPropertyName("type")]
    public string Type => "image_generation";

    /// <summary>
    ///     The quality of the generated images.
    /// </summary>
    [JsonPropertyName("quality")]
    public string? Quality { get; set; }

    /// <summary>
    ///     The size of the generated images.
    /// </summary>
    [JsonPropertyName("size")]
    public string? Size { get; set; }
}

