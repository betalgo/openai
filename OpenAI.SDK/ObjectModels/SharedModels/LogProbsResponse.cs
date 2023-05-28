using System.Text.Json.Serialization;

namespace OpenAI.ObjectModels.SharedModels;

public record LogProbsResponse
{
    [JsonPropertyName("tokens")] public List<string> Tokens { get; set; }

    [JsonPropertyName("token_logprobs")] public List<double> TokenLogProbs { get; set; }

    [JsonPropertyName("top_logprobs")] public List<Dictionary<string, double>> TopLogProbsRaw { get; set; }

    public List<TopLogProbResponse> TopLogProbs => TopLogProbsRaw.SelectMany(r => r.Select(a => new TopLogProbResponse
    {
        Key = a.Key,
        LogProp = a.Value
    })).ToList();

    [JsonPropertyName("text_offset")] public List<int> TextOffset { get; set; }
}

public class TopLogProbResponse
{
    public string Key { get; set; }
    public double LogProp { get; set; }
}