using System.Text.Json;
using System.Text.Json.Serialization;

namespace Betalgo.Ranul.OpenAI.Contracts.Requests.Responses;

/// <summary>
///     The conversation that this response belongs to. Items from this conversation are prepended to input items
///     for this response request. Input items and output items from this response are automatically added to
///     this conversation after this response completes.
///     <see href="https://platform.openai.com/docs/api-reference/responses/create">OpenAI API documentation</see>.
///     <see href="https://github.com/betalgo/openai/blob/master/Docs/openapi-split/components/schemas/conversationparam.yml">
///         Source Definition
///     </see>
/// </summary>
[JsonConverter(typeof(ConversationParamConverter))]
public class ConversationParam
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="ConversationParam" /> class.
    /// </summary>
    public ConversationParam()
    {
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="ConversationParam" /> class with a conversation ID.
    /// </summary>
    /// <param name="id">The unique ID of the conversation.</param>
    public ConversationParam(string id)
    {
        Id = id;
    }

    /// <summary>
    ///     The unique ID of the conversation.
    /// </summary>
    [JsonPropertyName("id")]
    public string Id { get; set; } = null!;

    /// <summary>
    ///     Implicitly converts a string to a <see cref="ConversationParam" />.
    /// </summary>
    public static implicit operator ConversationParam(string id) => new(id);
}

/// <summary>
///     JSON converter for <see cref="ConversationParam" /> to handle both string and object formats.
/// </summary>
public class ConversationParamConverter : JsonConverter<ConversationParam>
{
    /// <inheritdoc />
    public override ConversationParam? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.String)
        {
            return new ConversationParam(reader.GetString()!);
        }

        if (reader.TokenType == JsonTokenType.StartObject)
        {
            using var doc = JsonDocument.ParseValue(ref reader);
            var id = doc.RootElement.GetProperty("id").GetString()!;
            return new ConversationParam(id);
        }

        throw new JsonException("Expected string or object for ConversationParam");
    }

    /// <inheritdoc />
    public override void Write(Utf8JsonWriter writer, ConversationParam value, JsonSerializerOptions options)
    {
        // Always serialize as an object format
        writer.WriteStartObject();
        writer.WriteString("id", value.Id);
        writer.WriteEndObject();
    }
}

