using System.Text.Json;
using System.Text.Json.Serialization;

namespace Betalgo.Ranul.OpenAI.Contracts.Types;

[JsonConverter(typeof(FormFileOrListConverter))]
public class FormFileOrList
{
    public FormFileOrList(FormFile? value)
    {
        AsItem = value;
        AsList = null;
    }

    public FormFileOrList(List<FormFile>? value)
    {
        AsItem = null;
        AsList = value;
    }

    public FormFile? AsItem { get; set; }
    public List<FormFile>? AsList { get; set; }

    public static implicit operator FormFileOrList(FormFile? value) => new(value);
    public static implicit operator FormFileOrList(List<FormFile>? value) => new(value);

    public static implicit operator FormFile?(FormFileOrList format) => format.AsItem;
    public static implicit operator List<FormFile>?(FormFileOrList format) => format.AsList;
}

public class FormFile(string name, Stream data)
{
    public string Name { get; set; } = name;
    public Stream Data { get; set; } = data;
}

public class FormFileOrListConverter : JsonConverter<FormFileOrList>
{
    public override FormFileOrList? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return reader switch
        {
            { TokenType: JsonTokenType.StartObject } => new(JsonSerializer.Deserialize<FormFile>(ref reader, options)),
            { TokenType: JsonTokenType.StartArray } => new(JsonSerializer.Deserialize<List<FormFile>>(ref reader, options)),
            _ => throw new JsonException()
        };
    }

    public override void Write(Utf8JsonWriter writer, FormFileOrList? value, JsonSerializerOptions options)
    {
        if (value?.AsItem != null)
        {
            JsonSerializer.Serialize(writer, value.AsItem, options);
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