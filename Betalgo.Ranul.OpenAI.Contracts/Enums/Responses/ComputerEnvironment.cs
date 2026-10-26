using System.Text.Json;
using System.Text.Json.Serialization;

namespace Betalgo.Ranul.OpenAI.Contracts.Enums.Responses;

/// <summary>
///     The environment for the computer use tool.
///     <see href="https://platform.openai.com/docs/guides/tools-computer-use">Computer Use Guide</see>.
///     <see href="https://github.com/betalgo/openai/blob/master/Docs/openapi-split/components/schemas/computerenvironment.yml">
///         Source Definition
///     </see>
/// </summary>
[JsonConverter(typeof(Converter))]
public readonly struct ComputerEnvironment(string value) : IEquatable<ComputerEnvironment>
{
    /// <summary>
    ///     Windows operating system environment.
    /// </summary>
    public static ComputerEnvironment Windows { get; } = new("windows");

    /// <summary>
    ///     Mac operating system environment.
    /// </summary>
    public static ComputerEnvironment Mac { get; } = new("mac");

    /// <summary>
    ///     Linux operating system environment.
    /// </summary>
    public static ComputerEnvironment Linux { get; } = new("linux");

    /// <summary>
    ///     Ubuntu operating system environment.
    /// </summary>
    public static ComputerEnvironment Ubuntu { get; } = new("ubuntu");

    /// <summary>
    ///     Browser environment.
    /// </summary>
    public static ComputerEnvironment Browser { get; } = new("browser");

    /// <summary>
    ///     The underlying string value of the environment.
    /// </summary>
    public string Value { get; } = value;

    /// <inheritdoc />
    public override string ToString() => Value;

    /// <inheritdoc />
    public bool Equals(ComputerEnvironment other) => string.Equals(Value, other.Value, StringComparison.OrdinalIgnoreCase);

    /// <inheritdoc />
    public override bool Equals(object? obj) => obj is ComputerEnvironment other && Equals(other);

    /// <inheritdoc />
    public override int GetHashCode() => StringComparer.OrdinalIgnoreCase.GetHashCode(Value);

    /// <summary>
    ///     Determines whether two <see cref="ComputerEnvironment" /> values are equal.
    /// </summary>
    public static bool operator ==(ComputerEnvironment left, ComputerEnvironment right) => left.Equals(right);

    /// <summary>
    ///     Determines whether two <see cref="ComputerEnvironment" /> values are not equal.
    /// </summary>
    public static bool operator !=(ComputerEnvironment left, ComputerEnvironment right) => !(left == right);

    /// <summary>
    ///     Implicitly converts a <see cref="ComputerEnvironment" /> to a string.
    /// </summary>
    public static implicit operator string(ComputerEnvironment env) => env.Value;

    /// <summary>
    ///     Implicitly converts a string to a <see cref="ComputerEnvironment" />.
    /// </summary>
    public static implicit operator ComputerEnvironment(string value) => new(value);

    /// <summary>
    ///     JSON converter for <see cref="ComputerEnvironment" />.
    /// </summary>
    public sealed class Converter : JsonConverter<ComputerEnvironment>
    {
        /// <inheritdoc />
        public override ComputerEnvironment Read(ref Utf8JsonReader reader, Type t, JsonSerializerOptions o) => new(reader.GetString()!);

        /// <inheritdoc />
        public override void Write(Utf8JsonWriter writer, ComputerEnvironment value, JsonSerializerOptions o) => writer.WriteStringValue(value.Value);
    }
}

