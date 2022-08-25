using System.Collections;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace OpenAI.GPT3.ObjectModels
{
    /// <inheritdoc />
    public class SingleOrArrayConverter<TItem> : SingleOrArrayConverter<List<TItem>, TItem>
    {
        /// <inheritdoc />
        public SingleOrArrayConverter() : this(true)
        {
        }

        /// <inheritdoc />
        public SingleOrArrayConverter(bool canWrite) : base(canWrite)
        {
        }
    }

    /// <inheritdoc />
    public class SingleOrArrayConverterFactory : JsonConverterFactory
    {
        /// <inheritdoc />
        public SingleOrArrayConverterFactory() : this(true)
        {
        }

        /// <inheritdoc />
        public SingleOrArrayConverterFactory(bool canWrite)
        {
            CanWrite = canWrite;
        }

        public bool CanWrite { get; }

        /// <inheritdoc />
        public override bool CanConvert(Type typeToConvert)
        {
            var itemType = GetItemType(typeToConvert);

            if (itemType == null)
                return false;
            
            if (itemType != typeof(string) && typeof(IEnumerable).IsAssignableFrom(itemType))
                return false;

            if (typeToConvert.GetConstructor(Type.EmptyTypes) == null || typeToConvert.IsValueType)
                return false;

            return true;
        }

        /// <inheritdoc />
        public override JsonConverter? CreateConverter(Type typeToConvert, JsonSerializerOptions options)
        {
            var itemType = GetItemType(typeToConvert);
            var converterType = typeof(SingleOrArrayConverter<,>).MakeGenericType(typeToConvert, itemType);
            return (JsonConverter?)Activator.CreateInstance(converterType, new object[] { CanWrite });
        }

        private static Type? GetItemType(Type? type)
        {
            // Quick reject for performance
            if (type == null || type.IsPrimitive || type.IsArray || type == typeof(string))
                return null;
            while (type != null)
            {
                if (type.IsGenericType)
                {
                    var genType = type.GetGenericTypeDefinition();
                    if (genType == typeof(List<>))
                        return type.GetGenericArguments()[0];
                    // Add here other generic collection types as required, e.g. HashSet<> or ObservableCollection<> or etc.
                }

                type = type.BaseType;
            }

            return null;
        }
    }

    /// <inheritdoc />
    public class SingleOrArrayConverter<TCollection, TItem> : JsonConverter<TCollection> where TCollection : class, ICollection<TItem>, new()
    {
        /// <inheritdoc />
        public SingleOrArrayConverter() : this(true)
        {
        }

        /// <inheritdoc />
        public SingleOrArrayConverter(bool canWrite)
        {
            CanWrite = canWrite;
        }

        public bool CanWrite { get; }

        /// <inheritdoc />
        public override TCollection? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions? options)
        {
            switch (reader.TokenType)
            {
                case JsonTokenType.Null:
                    return null;
                case JsonTokenType.StartArray:
                    var list = new TCollection();
                    while (reader.Read())
                    {
                        if (reader.TokenType == JsonTokenType.EndArray)
                            break;
                        list.Add(JsonSerializer.Deserialize<TItem>(ref reader, options)!);
                    }

                    return list;
                default:
                    return new TCollection {JsonSerializer.Deserialize<TItem>(ref reader, options)!};
            }
        }

        /// <inheritdoc />
        public override void Write(Utf8JsonWriter writer, TCollection value, JsonSerializerOptions options)
        {
            if (CanWrite && value.Count == 1)
            {
                JsonSerializer.Serialize(writer, value.First(), options);
            }
            else
            {
                writer.WriteStartArray();
                foreach (var item in value)
                    JsonSerializer.Serialize(writer, item, options);
                writer.WriteEndArray();
            }
        }
    }
}