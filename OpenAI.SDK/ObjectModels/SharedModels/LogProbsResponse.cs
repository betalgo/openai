using System.Text.Json.Serialization;

namespace OpenAI.GPT3.ObjectModels.SharedModels;

public record LogProbsResponse
{
    [JsonPropertyName("tokens")] public List<string> Tokens { get; set; }

    [JsonPropertyName("token_logprobs")] public List<double> TokenLogProbs { get; set; }

    [JsonPropertyName("top_logprobs")] public List<Dictionary<string, double>> TopLogProbsRaw { get; set; }

    public Dictionary<string, double> TopLogProbs => TopLogProbsRaw.SelectMany(r => r).ToDictionary(k => k.Key, k => k.Value);

    [JsonPropertyName("text_offset")] public List<int> TextOffset { get; set; }
}