using System.Text.Json;
using System.Text.Json.Serialization;

namespace Betalgo.Ranul.OpenAI.Contracts.Types.Items;

/// <summary>
///     Base interface for code interpreter output types.
///     <see href="https://platform.openai.com/docs/guides/code-interpreter">Code Interpreter Guide</see>.
/// </summary>
[JsonConverter(typeof(CodeInterpreterOutputConverter))]
public interface ICodeInterpreterOutput
{
    /// <summary>
    ///     The type of the output.
    /// </summary>
    [JsonPropertyName("type")]
    string Type { get; }
}

/// <summary>
///     The logs output from the code interpreter.
///     <see href="https://github.com/betalgo/openai/blob/master/Docs/openapi-split/components/schemas/codeinterpreteroutputlogs.yml">
///         Source Definition
///     </see>
/// </summary>
public class CodeInterpreterOutputLogs : ICodeInterpreterOutput
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="CodeInterpreterOutputLogs" /> class.
    /// </summary>
    public CodeInterpreterOutputLogs()
    {
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="CodeInterpreterOutputLogs" /> class with required parameters.
    /// </summary>
    /// <param name="logs">The logs output from the code interpreter.</param>
    public CodeInterpreterOutputLogs(string logs)
    {
        Logs = logs;
    }

    /// <summary>
    ///     The type of the output. Always <c>logs</c>.
    /// </summary>
    [JsonPropertyName("type")]
    public string Type => "logs";

    /// <summary>
    ///     The logs output from the code interpreter.
    /// </summary>
    [JsonPropertyName("logs")]
    public string Logs { get; set; } = null!;
}

/// <summary>
///     The image output from the code interpreter.
///     <see href="https://github.com/betalgo/openai/blob/master/Docs/openapi-split/components/schemas/codeinterpreteroutputimage.yml">
///         Source Definition
///     </see>
/// </summary>
public class CodeInterpreterOutputImage : ICodeInterpreterOutput
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="CodeInterpreterOutputImage" /> class.
    /// </summary>
    public CodeInterpreterOutputImage()
    {
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="CodeInterpreterOutputImage" /> class with required parameters.
    /// </summary>
    /// <param name="url">The URL of the image output from the code interpreter.</param>
    public CodeInterpreterOutputImage(string url)
    {
        Url = url;
    }

    /// <summary>
    ///     The type of the output. Always <c>image</c>.
    /// </summary>
    [JsonPropertyName("type")]
    public string Type => "image";

    /// <summary>
    ///     The URL of the image output from the code interpreter.
    /// </summary>
    [JsonPropertyName("url")]
    public string Url { get; set; } = null!;
}

/// <summary>
///     JSON converter for <see cref="ICodeInterpreterOutput" /> that handles polymorphic deserialization.
/// </summary>
public class CodeInterpreterOutputConverter : JsonConverter<ICodeInterpreterOutput>
{
    /// <inheritdoc />
    public override ICodeInterpreterOutput? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.StartObject)
            throw new JsonException("Expected StartObject token for CodeInterpreterOutput");

        using var jsonDoc = JsonDocument.ParseValue(ref reader);
        if (!jsonDoc.RootElement.TryGetProperty("type", out var typeProp))
            throw new JsonException("Missing 'type' property in CodeInterpreterOutput");

        var type = typeProp.GetString();
        var rawText = jsonDoc.RootElement.GetRawText();

        return type switch
        {
            "logs" => JsonSerializer.Deserialize<CodeInterpreterOutputLogs>(rawText, options),
            "image" => JsonSerializer.Deserialize<CodeInterpreterOutputImage>(rawText, options),
            _ => throw new JsonException($"Unknown CodeInterpreterOutput type: {type}")
        };
    }

    /// <inheritdoc />
    public override void Write(Utf8JsonWriter writer, ICodeInterpreterOutput value, JsonSerializerOptions options)
    {
        JsonSerializer.Serialize(writer, value, value.GetType(), options);
    }
}

