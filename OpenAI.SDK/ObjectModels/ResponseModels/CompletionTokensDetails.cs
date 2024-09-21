using System.Text.Json.Serialization;

namespace OpenAI.ObjectModels.ResponseModels;

public record CompletionTokensDetails
{
    [JsonPropertyName("reasoning_tokens")]
    public int ReasoningTokens { get; set; }
}