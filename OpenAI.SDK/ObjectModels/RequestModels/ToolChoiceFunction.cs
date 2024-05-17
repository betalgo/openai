using System.Text.Json;
using System.Text.Json.Serialization;

namespace OpenAI.ObjectModels.RequestModels;

public class ToolChoice
{
    public static ToolChoice None => new() { Type = StaticValues.CompletionStatics.ToolChoiceType.None };
    public static ToolChoice Auto => new() { Type = StaticValues.CompletionStatics.ToolChoiceType.Auto };
    public static ToolChoice FunctionChoice(string functionName) => new()
    {
        Type = StaticValues.CompletionStatics.ToolChoiceType.Function,
        Function = new FunctionTool()
        {
            Name = functionName
        }
    };

    /// <summary>
    ///     "none" is the default when no functions are present.  <br />
    ///     "auto" is the default if functions are present.  <br />
    ///     "function" has to be assigned if user Function is not null<br />
    ///     <br />
    ///     Check <see cref="StaticValues.CompletionStatics.ToolChoiceType" /> for possible values.
    /// </summary>
    [JsonPropertyName("type")]
    public string Type { get; set; }

    [JsonPropertyName("function")] public FunctionTool? Function { get; set; }

    public class FunctionTool
    {
        [JsonPropertyName("name")] public string Name { get; set; }
    }
}


[JsonConverter(typeof(ToolChoiceOneOfTypeConverter))]
public class ToolChoiceOneOfType
{
    public ToolChoiceOneOfType(string toolChoiceAsString)
    {
        AsString =  toolChoiceAsString;
    }

    public ToolChoiceOneOfType(ToolChoice toolChoiceAsObject)
    {
        AsObject = toolChoiceAsObject;
    }

    public ToolChoiceOneOfType()
    {
    }

    [JsonIgnore]
    public string? AsString { get; set; }

    [JsonIgnore]
    public ToolChoice? AsObject { get; set; }

    public class ToolChoiceOneOfTypeConverter : JsonConverter<ToolChoiceOneOfType>
    {
        public override ToolChoiceOneOfType? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return reader.TokenType switch
            {
                JsonTokenType.String => new() { AsString = reader.GetString() },
                JsonTokenType.StartArray => new() { AsObject = JsonSerializer.Deserialize<ToolChoice>(ref reader, options) },
                _ => throw new JsonException()
            };
        }

        public override void Write(Utf8JsonWriter writer, ToolChoiceOneOfType? value, JsonSerializerOptions options)
        {
            if (value?.AsString != null)
            {
                writer.WriteStringValue(value.AsString);
            }
            else if (value?.AsObject != null)
            {
                JsonSerializer.Serialize(writer, value.AsObject, options);
            }
            else
            {
                writer.WriteNullValue();
            }
        }
    }
}

