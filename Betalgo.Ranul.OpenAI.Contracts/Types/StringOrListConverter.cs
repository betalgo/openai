using System.Text.Json;
using System.Text.Json.Serialization;

namespace Betalgo.Ranul.OpenAI.Contracts.Types;


[JsonConverter(typeof(StringOrListConverter))]
public class StringOrList
{
    public StringOrList(string? value)
    {
        AsString = value;
        AsList = null;
    }

    public StringOrList(List<string>? value)
    {
        AsString = null;
        AsList = value;
    }

    public string? AsString { get; set; }
    public List<string>? AsList { get; set; }

    public static implicit operator StringOrList(string? value) => new(value);
    public static implicit operator StringOrList(List<string>? value) => new(value);

    public static implicit operator string?(StringOrList format) => format.AsString;
    public static implicit operator List<string>?(StringOrList format) => format.AsList;
}

public class StringOrListConverter : JsonConverter<StringOrList>
{
    public override StringOrList? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return reader switch
        {
            { TokenType: JsonTokenType.String } => new(reader.GetString()),
            { TokenType: JsonTokenType.StartArray } => new(JsonSerializer.Deserialize<List<string>>(ref reader, options)),
            _ => throw new JsonException()
        };
    }

    public override void Write(Utf8JsonWriter writer, StringOrList? value, JsonSerializerOptions options)
    {
        if (value?.AsString != null)
        {
            writer.WriteStringValue(value.AsString);
        }
        else if (value?.AsList != null)
        {
            JsonSerializer.Serialize(writer, value.AsList, options);
        }
        else
        {
            writer.WriteNullValue();
        }
    }
}