using System.Text.Json;
using System.Text.Json.Serialization;

namespace Betalgo.Ranul.OpenAI.Contracts.Enums.Responses;

/// <summary>
///     The status of a code interpreter tool call.
///     <see href="https://platform.openai.com/docs/guides/code-interpreter">Code Interpreter Guide</see>.
///     <see href="https://github.com/betalgo/openai/blob/master/Docs/openapi-split/components/schemas/codeinterpretertoolcall.yml">
///         Source Definition
///     </see>
/// </summary>
[JsonConverter(typeof(Converter))]
public readonly struct CodeInterpreterCallStatus(string value) : IEquatable<CodeInterpreterCallStatus>
{
    /// <summary>
    ///     The code interpreter call is in progress.
    /// </summary>
    public static CodeInterpreterCallStatus InProgress { get; } = new("in_progress");

    /// <summary>
    ///     The code interpreter call has been completed successfully.
    /// </summary>
    public static CodeInterpreterCallStatus Completed { get; } = new("completed");

    /// <summary>
    ///     The code interpreter call was not completed.
    /// </summary>
    public static CodeInterpreterCallStatus Incomplete { get; } = new("incomplete");

    /// <summary>
    ///     The code interpreter is currently interpreting code.
    /// </summary>
    public static CodeInterpreterCallStatus Interpreting { get; } = new("interpreting");

    /// <summary>
    ///     The code interpreter call failed.
    /// </summary>
    public static CodeInterpreterCallStatus Failed { get; } = new("failed");

    /// <summary>
    ///     The underlying string value of the status.
    /// </summary>
    public string Value { get; } = value;

    /// <inheritdoc />
    public override string ToString() => Value;

    /// <inheritdoc />
    public bool Equals(CodeInterpreterCallStatus other) => string.Equals(Value, other.Value, StringComparison.OrdinalIgnoreCase);

    /// <inheritdoc />
    public override bool Equals(object? obj) => obj is CodeInterpreterCallStatus other && Equals(other);

    /// <inheritdoc />
    public override int GetHashCode() => StringComparer.OrdinalIgnoreCase.GetHashCode(Value);

    /// <summary>
    ///     Determines whether two <see cref="CodeInterpreterCallStatus" /> values are equal.
    /// </summary>
    public static bool operator ==(CodeInterpreterCallStatus left, CodeInterpreterCallStatus right) => left.Equals(right);

    /// <summary>
    ///     Determines whether two <see cref="CodeInterpreterCallStatus" /> values are not equal.
    /// </summary>
    public static bool operator !=(CodeInterpreterCallStatus left, CodeInterpreterCallStatus right) => !(left == right);

    /// <summary>
    ///     Implicitly converts a <see cref="CodeInterpreterCallStatus" /> to a string.
    /// </summary>
    public static implicit operator string(CodeInterpreterCallStatus status) => status.Value;

    /// <summary>
    ///     Implicitly converts a string to a <see cref="CodeInterpreterCallStatus" />.
    /// </summary>
    public static implicit operator CodeInterpreterCallStatus(string value) => new(value);

    /// <summary>
    ///     JSON converter for <see cref="CodeInterpreterCallStatus" />.
    /// </summary>
    public sealed class Converter : JsonConverter<CodeInterpreterCallStatus>
    {
        /// <inheritdoc />
        public override CodeInterpreterCallStatus Read(ref Utf8JsonReader reader, Type t, JsonSerializerOptions o) => new(reader.GetString()!);

        /// <inheritdoc />
        public override void Write(Utf8JsonWriter writer, CodeInterpreterCallStatus value, JsonSerializerOptions o) => writer.WriteStringValue(value.Value);
    }
}

