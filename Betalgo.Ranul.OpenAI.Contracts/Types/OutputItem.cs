using System.Text.Json;
using System.Text.Json.Serialization;
using Betalgo.Ranul.OpenAI.Contracts.Types.Items;

namespace Betalgo.Ranul.OpenAI.Contracts.Types;

/// <summary>
///     Base interface for all output item types in the Responses API.
///     <see href="https://platform.openai.com/docs/api-reference/responses">OpenAI API documentation</see>.
///     <see href="https://github.com/betalgo/openai/blob/master/Docs/openapi-split/components/schemas/outputitem.yml">
///         Source Definition
///     </see>
/// </summary>
[JsonConverter(typeof(OutputItemConverter))]
public interface IOutputItem
{
    /// <summary>
    ///     The type discriminator for the output item.
    /// </summary>
    [JsonPropertyName("type")]
    string Type { get; }
}

/// <summary>
///     JSON converter for <see cref="IOutputItem" /> that handles polymorphic deserialization based on the type
///     discriminator.
/// </summary>
public class OutputItemConverter : JsonConverter<IOutputItem>
{
    /// <inheritdoc />
    public override IOutputItem? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.StartObject)
            throw new JsonException("Expected StartObject token for OutputItem");

        using var jsonDoc = JsonDocument.ParseValue(ref reader);
        if (!jsonDoc.RootElement.TryGetProperty("type", out var typeProp))
            throw new JsonException("Missing 'type' property in OutputItem");

        var type = typeProp.GetString();
        var rawText = jsonDoc.RootElement.GetRawText();

        return type switch
        {
            "message" => JsonSerializer.Deserialize<OutputMessageItem>(rawText, options),
            "function_call" => JsonSerializer.Deserialize<FunctionToolCallItem>(rawText, options),
            "file_search_call" => JsonSerializer.Deserialize<FileSearchToolCallItem>(rawText, options),
            "web_search_call" => JsonSerializer.Deserialize<WebSearchToolCallItem>(rawText, options),
            "reasoning" => JsonSerializer.Deserialize<ReasoningItem>(rawText, options),
            "computer_call" => JsonSerializer.Deserialize<ComputerToolCall>(rawText, options),
            "code_interpreter_call" => JsonSerializer.Deserialize<CodeInterpreterToolCall>(rawText, options),
            "mcp_call" => JsonSerializer.Deserialize<MCPToolCall>(rawText, options),
            "mcp_list_tools" => JsonSerializer.Deserialize<MCPListTools>(rawText, options),
            "mcp_approval_request" => JsonSerializer.Deserialize<MCPApprovalRequest>(rawText, options),
            "image_generation_call" => JsonSerializer.Deserialize<ImageGenToolCall>(rawText, options),
            "local_shell_call" => JsonSerializer.Deserialize<LocalShellToolCall>(rawText, options),
            "shell_call" => JsonSerializer.Deserialize<FunctionShellCall>(rawText, options),
            "apply_patch_call" => JsonSerializer.Deserialize<ApplyPatchToolCall>(rawText, options),
            "custom_tool_call" => JsonSerializer.Deserialize<CustomToolCall>(rawText, options),
            _ => throw new JsonException($"Unknown OutputItem type: {type}")
        };
    }

    /// <inheritdoc />
    public override void Write(Utf8JsonWriter writer, IOutputItem value, JsonSerializerOptions options)
    {
        JsonSerializer.Serialize(writer, value, value.GetType(), options);
    }
}
