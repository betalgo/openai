using System.Text.Json.Serialization;

namespace Betalgo.Ranul.OpenAI.Contracts.Types.Items;

/// <summary>
///     An image generation request made by the model.
///     <see href="https://platform.openai.com/docs/api-reference/responses">OpenAI API documentation</see>.
///     <see href="https://github.com/betalgo/openai/blob/master/Docs/openapi-split/components/schemas/imagegentoolcall.yml">
///         Source Definition
///     </see>
/// </summary>
public class ImageGenToolCall : IOutputItem
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="ImageGenToolCall" /> class.
    /// </summary>
    public ImageGenToolCall()
    {
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="ImageGenToolCall" /> class with required parameters.
    /// </summary>
    /// <param name="id">The unique ID of the image generation call.</param>
    /// <param name="status">The status of the image generation call.</param>
    public ImageGenToolCall(string id, string status)
    {
        Id = id;
        Status = status;
    }

    /// <summary>
    ///     The type of the image generation call. Always <c>image_generation_call</c>.
    /// </summary>
    [JsonPropertyName("type")]
    public string Type => "image_generation_call";

    /// <summary>
    ///     The unique ID of the image generation call.
    /// </summary>
    [JsonPropertyName("id")]
    public string Id { get; set; } = null!;

    /// <summary>
    ///     The status of the image generation call.
    ///     One of <c>in_progress</c>, <c>completed</c>, <c>generating</c>, or <c>failed</c>.
    /// </summary>
    [JsonPropertyName("status")]
    public string Status { get; set; } = null!;

    /// <summary>
    ///     The generated image encoded in base64.
    /// </summary>
    [JsonPropertyName("result")]
    public string? Result { get; set; }
}




