using System.Text.Json.Serialization;

namespace OpenAI.ObjectModels.ResponseModels;

public record AudioCreateTranscriptionResponse : BaseResponse
{
    [JsonPropertyName("text")] public string Text { get; set; }

    [JsonPropertyName("task")] public string Task { get; set; }

    [JsonPropertyName("language")] public string Language { get; set; }

    [JsonPropertyName("duration")] public float Duration { get; set; }

    [JsonPropertyName("words")] public List<WordSegment> Words { get; set; }

    [JsonPropertyName("segments")] public List<Segment> Segments { get; set; }
    
    public class WordSegment
    {
        [JsonPropertyName("word")] public string Word { get; set; }

        [JsonPropertyName("start")] public float Start { get; set; }

        [JsonPropertyName("end")] public float End { get; set; }
    }

    public class Segment
    {
        [JsonPropertyName("id")] public int Id { get; set; }

        [JsonPropertyName("seek")] public int Seek { get; set; }

        [JsonPropertyName("start")] public float Start { get; set; }

        [JsonPropertyName("end")] public float End { get; set; }

        [JsonPropertyName("text")] public string Text { get; set; }

        [JsonPropertyName("tokens")] public List<int> Tokens { get; set; }

        [JsonPropertyName("temperature")] public float Temperature { get; set; }

        [JsonPropertyName("avg_logprob")] public float Avglogprob { get; set; }

        [JsonPropertyName("compression_ratio")]
        public float CompressionRatio { get; set; }

        [JsonPropertyName("no_speech_prob")] public float Nospeechprob { get; set; }

        [JsonPropertyName("transient")] public bool Transient { get; set; }
    }
}