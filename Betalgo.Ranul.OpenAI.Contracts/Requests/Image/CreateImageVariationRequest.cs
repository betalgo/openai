using System.Text.Json.Serialization;
using Betalgo.Ranul.OpenAI.Contracts.Enums.Image;
using Betalgo.Ranul.OpenAI.Contracts.Enums.Model;
using Betalgo.Ranul.OpenAI.Contracts.Interfaces;
using Betalgo.Ranul.OpenAI.Contracts.Types;

namespace Betalgo.Ranul.OpenAI.Contracts.Requests.Image;

/// <summary>
///     <see href="https://platform.openai.com/docs/api-reference/images/createVariation">
///         OpenAI API documentation
///     </see>
///     <see href="https://github.com/betalgo/openai/blob/master/Docs/openapi-split/components/schemas/createimagevariationrequest.yml">
///         Source Definition
///     </see>
/// </summary>
public class CreateImageVariationRequest : IRequest, IHasModelImage, IHasImageSize
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="CreateImageVariationRequest" /> class.
    /// </summary>
    public CreateImageVariationRequest()
    {
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="CreateImageVariationRequest" /> class.
    /// </summary>
    /// <param name="image">
    ///     The image to use as the basis for the variation(s). Must be a valid PNG file, less than 4MB, and square.
    /// </param>
    public CreateImageVariationRequest(FormFile image)
    {
        Image = image;
    }


    /// <summary>
    ///     The image to use as the basis for the variation(s). Must be a valid PNG file, less than 4MB, and square.
    /// </summary>
    [JsonPropertyName("image")]
    public FormFile Image { get; set; } = null!;

    /// <summary>
    ///     The number of images to generate. Must be between 1 and 10.
    /// </summary>
    [JsonPropertyName("n")]
    public int? N { get; set; }

    /// <summary>
    ///     The format in which the generated images are returned. Must be one of <c>url</c> or <c>b64_json</c>. URLs are only
    ///     valid for 60 minutes after the image has been generated.
    /// </summary>
    [JsonPropertyName("response_format")]
    public ImageResponseFormat? ResponseFormat { get; set; }

    /// <summary>
    ///     A unique identifier representing your end-user, which can help OpenAI to monitor and detect abuse.
    ///     <see href="https://platform.openai.com/docs/guides/safety-best-practices#end-user-ids">Learn more</see>.
    /// </summary>
    [JsonPropertyName("user")]
    public string? User { get; set; }

    /// <summary>
    ///     The size of the generated images. Must be one of <c>256x256</c>, <c>512x512</c>, or <c>1024x1024</c>.
    /// </summary>
    [JsonPropertyName("size")]
    public ImageSize? Size { get; set; }

    /// <summary>
    ///     The model to use for image generation. Only <c>dall-e-2</c> is supported at this time.
    /// </summary>
    [JsonPropertyName("model")]
    public ImageModel? Model { get; set; }
}