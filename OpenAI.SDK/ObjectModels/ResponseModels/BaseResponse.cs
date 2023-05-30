using System.Text.Json.Serialization;

namespace OpenAI.ObjectModels.ResponseModels;

public record BaseResponse
{
    [JsonPropertyName("object")] public string? ObjectTypeName { get; set; }
    public bool Successful => Error == null;
    [JsonPropertyName("error")] public Error? Error { get; set; }
}

public record Error
{
    [JsonPropertyName("code")] public string? Code { get; set; }

    [JsonPropertyName("message")] public string? Message { get; set; }

    [JsonPropertyName("param")] public string? Param { get; set; }

    [JsonPropertyName("type")] public string? Type { get; set; }
}