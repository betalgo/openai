using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using OpenAI.ObjectModels.SharedModels;

namespace OpenAI.ObjectModels.RequestModels;

public record CreateModerationRequest : IOpenAiModels.IModel
{
    /// <summary>
    ///     The input text to classify
    /// </summary>
    [JsonIgnore]
    public List<string>? InputAsList { get; set; }

    /// <summary>
    ///     The input text to classify
    /// </summary>
    [JsonIgnore]
    public string? Input { get; set; }


    [JsonPropertyName("input")]
    public IList<string>? InputCalculated
    {
        get
        {
            if (Input != null && InputAsList != null)
            {
                throw new ValidationException("Input and InputAsList can not be assigned at the same time. One of them is should be null.");
            }

            if (Input != null)
            {
                return new List<string> {Input};
            }

            return InputAsList;
        }
    }

    /// <summary>
    ///     Two content moderations models are available: text-moderation-stable and text-moderation-latest.
    ///     The default is text-moderation-latest which will be automatically upgraded over time. This ensures you are always
    ///     using our most accurate model. If you use text-moderation-stable, we will provide advanced notice before updating
    ///     the model. Accuracy of text-moderation-stable may be slightly lower than for text-moderation-latest.
    /// </summary>
    [JsonPropertyName("model")]
    public string? Model { get; set; }
}