using System.Text.Json.Serialization;
using OpenAI.ObjectModels.SharedModels;

namespace OpenAI.ObjectModels.RequestModels;

public record ImageEditCreateRequest : SharedImageRequestBaseModel
{
    /// <summary>
    ///     The image to edit. Must be a valid PNG file, less than 4MB, and square.
    /// </summary>
    public byte[] Image { get; set; }

    /// <summary>
    ///     Image file name
    /// </summary>
    public string ImageName { get; set; }

    /// <summary>
    ///     An additional image whose fully transparent areas (e.g. where alpha is zero) indicate where image should be edited.
    ///     Must be a valid PNG file, less than 4MB, and have the same dimensions as image.
    /// </summary>
    public byte[]? Mask { get; set; }

    /// <summary>
    ///     Mask file name
    /// </summary>
    public string? MaskName { get; set; }

    /// <summary>
    ///     A text description of the desired image(s). The maximum length is 1000 characters.
    /// </summary>
    [JsonPropertyName("prompt")]
    public string Prompt { get; set; }
}