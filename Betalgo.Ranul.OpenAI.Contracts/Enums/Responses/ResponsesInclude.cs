using System.Text.Json;
using System.Text.Json.Serialization;

namespace Betalgo.Ranul.OpenAI.Contracts.Enums.Responses;

/// <summary>
///     Specifies additional output data to include in the model response.
///     <see href="https://platform.openai.com/docs/api-reference/responses/create">OpenAI API documentation</see>.
///     <see href="https://github.com/betalgo/openai/blob/master/Docs/openapi-split/components/schemas/includeenum.yml">
///         Source Definition
///     </see>
/// </summary>
[JsonConverter(typeof(Converter))]
public readonly struct ResponsesInclude(string value) : IEquatable<ResponsesInclude>
{
    /// <summary>
    ///     Include the results of the file search tool call.
    /// </summary>
    public static ResponsesInclude FileSearchCallResults { get; } = new("file_search_call.results");

    /// <summary>
    ///     Include the results of the web search tool call.
    ///     <b>Deprecated:</b> Use <see cref="WebSearchCallActionSources" /> instead.
    /// </summary>
    [Obsolete("Use WebSearchCallActionSources instead. This value is not in the current OpenAPI schema.")]
    public static ResponsesInclude WebSearchCallResults { get; } = new("web_search_call.results");

    /// <summary>
    ///     Include the sources of the web search tool call action.
    /// </summary>
    public static ResponsesInclude WebSearchCallActionSources { get; } = new("web_search_call.action.sources");

    /// <summary>
    ///     Include image URLs from the input message.
    /// </summary>
    public static ResponsesInclude MessageInputImageImageUrl { get; } = new("message.input_image.image_url");

    /// <summary>
    ///     Include image URLs from the computer call output.
    /// </summary>
    public static ResponsesInclude ComputerCallOutputImageUrl { get; } = new("computer_call_output.output.image_url");

    /// <summary>
    ///     Include the outputs of Python code execution in code interpreter tool call items.
    /// </summary>
    public static ResponsesInclude CodeInterpreterCallOutputs { get; } = new("code_interpreter_call.outputs");

    /// <summary>
    ///     Include an encrypted version of reasoning tokens in reasoning item outputs.
    ///     This enables reasoning items to be used in multi-turn conversations when using the Responses API statelessly.
    /// </summary>
    public static ResponsesInclude ReasoningEncryptedContent { get; } = new("reasoning.encrypted_content");

    /// <summary>
    ///     Include logprobs with assistant messages.
    /// </summary>
    public static ResponsesInclude MessageOutputTextLogprobs { get; } = new("message.output_text.logprobs");

    /// <summary>
    ///     The underlying string value of the include option.
    /// </summary>
    public string Value { get; } = value;

    /// <inheritdoc />
    public override string ToString() => Value;

    /// <inheritdoc />
    public bool Equals(ResponsesInclude other) => string.Equals(Value, other.Value, StringComparison.OrdinalIgnoreCase);

    /// <inheritdoc />
    public override bool Equals(object? obj) => obj is ResponsesInclude other && Equals(other);

    /// <inheritdoc />
    public override int GetHashCode() => StringComparer.OrdinalIgnoreCase.GetHashCode(Value);

    /// <summary>
    ///     Determines whether two <see cref="ResponsesInclude" /> values are equal.
    /// </summary>
    public static bool operator ==(ResponsesInclude left, ResponsesInclude right) => left.Equals(right);

    /// <summary>
    ///     Determines whether two <see cref="ResponsesInclude" /> values are not equal.
    /// </summary>
    public static bool operator !=(ResponsesInclude left, ResponsesInclude right) => !(left == right);

    /// <summary>
    ///     Implicitly converts a <see cref="ResponsesInclude" /> to a string.
    /// </summary>
    public static implicit operator string(ResponsesInclude include) => include.Value;

    /// <summary>
    ///     Implicitly converts a string to a <see cref="ResponsesInclude" />.
    /// </summary>
    public static implicit operator ResponsesInclude(string value) => new(value);

    /// <summary>
    ///     JSON converter for <see cref="ResponsesInclude" />.
    /// </summary>
    public sealed class Converter : JsonConverter<ResponsesInclude>
    {
        /// <inheritdoc />
        public override ResponsesInclude Read(ref Utf8JsonReader reader, Type t, JsonSerializerOptions o) => new(reader.GetString()!);

        /// <inheritdoc />
        public override void Write(Utf8JsonWriter writer, ResponsesInclude value, JsonSerializerOptions o) => writer.WriteStringValue(value.Value);
    }
}

