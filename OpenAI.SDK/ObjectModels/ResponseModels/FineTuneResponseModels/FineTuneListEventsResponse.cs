using System.Text.Json.Serialization;
using Betalgo.Ranul.OpenAI.ObjectModels.SharedModels;

namespace Betalgo.Ranul.OpenAI.ObjectModels.ResponseModels.FineTuneResponseModels;

public record FineTuneListEventsResponse : BaseResponse
{
    [JsonPropertyName("data")]
    public List<EventResponse> Data { get; set; }
}