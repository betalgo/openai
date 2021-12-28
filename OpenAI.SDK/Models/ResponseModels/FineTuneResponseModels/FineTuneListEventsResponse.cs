using System.Text.Json.Serialization;
using OpenAI.GPT3.Models.SharedModels;

namespace OpenAI.GPT3.Models.ResponseModels.FineTuneResponseModels;

public record FineTuneListEventsResponse : BaseResponse
{
    [JsonPropertyName("data")] public List<EventResponse> Data { get; set; }
}