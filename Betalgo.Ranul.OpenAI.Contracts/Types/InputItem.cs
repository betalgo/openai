using System.Text.Json;
using System.Text.Json.Serialization;
using Betalgo.Ranul.OpenAI.Contracts.Types.Items;

namespace Betalgo.Ranul.OpenAI.Contracts.Types;

/// <summary>
///     Base interface for all input item types in the Responses API.
///     <see href="https://platform.openai.com/docs/api-reference/responses">OpenAI API documentation</see>.
///     <see href="https://github.com/betalgo/openai/blob/master/Docs/openapi-split/components/schemas/inputitem.yml">
///         Source Definition
///     </see>
/// </summary>
[JsonConverter(typeof(InputItemConverter))]
public interface IInputItem
{
    /// <summary>
    ///     The type discriminator for the input item.
    /// </summary>
    [JsonPropertyName("type")]
    string Type { get; }
}

/// <summary>
///     JSON converter for <see cref="IInputItem" /> that handles polymorphic deserialization based on the type
///     discriminator.
/// </summary>
public class InputItemConverter : JsonConverter<IInputItem>
{
    /// <inheritdoc />
    public override IInputItem? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.StartObject)
            throw new JsonException("Expected StartObject token for InputItem");

        using var jsonDoc = JsonDocument.ParseValue(ref reader);
        if (!jsonDoc.RootElement.TryGetProperty("type", out var typeProp))
            throw new JsonException("Missing 'type' property in InputItem");

        var type = typeProp.GetString();
        var rawText = jsonDoc.RootElement.GetRawText();

        return type switch
        {
            "message" => JsonSerializer.Deserialize<InputMessageItem>(rawText, options),
            "item_reference" => JsonSerializer.Deserialize<ItemReferenceItem>(rawText, options),
            "function_call" => JsonSerializer.Deserialize<FunctionToolCallItem>(rawText, options),
            "file_search_call" => JsonSerializer.Deserialize<FileSearchToolCallItem>(rawText, options),
            "web_search_call" => JsonSerializer.Deserialize<WebSearchToolCallItem>(rawText, options),
            "reasoning" => JsonSerializer.Deserialize<ReasoningItem>(rawText, options),
            "computer_call_output" => JsonSerializer.Deserialize<ComputerCallOutputItemParam>(rawText, options),
            "mcp_approval_response" => JsonSerializer.Deserialize<MCPApprovalResponse>(rawText, options),
            "function_call_output" => JsonSerializer.Deserialize<FunctionCallOutputItemParam>(rawText, options),
            "local_shell_call" => JsonSerializer.Deserialize<LocalShellToolCall>(rawText, options),
            "shell_call" => JsonSerializer.Deserialize<FunctionShellCall>(rawText, options),
            "apply_patch_call" => JsonSerializer.Deserialize<ApplyPatchToolCall>(rawText, options),
            "custom_tool_call" => JsonSerializer.Deserialize<CustomToolCall>(rawText, options),
            _ => throw new JsonException($"Unknown InputItem type: {type}")
        };
    }

    /// <inheritdoc />
    public override void Write(Utf8JsonWriter writer, IInputItem value, JsonSerializerOptions options)
    {
        JsonSerializer.Serialize(writer, value, value.GetType(), options);
    }
}
