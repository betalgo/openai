using System.Text.Json.Serialization;
using OpenAI.GPT3.ObjectModels.SharedModels;

namespace OpenAI.GPT3.ObjectModels.RequestModels
{
    public record CreateModerationRequest : IOpenAiModels.IModel
    {
        /// <summary>
        ///     The input text to classify
        /// </summary>
        [JsonPropertyName("input")]
        public string Input { get; set; }

        /// <summary>
        ///     Two content moderations models are available: text-moderation-stable and text-moderation-latest.
        ///     The default is text-moderation-latest which will be automatically upgraded over time. This ensures you are always
        ///     using our most accurate model. If you use text-moderation-stable, we will provide advanced notice before updating
        ///     the model. Accuracy of text-moderation-stable may be slightly lower than for text-moderation-latest.
        /// </summary>
        [JsonPropertyName("model")]
        public string? Model { get; set; }
    }
}