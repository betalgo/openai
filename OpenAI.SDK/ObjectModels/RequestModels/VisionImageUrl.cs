using System.Text.Json.Serialization;

namespace OpenAI.ObjectModels.RequestModels;

/// <summary>
///     The image_url object of vision message content
/// </summary>
public class MessageImageUrl
{
    /// <summary>
    ///     The Url property
    ///     Images are made available to the model in two main ways: by passing a link to the image or by passing the base64 encoded image directly in the url property.
    ///     link example: "url" : "https://upload.wikimedia.org/wikipedia/commons/thumb/d/dd/Gfp-wisconsin-madison-the-nature-boardwalk.jpg/2560px-Gfp-wisconsin-madison-the-nature-boardwalk.jpg"
    ///     base64 encoded image example: "url" : "data:image/jpeg;base64,{base64_image}"
    ///     
    ///     Limitations:
    ///     OpenAI currently supports PNG (.png), JPEG (.jpeg and .jpg), WEBP (.webp), and non-animated GIF (.gif) image formats
    ///     Image upload size is limited to 20MB per image
    ///     Captcha submission is blocked
    ///     
    /// </summary>
    [JsonPropertyName("url")]
    public string Url { get; set; }

    /// <summary>
    ///    The optional Detail property controls low or high fidelity image understanding
    ///    It has three options, low, high, or auto, you have control over how the model processes the image and generates its textual understanding.
    ///    By default, the model will use the auto setting which will look at the image input size and decide if it should use the low or high setting.
    ///    
    ///    low will disable the “high res” model. The model will receive a low-res 512px x 512px version of the image.
    ///    high will enable “high res” mode, which first allows the model to see the low res image and then creates detailed crops of input images 
    ///    as 512px squares based on the input image size.
    /// </summary>
    [JsonPropertyName("detail")]
    public string? Detail { get; set; }

}

public class MessageImageFile
{
    /// <summary>
    /// The File ID of the image in the message content. Set purpose="vision" when uploading the File if you need to later display the file content.
    /// </summary>
    [JsonPropertyName("file_Id")]
    public string FileId { get; set; }
    /// <summary>
    /// Specifies the detail level of the image if specified by the user. low uses fewer tokens, you can opt in to high resolution using high.
    /// </summary>
    [JsonPropertyName("detail")]
    public string Detail { get; set; }
}