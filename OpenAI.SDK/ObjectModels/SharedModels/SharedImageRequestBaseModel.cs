using System.Text.Json.Serialization;

namespace OpenAI.GPT3.ObjectModels.SharedModels;

public record SharedImageRequestBaseModel
{
    /// <summary>
    ///     The number of images to generate. Must be between 1 and 10.
    /// </summary>
    [JsonPropertyName("n")]
    public int? N { get; set; }

    /// <summary>
    ///     The size of the generated images. Must be one of 256x256, 512x512, or 1024x1024.
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
}