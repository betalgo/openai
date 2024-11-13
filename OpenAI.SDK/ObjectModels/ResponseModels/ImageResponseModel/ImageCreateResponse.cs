using System.Text.Json.Serialization;
using Betalgo.Ranul.OpenAI.ObjectModels.SharedModels;

namespace Betalgo.Ranul.OpenAI.ObjectModels.ResponseModels.ImageResponseModel;

public record ImageCreateResponse : BaseResponse, IOpenAIModels.ICreatedAt
{
    [JsonPropertyName("data")]
    public List<ImageDataResult> Results { get; set; }

    [JsonPropertyName("created")]
    public long CreatedAtUnix { get; set; }  
    public DateTimeOffset CreatedAt => DateTimeOffset.FromUnixTimeSeconds(CreatedAtUnix);

    public record ImageDataResult
    {
        [JsonPropertyName("url")]
        public string Url { get; set; }

        [JsonPropertyName("b64_json")]
        public string B64 { get; set; }

        [JsonPropertyName("revised_prompt")]
        public string RevisedPrompt { get; set; }
    }
}