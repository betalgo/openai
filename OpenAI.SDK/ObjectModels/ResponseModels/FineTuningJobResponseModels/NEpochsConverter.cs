using System.Text.Json;
using System.Text.Json.Serialization;

namespace OpenAI.ObjectModels.ResponseModels.FineTuningJobResponseModels;

public class NEpochsConverter : JsonConverter<int?>
{
    public override int? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        switch (reader.TokenType)
        {
            case JsonTokenType.String:
            {
                var stringValue = reader.GetString();
                if (stringValue?.ToLower() == "auto")
                    return -1;
                break;
            }
            case JsonTokenType.Number:
                return reader.GetInt32();
            case JsonTokenType.Null:
                return null;
        }

        throw new JsonException($"Unexpected token type: {reader.TokenType}");
    }

    public override void Write(Utf8JsonWriter writer, int? value, JsonSerializerOptions options)
    {
        if (value.HasValue)
        {
            if (value.Value == -1)
                writer.WriteStringValue("auto");
            else
                writer.WriteNumberValue(value.Value);
        }
        else
        {
            writer.WriteNullValue();
        }
    }
}