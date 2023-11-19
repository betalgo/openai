using System.Text.Json.Serialization;

namespace OpenAI.ObjectModels.RequestModels;

/// <summary>
///     The content of a vision message.
/// </summary>
public class VisionContent
{
    /// <summary>
    /// </summary>
    /// <param name="type">The content type. One of text, image_url.</param>
    /// <param name="text">The text if content type is text.</param>
    public VisionContent(string type, string text)
    {
        Type = type;
        Text = text;
    }

    /// <summary>
    /// </summary>
    /// <param name="type">The content type. One of text, image_url.</param>
    /// <param name="imageUrl">The image url object if the content type is image_url</param>
    public VisionContent(string type, VisionImageUrl imageUrl)
    {
        Type = type;
        ImageUrl = imageUrl;
    }

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
    public string? Text { get; set; }

    /// <summary>
    ///     If the value of Type property is "image_url" then ImageUrl property must contain a valid image url object
    /// </summary>
    [JsonPropertyName("image_url")]
    public VisionImageUrl? ImageUrl { get; set; }


}