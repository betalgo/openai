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
    ///     A text description of the desired image(s). The maximum length is 1000 characters.
    /// </summary>
    [JsonPropertyName("prompt")]
    public string Prompt { get; set; }
}