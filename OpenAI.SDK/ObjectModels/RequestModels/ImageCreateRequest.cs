using System.Text.Json.Serialization;
using OpenAI.ObjectModels.SharedModels;

namespace OpenAI.ObjectModels.RequestModels;

/// <summary>
///     Image Create Request Model
/// </summary>
public record ImageCreateRequest : SharedImageRequestBaseModel, IOpenAiModels.IUser
{
    public ImageCreateRequest()
    {
    }

    public ImageCreateRequest(string prompt)
    {
        Prompt = prompt;
    }

    /// <summary>
    ///     A text description of the desired image(s). The maximum length is 1000 characters for dall-e-2 and 4000 characters for dall-e-3
    /// </summary>
    [JsonPropertyName("prompt")]
    public string Prompt { get; set; }

    /// <summary>
    ///     The quality of the image that will be generated. Possible values are 'standard' or 'hd' (default is 'standard').
    ///     Hd creates images with finer details and greater consistency across the image. 
    ///     This param is only supported for dall-e-3 model.
    ///     <br /><br />Check <see cref="StaticValues.ImageStatics.Quality"/> for possible values
    /// </summary>
    [JsonPropertyName("quality")]
    public string? Quality { get; set; }

    /// <summary>
    ///     The style of the generated images. Must be one of vivid or natural. 
    ///     Vivid causes the model to lean towards generating hyper-real and dramatic images. 
    ///     Natural causes the model to produce more natural, less hyper-real looking images. This param is only supported for dall-e-3. 
    ///     <br /><br />Check <see cref="StaticValues.ImageStatics.Style"/> for possible values
    /// </summary>
    [JsonPropertyName("style")]
    public string? Style { get; set; }
}