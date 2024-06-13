using System.Text.Json.Serialization;
using OpenAI.ObjectModels.SharedModels;

namespace OpenAI.ObjectModels.ResponseModels.ImageResponseModel;

public record ImageCreateResponse : BaseResponse, IOpenAiModels.ICreatedAt
{
    [JsonPropertyName("data")] public List<ImageDataResult> Results { get; set; }

    [JsonPropertyName("created")] public long CreatedAt { get; set; }

    public record ImageDataResult
    {
        [JsonPropertyName("url")] public string Url { get; set; }
        [JsonPropertyName("b64_json")] public string B64 { get; set; }
        [JsonPropertyName("revised_prompt")] public string RevisedPrompt { get; set; }
    }
}