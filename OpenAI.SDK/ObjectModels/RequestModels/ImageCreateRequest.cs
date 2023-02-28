using System.Text.Json.Serialization;
using OpenAI.GPT3.ObjectModels.SharedModels;

namespace OpenAI.GPT3.ObjectModels.RequestModels;

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