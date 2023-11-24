using System.Text.Json.Serialization;

namespace OpenAI.ObjectModels.RequestModels;

/// <summary>
///     The content of a vision message.
/// </summary>
public class VisionContent
{
    /// <summary>
    ///     The value of Type property must be one of "text", "image_url"
    ///
    ///     note: Currently openAI doesn't support images in the first system message.
    /// </summary>
    [JsonPropertyName("type")]
    public string Type { get; set; }

    /// <summary>
    ///     If the value of Type property is "text" then Text property must contain the message content text
    /// </summary>
    [JsonPropertyName("text")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Text { get; set; }

    /// <summary>
    ///     If the value of Type property is "image_url" then ImageUrl property must contain a valid image url object
    /// </summary>
    [JsonPropertyName("image_url")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public VisionImageUrl? ImageUrl { get; set; }

    /// <summary>
    ///    Static helper method to create VisionContent Text
    /// <param name="text">The text content</param>
    /// </summary>
    public static VisionContent TextContent(string text)
    {
        return new() { Type = "text", Text = text };
    }

    /// <summary>
    ///    Static helper method to create VisionContent with Url
    ///    OpenAI currently supports PNG, JPEG, WEBP, and non-animated GIF
    /// <param name="imageUrl">The url of an image</param>
    /// <param name="detail">The detail property</param>
    /// </summary>
    public static VisionContent ImageUrlContent(string imageUrl, string? detail = "auto")
    {
        return new()
        {
            Type = "image_url",
            ImageUrl = new() { Url = imageUrl, Detail = detail }
        };
    }

    /// <summary>
    ///    Static helper method to create VisionContent from binary image
    ///    OpenAI currently supports PNG, JPEG, WEBP, and non-animated GIF
    /// <param name="binaryImage">The image binary data as byte array</param>
    /// <param name="imageType">The type of image</param>
    /// <param name="detail">The detail property</param>
    /// </summary>
    public static VisionContent ImageBinaryContent(
        byte[] binaryImage,
        string imageType,
        string? detail = "auto"
    )
    {
        return new()
        {
            Type = "image_url",
            ImageUrl = new()
            {
                Url = string.Format(
                    "data:image/{0};base64,{{{1}}}",
                    imageType,
                    Convert.ToBase64String(binaryImage)
                ),
                Detail = detail
            }
        };
    }
}
