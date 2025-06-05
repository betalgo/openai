using System.Text.Json.Serialization;

namespace Betalgo.Ranul.OpenAI.ObjectModels.RequestModels;

/// <summary>
///     The content of a message.
/// </summary>
public class MessageContent
{
    /// <summary>
    ///     The value of Type property must be one of "text", "image_url"
    ///     note: Currently openAI doesn't support images in the first system message.
    /// </summary>
    [JsonPropertyName("type")]
    public string Type { get; set; } = null!;

    /// <summary>
    ///     If the value of Type property is "text" then Text property must contain the message content text
    /// </summary>
    [JsonPropertyName("text")]
    public string? Text { get; set; }

    /// <summary>
    ///     If the value of Type property is "image_url" then ImageUrl property must contain a valid image url object
    /// </summary>
    [JsonPropertyName("image_url")]
    public MessageImageUrl? ImageUrl { get; set; }

    [JsonPropertyName("image_file")]
    public MessageImageFile? ImageFile { get; set; }

    [JsonPropertyName("file")]
    public MessageFile? File { get; set; }

    /// <summary>
    ///     Static helper method to create MessageContent Text
    ///     <param name="text">The text content</param>
    /// </summary>
    public static MessageContent TextContent(string text) => new() { Type = "text", Text = text };

    /// <summary>
    ///     Static helper method to create MessageContent with Url
    ///     OpenAI currently supports PNG, JPEG, WEBP, and non-animated GIF
    ///     <param name="imageUrl">The url of an image</param>
    ///     <param name="detail">The detail property</param>
    /// </summary>
    public static MessageContent ImageUrlContent(string imageUrl, string? detail = null) => new()
    {
        Type = "image_url",
        ImageUrl = new() { Url = imageUrl, Detail = detail }
    };

    public static MessageContent ImageFileContent(string fileId, string detail) => new()
    {
        Type = "image_file",
        ImageFile = new() { FileId = fileId, Detail = detail }
    };

    /// <summary>
    ///     Static helper method to create MessageContent from binary image
    ///     OpenAI currently supports PNG, JPEG, WEBP, and non-animated GIF
    ///     <param name="binaryImage">The image binary data as byte array</param>
    ///     <param name="imageType">The type of image</param>
    ///     <param name="detail">The detail property</param>
    /// </summary>
    public static MessageContent ImageBinaryContent(byte[] binaryImage, string imageType, string? detail = "auto") => new()
    {
        Type = "image_url",
        ImageUrl = new()
        {
            Url = $"data:image/{imageType};base64,{Convert.ToBase64String(binaryImage)}",
            Detail = detail
        }
    };

    /// <summary>
    ///     Static helper method to create MessageContent from binary image
    ///     OpenAI currently supports PNG, JPEG, WEBP, and non-animated GIF
    ///     <param name="binaryFile">The image binary data as byte array</param>
    ///     <param name="fileType">The type of file, as an example "pdf"</param>
    ///     <param name="fileName"> The name of the file, as an example "file.pdf"</param>
    /// </summary>
    public static MessageContent FileBinaryContent(byte[] binaryFile, string fileType, string fileName) => new()
    {
        Type = "file",
        File = new()
        {
            FileData = $"data:application/{fileType};base64,{Convert.ToBase64String(binaryFile)}",
            Filename = fileName
        }
    };

    public class MessageFile
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="MessageFile" /> class.
        /// </summary>
        public MessageFile()
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="MessageFile" /> class.
        /// </summary>
        /// <param name="filename">
        ///     The name of the file, used when passing the file to the model as a
        ///     string.
        /// </param>
        /// <param name="fileData">
        ///     The base64 encoded file data, used when passing the file to the model
        ///     as a string.
        /// </param>
        /// <param name="fileId">
        ///     The ID of an uploaded file to use as input.
        /// </param>
        public MessageFile(string filename, string fileData, string fileId)
        {
            Filename = filename;
            FileData = fileData;
            FileId = fileId;
        }


        /// <summary>
        ///     The name of the file, used when passing the file to the model as a
        ///     string.
        /// </summary>
        [JsonPropertyName("filename")]
        public string Filename { get; set; }

        /// <summary>
        ///     The base64 encoded file data, used when passing the file to the model
        ///     as a string.
        /// </summary>
        [JsonPropertyName("file_data")]
        public string FileData { get; set; }

        /// <summary>
        ///     The ID of an uploaded file to use as input.
        /// </summary>
        [JsonPropertyName("file_id")]
        public string FileId { get; set; }
    }
}