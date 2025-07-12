using System.Text.Json;
using System.Text.Json.Serialization;
using Betalgo.Ranul.OpenAI.Contracts.Enums;

namespace Betalgo.Ranul.OpenAI.ObjectModels.RequestModels;

public class ToolChoice
{
    public static ToolChoice None => new() { Type = ToolChoiceTypeEnum.None };
    public static ToolChoice Auto => new() { Type = ToolChoiceTypeEnum.Auto };
    public static ToolChoice Required => new() { Type = ToolChoiceTypeEnum.Required };

    /// <summary>
    ///     "none" is the default when no functions are present.  <br />
    ///     "auto" is the default if functions are present.  <br />
    ///     "function" has to be assigned if user Function is not null<br />
    ///     <br />
    /// </summary>
    [JsonPropertyName("type")]
    public ToolChoiceTypeEnum Type { get; set; }

    [JsonPropertyName("function")]
    public FunctionTool? Function { get; set; }

    public static ToolChoice FunctionChoice(string functionName)
    {
        return new()
        {
            Type = ToolChoiceTypeEnum.Function,
            Function = new()
            {
                Name = functionName
            }
        };
    }

    public class FunctionTool
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }
    }
}

[JsonConverter(typeof(ToolChoiceOneOfTypeConverter))]
public class ToolChoiceOneOfType
{
    public ToolChoiceOneOfType(string toolChoiceAsString)
    {
        AsString = toolChoiceAsString;
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
