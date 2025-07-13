using System.Text.Json.Serialization;
using Betalgo.Ranul.OpenAI.Contracts.Enums;
using Betalgo.Ranul.OpenAI.ObjectModels.SharedModels;

namespace Betalgo.Ranul.OpenAI.ObjectModels.RequestModels;

/// <summary>
///     Image Create Request Model
/// </summary>
public record ImageCreateRequest : SharedImageRequestBaseModel, IOpenAIModels.IUser
{
    public ImageCreateRequest()
    {
    }

    public ImageCreateRequest(string prompt)
    {
        Prompt = prompt;
    }

    /// <summary>
    ///     A text description of the desired image(s). The maximum length is 1000 characters for dall-e-2 and 4000 characters
    ///     for dall-e-3
    /// </summary>
    [JsonPropertyName("prompt")]
    public string Prompt { get; set; }

    /// <summary>
    ///     The quality of the image that will be generated. Possible values are 'standard' or 'hd' (default is 'standard').
    ///     Hd creates images with finer details and greater consistency across the image.
    ///     This param is only supported for dall-e-3 model.
    /// </summary>
    [JsonPropertyName("quality")]
    public ImageQuality? Quality { get; set; }

    /// <summary>
    ///     The style of the generated images. Must be one of vivid or natural.
    ///     Vivid causes the model to lean towards generating hyper-real and dramatic images.
    ///     Natural causes the model to produce more natural, less hyper-real looking images. This param is only supported for
    ///     dall-e-3.
    /// </summary>
    [JsonPropertyName("style")]
    public ImageStyle? Style { get; set; }
}