using System.Text.Json;
using System.Text.Json.Serialization;

namespace OpenAI.ObjectModels.ResponseModels;

public record BaseResponse
{
    [JsonPropertyName("object")] public string? ObjectTypeName { get; set; }
    public bool Successful => Error == null;
    [JsonPropertyName("error")] public Error? Error { get; set; }
}

//public record Error
//{
//    [JsonPropertyName("code")] public string? Code { get; set; }

//    [JsonPropertyName("message")] public object? MessageRaw { get; set; }
//    [JsonIgnore]
//    public List<string>? Messages
//    {
//        get
//        {
//            if (MessageRaw?.GetType() == typeof(string))
//            {
//                return new List<string> {MessageRaw.ToString()!};
//            }
//            return MessageRaw?.GetType() == typeof(List<string>) ? (List<string>) MessageRaw : null;
//        }
//    }
//    [JsonIgnore]
//    public string? Message
//    {
//        get
//        {
//            if (MessageRaw?.GetType() == typeof(string))
//            {
//                return MessageRaw.ToString();
//            }
//            return MessageRaw?.GetType() == typeof(List<string>) ? string.Join(Environment.NewLine,(List<string>) MessageRaw) : null;
//        }
//    }

//    [JsonPropertyName("param")] public string? Param { get; set; }

//    [JsonPropertyName("type")] public string? Type { get; set; }
//}

public class Error
{
    [JsonPropertyName("code")] public string? Code { get; set; }

    [JsonPropertyName("param")] public string? Param { get; set; }

    [JsonPropertyName("type")] public string? Type { get; set; }

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
                    Messages = new List<string?> {s};
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