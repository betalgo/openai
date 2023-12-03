using System.Text.Json.Serialization;

namespace OpenAI.ObjectModels.SharedModels;

public record SharedImageRequestBaseModel
{
    /// <summary>
    ///     The number of images to generate. Must be between 1 and 10.
    ///     For dall-e-3 model, only n=1 is supported.
    /// </summary>
    [JsonPropertyName("n")]
    public int? N { get; set; }

    /// <summary>
    ///     The size of the generated images. 
    ///     Must be one of 256x256, 512x512, or 1024x1024 for dall-e-2. 
    ///     Must be one of 1024x1024, 1792x1024, or 1024x1792 for dall-e-3 models.
    ///     <br /><br />Check <see cref="StaticValues.ImageStatics.Size"/> for possible values
    /// </summary>
    [JsonPropertyName("size")]
    public string? Size { get; set; }

    /// <summary>
    ///     The format in which the generated images are returned. Must be one of url or b64_json
    /// </summary>
    [JsonPropertyName("response_format")]
    public string? ResponseFormat { get; set; }

    /// <summary>
    ///     A unique identifier representing your end-user, which will help OpenAI to monitor and detect abuse.
    ///     <a href="https://platform.openai.com/docs/usage-policies/end-user-ids">Learn more</a>.
    /// </summary>
    [JsonPropertyName("user")]
    public string? User { get; set; }

    /// <summary>
    ///     The model to use for image generation. Must be one of dall-e-2 or dall-e-3
    ///     For ImageEditCreateRequest and for ImageVariationCreateRequest only dall-e-2 modell is supported at this time.
    /// </summary>
    [JsonPropertyName("model")]
    public string? Model { get; set; }
}
