using System.Text.Json.Serialization;
using Betalgo.Ranul.OpenAI.Contracts.Enums.Image;
using Betalgo.Ranul.OpenAI.Contracts.Enums.Model;
using Betalgo.Ranul.OpenAI.Contracts.Interfaces;
using Betalgo.Ranul.OpenAI.Contracts.Types;

namespace Betalgo.Ranul.OpenAI.Contracts.Requests.Image;

/// <summary>
///     <see href="https://platform.openai.com/docs/api-reference/images/createEdit">
///         OpenAI API documentation
///     </see>
/// </summary>
public class CreateImageEditRequest : IRequest, IHasModelImage, IHasImageSize, IHasImageBackground, IHasImageOutputFormat, IHasImageQuality
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="CreateImageEditRequest" /> class.
    /// </summary>
    public CreateImageEditRequest()
    {
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="CreateImageEditRequest" /> class.
    /// </summary>
    /// <param name="image">
    ///     The image(s) to edit. Must be a supported image file or an array of images.
    ///     <para />
    ///     For <c>gpt-image-1</c>, each image should be a <c>png</c>, <c>webp</c>, or <c>jpg</c> file less
    ///     than 50MB. You can provide up to 16 images.
    ///     <para />
    ///     For <c>dall-e-2</c>, you can only provide one image, and it should be a square
    ///     <c>png</c> file less than 4MB.
    /// </param>
    /// <param name="prompt">
    ///     A text description of the desired image(s). The maximum length is 1000 characters for <c>dall-e-2</c>, and 32000
    ///     characters for <c>gpt-image-1</c>.
    /// </param>
    public CreateImageEditRequest(FormFileOrList image, string prompt)
    {
        Image = image;
        Prompt = prompt;
    }


    /// <summary>
    ///     The image(s) to edit. Must be a supported image file or an array of images.
    ///     <para />
    ///     For <c>gpt-image-1</c>, each image should be a <c>png</c>, <c>webp</c>, or <c>jpg</c> file less
    ///     than 50MB. You can provide up to 16 images.
    ///     <para />
    ///     For <c>dall-e-2</c>, you can only provide one image, and it should be a square
    ///     <c>png</c> file less than 4MB.
    /// </summary>
    [JsonPropertyName("image")]
    public FormFileOrList Image { get; set; }

    /// <summary>
    ///     A text description of the desired image(s). The maximum length is 1000 characters for <c>dall-e-2</c>, and 32000
    ///     characters for <c>gpt-image-1</c>.
    /// </summary>
    [JsonPropertyName("prompt")]
    public string Prompt { get; set; }

    /// <summary>
    ///     An additional image whose fully transparent areas (e.g. where alpha is zero) indicate where <c>image</c> should be
    ///     edited. If there are multiple images provided, the mask will be applied on the first image. Must be a valid PNG
    ///     file, less than 4MB, and have the same dimensions as <c>image</c>.
    /// </summary>
    [JsonPropertyName("mask")]
    public FormFile? Mask { get; set; }

    /// <summary>
    ///     The number of images to generate. Must be between 1 and 10.
    /// </summary>
    [JsonPropertyName("n")]
    public int? N { get; set; }

    /// <summary>
    ///     The format in which the generated images are returned. Must be one of <c>url</c> or <c>b64_json</c>. URLs are only
    ///     valid for 60 minutes after the image has been generated. This parameter is only supported for <c>dall-e-2</c>, as
    ///     <c>gpt-image-1</c> will always return base64-encoded images.
    /// </summary>
    [JsonPropertyName("response_format")]
    public ImageResponseFormat? ResponseFormat { get; set; }

    /// <summary>
    ///     The compression level (0-100%) for the generated images. This parameter
    ///     is only supported for <c>gpt-image-1</c> with the <c>webp</c> or <c>jpeg</c> output
    ///     formats, and defaults to 100.
    /// </summary>
    [JsonPropertyName("output_compression")]
    public int? OutputCompression { get; set; }

    /// <summary>
    ///     A unique identifier representing your end-user, which can help OpenAI to monitor and detect abuse.
    ///     <see href="https://platform.openai.com/docs/guides/safety-best-practices#end-user-ids">Learn more</see>.
    /// </summary>
    [JsonPropertyName("user")]
    public string? User { get; set; }

    /// <summary>
    ///     Allows to set transparency for the background of the generated image(s).
    ///     This parameter is only supported for <c>gpt-image-1</c>. Must be one of
    ///     <c>transparent</c>, <c>opaque</c> or <c>auto</c> (default value). When <c>auto</c> is used, the
    ///     model will automatically determine the best background for the image.
    ///     <para />
    ///     If <c>transparent</c>, the output format needs to support transparency, so it
    ///     should be set to either <c>png</c> (default value) or <c>webp</c>.
    /// </summary>
    [JsonPropertyName("background")]
    public ImageBackground? Background { get; set; }

    /// <summary>
    ///     The format in which the generated images are returned. This parameter is
    ///     only supported for <c>gpt-image-1</c>. Must be one of <c>png</c>, <c>jpeg</c>, or <c>webp</c>.
    ///     The default value is <c>png</c>.
    /// </summary>
    [JsonPropertyName("output_format")]
    public ImageOutputFormat? OutputFormat { get; set; }

    /// <summary>
    ///     The quality of the image that will be generated. <c>high</c>, <c>medium</c> and <c>low</c> are only supported for
    ///     <c>gpt-image-1</c>. <c>dall-e-2</c> only supports <c>standard</c> quality. Defaults to <c>auto</c>.
    /// </summary>
    [JsonPropertyName("quality")]
    public ImageQuality? Quality { get; set; }

    /// <summary>
    ///     The size of the generated images. Must be one of <c>1024x1024</c>, <c>1536x1024</c> (landscape), <c>1024x1536</c>
    ///     (portrait), or <c>auto</c> (default value) for <c>gpt-image-1</c>, and one of <c>256x256</c>, <c>512x512</c>, or
    ///     <c>1024x1024</c> for <c>dall-e-2</c>.
    /// </summary>
    [JsonPropertyName("size")]
    public ImageSize? Size { get; set; }

    /// <summary>
    ///     The model to use for image generation. Only <c>dall-e-2</c> and <c>gpt-image-1</c> are supported. Defaults to
    ///     <c>dall-e-2</c> unless a parameter specific to <c>gpt-image-1</c> is used.
    /// </summary>
    [JsonPropertyName("model")]
    public ModelImage? Model { get; set; }
}