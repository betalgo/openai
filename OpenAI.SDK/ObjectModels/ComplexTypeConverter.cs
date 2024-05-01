using System.Text.Json;
using System.Text.Json.Serialization;

namespace OpenAI.ObjectModels;

public class ComplexTypeConverter<T> : JsonConverter<T> where T : new()
{
    public override T? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var complexType = new T();
        foreach (var propertyInfo in typeToConvert.GetProperties())
        {
            try
            {
                propertyInfo.SetValue(complexType, JsonSerializer.Deserialize(ref reader, propertyInfo.PropertyType, options));
                break;
            }
            catch (Exception)
            {
                // ignored
            }
        }

        return complexType;
    }

    public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
    {
        var property = value?.GetType()
            .GetProperties()
            .FirstOrDefault(p => p.GetValue(value) != null);

        if (property != null)
        {
            JsonSerializer.Serialize(writer, property.GetValue(value), property.PropertyType, options);
        }
        else
        {
            writer.WriteNullValue();
        }
    }
}