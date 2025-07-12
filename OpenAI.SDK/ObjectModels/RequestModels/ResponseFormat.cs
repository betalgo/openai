using System.Text.Json.Serialization;
using Betalgo.Ranul.OpenAI.Contracts.Enums;
using Betalgo.Ranul.OpenAI.ObjectModels.SharedModels;

namespace Betalgo.Ranul.OpenAI.ObjectModels.RequestModels;

/// <summary>
///     An object specifying the format that the model must output.
///     Used to enable JSON mode.
/// </summary>
public class ResponseFormat
{
    /// <summary>
    ///     Setting to json_object enables JSON mode.
    ///     This guarantees that the message the model generates is valid JSON.
    ///     Note that the message content may be partial if finish_reason="length",
    ///     which indicates the generation exceeded max_tokens or the conversation exceeded the max context length.
    /// </summary>

    [JsonPropertyName("type")]
    public ResponseFormatEnum? Type { get; set; }
    
    [JsonPropertyName("json_schema")]
    public JsonSchema JsonSchema { get; set; }
}

public class JsonSchema
{
    [JsonPropertyName("description")]
    public string? Description { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("strict")]
    public bool? Strict { get; set; }

    [JsonPropertyName("schema")]
    public PropertyDefinition? Schema { get; set; }
}

[JsonConverter(typeof(ResponseFormatOptionConverter))]
public class ResponseFormatOneOfType
{
    public ResponseFormatOneOfType()
    {
    }

    public ResponseFormatOneOfType(string asString)
    {
        AsString = asString;
    }

    public ResponseFormatOneOfType(ResponseFormat asObject)
    {
        AsObject = asObject;
    }

    [JsonIgnore]
    public string? AsString { get; set; }

    [JsonIgnore]
    public ResponseFormat? AsObject { get; set; }
}