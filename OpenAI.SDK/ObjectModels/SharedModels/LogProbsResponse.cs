using System.Text.Json.Serialization;

namespace OpenAI.GPT3.ObjectModels.SharedModels;

public record LogProbsResponse
{
    [JsonPropertyName("tokens")] public string[] Tokens { get; set; }

    [JsonPropertyName("token_logprobs")] public double[] TokenLogProbs { get; set; }

    [JsonPropertyName("top_logprobs")] public Dictionary<string,double>[] TopLogProbs { get; set; }

    [JsonPropertyName("text_offset")] public int[] TextOffset { get; set; }
}