using System.Collections;
using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace OpenAI.ObjectModels.ResponseModels;

public record BaseResponse
{
    [JsonPropertyName("object")] public string? ObjectTypeName { get; set; }
    public bool Successful => Error == null;
    [JsonPropertyName("error")] public Error? Error { get; set; }
    public HttpStatusCode HttpStatusCode { get; set; }
    public ResponseHeaderValues? HeaderValues { get; set; }
}

public record RateLimitInfo
{
    public string? LimitRequests { get; set; }
    public string? LimitTokens { get; set; }
    public string? LimitTokensUsageBased { get; set; }
    public string? RemainingRequests { get; set; }
    public string? RemainingTokens { get; set; }
    public string? RemainingTokensUsageBased { get; set; }
    public string? ResetRequests { get; set; }
    public string? ResetTokens { get; set; }
    public string? ResetTokensUsageBased { get; set; }
}

public record OpenAIInfo
{
    public string? Model { get; set; }
    public string? Organization { get; set; }
    public string? ProcessingMs { get; set; }
    public string? Version { get; set; }
}

public record ResponseHeaderValues
{
    public DateTimeOffset? Date { get; set; }
    public string? Connection { get; set; }
    public string? AccessControlAllowOrigin { get; set; }
    public string? CacheControl { get; set; }
    public string? Vary { get; set; }
    public string? XRequestId { get; set; }
    public string? StrictTransportSecurity { get; set; }
    public string? CFCacheStatus { get; set; }
    public List<string>? SetCookie { get; set; }
    public string? Server { get; set; }
    public string? CF_RAY { get; set; }
    public string? AltSvc { get; set; }
    public Dictionary<string, IEnumerable<string>>? All { get; set; }
    public RateLimitInfo? RateLimits { get; set; }
    public OpenAIInfo? OpenAI { get; set; }
}

public record DataWithPagingBaseResponse<T> : BaseResponse where T :IList
{
    [JsonPropertyName("data")] public T? Data { get; set; }

    [JsonPropertyName("first_id")]
    public string FirstId { get; set; }

    [JsonPropertyName("last_id")]
    public string LastId { get; set; }

    [JsonPropertyName("has_more")]
    public bool HasMore { get; set; }
}
public record DataBaseResponse<T> : BaseResponse
{
    [JsonPropertyName("data")] public T? Data { get; set; }
}

public record ErrorList: DataBaseResponse<List<Error>>
{
}
public class Error
{
    [JsonPropertyName("code")] public string? Code { get; set; }

    [JsonPropertyName("param")] public string? Param { get; set; }

    [JsonPropertyName("type")] public string? Type { get; set; }

    [JsonPropertyName("line")]
    public int? Line { get; set; }
    [JsonIgnore] public string? Message { get; private set; }

    [JsonIgnore] public List<string?> Messages { get; private set; }

    [JsonPropertyName("message")]
    [JsonConverter(typeof(MessageConverter))]
    public object MessageObject
    {
        set
        {
            switch (value)
            {
                case string s:
                    Message = s;
                    Messages = new() { s };
                    break;
                case List<object> list when list.All(i => i is JsonElement):
                    Messages = list.Cast<JsonElement>().Select(e => e.GetString()).ToList();
                    Message = string.Join(Environment.NewLine, Messages);
                    break;
            }
        }
    }

    public class MessageConverter : JsonConverter<object>
    {
        public override object? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.String)
            {
                return reader.GetString();
            }

            if (reader.TokenType == JsonTokenType.StartArray)
            {
                return JsonSerializer.Deserialize<List<object>>(ref reader, options);
            }

            throw new JsonException();
        }

        public override void Write(Utf8JsonWriter writer, object value, JsonSerializerOptions options)
        {
            JsonSerializer.Serialize(writer, value, options);
        }
    }
}