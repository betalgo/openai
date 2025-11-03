using System.Text.Json;
using System.Text.Json.Serialization;

namespace Betalgo.Ranul.OpenAI.Contracts.Responses.Base;

public class ResponseError
{
    [JsonPropertyName("code")]
    public string? Code { get; set; }

    [JsonPropertyName("param")]
    public string? Param { get; set; }

    [JsonPropertyName("type")]
    public string? Type { get; set; }

    [JsonPropertyName("line")]
    public int? Line { get; set; }

    [JsonIgnore]
    public string? Message { get; private set; }

    [JsonIgnore]
    public List<string?>? Messages { get; private set; }

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
                    Messages = [s];
                    break;
                case List<object> list when list.All(i => i is JsonElement):
                    Messages = list.Cast<JsonElement>().Select(e => e.GetString()).ToList();
                    Message = string.Join(Environment.NewLine, Messages);
                    break;
            }
        }
    }

    /// <summary>
    ///     The event_id of the client event that caused the error, if applicable.
    /// </summary>
    [JsonPropertyName("event_id")]
    public string? EventId { get; set; }

    public class MessageConverter : JsonConverter<object>
    {
        public override object? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return reader switch
            {
                { TokenType: JsonTokenType.String } => reader.GetString(),
                { TokenType: JsonTokenType.StartArray } => JsonSerializer.Deserialize<List<object>>(ref reader, options),
                _ => throw new JsonException()
            };
        }

        public override void Write(Utf8JsonWriter writer, object value, JsonSerializerOptions options)
        {
            JsonSerializer.Serialize(writer, value, options);
        }
    }
}