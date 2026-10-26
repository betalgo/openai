using System.Text.Json;
using System.Text.Json.Serialization;

namespace Betalgo.Ranul.OpenAI.Contracts.Types.Tools;

/// <summary>
///     Base interface for all tool types in the Responses API.
///     <see href="https://platform.openai.com/docs/guides/tools">OpenAI Tools Guide</see>.
///     <see href="https://github.com/betalgo/openai/blob/master/Docs/openapi-split/components/schemas/tool.yml">
///         Source Definition
///     </see>
/// </summary>
[JsonConverter(typeof(ToolConverter))]
public interface ITool
{
    /// <summary>
    ///     The type discriminator for the tool.
    /// </summary>
    [JsonPropertyName("type")]
    string Type { get; }
}

/// <summary>
///     JSON converter for <see cref="ITool" /> that handles polymorphic deserialization.
///     Also handles <see cref="RawTool" /> for zero-cost pass-through serialization.
/// </summary>
public class ToolConverter : JsonConverter<ITool>
{
    /// <inheritdoc />
    public override ITool? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.StartObject)
            throw new JsonException("Expected StartObject token for Tool");

        using var jsonDoc = JsonDocument.ParseValue(ref reader);
        if (!jsonDoc.RootElement.TryGetProperty("type", out var typeProp))
            throw new JsonException("Missing 'type' property in Tool");

        var type = typeProp.GetString();
        var rawText = jsonDoc.RootElement.GetRawText();

        return type switch
        {
            "function" => JsonSerializer.Deserialize<FunctionTool>(rawText, options),
            "web_search" or "web_search_2025_08_26" or "web_search_preview" or "web_search_preview_2025_03_11" => JsonSerializer.Deserialize<WebSearchTool>(rawText, options),
            "file_search" => JsonSerializer.Deserialize<FileSearchTool>(rawText, options),
            "code_interpreter" => JsonSerializer.Deserialize<CodeInterpreterTool>(rawText, options),
            "image_generation" => JsonSerializer.Deserialize<ImageGenerationTool>(rawText, options),
            "computer_use_preview" => JsonSerializer.Deserialize<ComputerUsePreviewTool>(rawText, options),
            "mcp" => JsonSerializer.Deserialize<MCPTool>(rawText, options),
            "local_shell" => JsonSerializer.Deserialize<LocalShellTool>(rawText, options),
            "custom" => JsonSerializer.Deserialize<CustomTool>(rawText, options),
            "apply_patch" => JsonSerializer.Deserialize<ApplyPatchTool>(rawText, options),
            _ => new RawTool(rawText)
        };
    }

    /// <inheritdoc />
    public override void Write(Utf8JsonWriter writer, ITool value, JsonSerializerOptions options)
    {
        // Special handling for RawTool - zero-cost pass-through
        if (value is RawTool rawTool)
        {
            writer.WriteRawValue(rawTool.RawJson);
            return;
        }

        JsonSerializer.Serialize(writer, value, value.GetType(), options);
    }
}

