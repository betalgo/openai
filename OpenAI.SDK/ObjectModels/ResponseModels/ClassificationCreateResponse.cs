using System.Text.Json.Serialization;

namespace OpenAI.GPT3.ObjectModels.ResponseModels
{
    public record ClassificationCreateResponse : BaseResponse
    {
        [JsonPropertyName("completion")] public string Completion { get; set; }

        [JsonPropertyName("label")] public string Label { get; set; }

        [JsonPropertyName("model")] public string Model { get; set; }

        [JsonPropertyName("search_model")] public string SearchModel { get; set; }

        [JsonPropertyName("selected_examples")]
        public SelectedExamples[] SelectedExamples { get; set; }
    }

    public class SelectedExamples
    {
        [JsonPropertyName("document")] public int Document { get; set; }

        [JsonPropertyName("label")] public string Label { get; set; }

        [JsonPropertyName("text")] public string Text { get; set; }
    }
}