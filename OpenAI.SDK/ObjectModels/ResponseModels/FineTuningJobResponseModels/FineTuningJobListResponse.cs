using System.Text.Json.Serialization;

namespace OpenAI.ObjectModels.ResponseModels.FineTuningJobResponseModels;

public record FineTuningJobListResponse : BaseResponse
{
    [JsonPropertyName("data")]
    public List<FineTuningJobResponse> Data { get; set; }

    [JsonPropertyName("has_more")]
    public bool HasMore { get; set; }
}

