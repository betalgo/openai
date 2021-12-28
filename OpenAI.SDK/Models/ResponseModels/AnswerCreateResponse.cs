using System.Text.Json.Serialization;

namespace OpenAI.GPT3.Models.ResponseModels
{
    public record AnswerCreateResponse : BaseResponse
    {
        [JsonPropertyName("Answers")] public List<string> Answers { get; set; }

        [JsonPropertyName("completion")] public string Completion { get; set; }

        [JsonPropertyName("model")] public string Model { get; set; }

        [JsonPropertyName("search_model")] public string SearchModel { get; set; }

        [JsonPropertyName("selected_documents")]
        public SelectedDocument[] SelectedDocuments { get; set; }
    }

    public class SelectedDocument
    {
        [JsonPropertyName("document")] public int Document { get; set; }

        [JsonPropertyName("text")] public string Text { get; set; }
    }
}