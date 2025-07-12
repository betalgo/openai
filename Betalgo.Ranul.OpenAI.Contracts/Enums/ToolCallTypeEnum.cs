using System.Text.Json;
using System.Text.Json.Serialization;

namespace Betalgo.Ranul.OpenAI.Contracts.Enums;
/// <summary>
///     <see href="">
///         OpenAI API documentation
///     </see>
/// </summary>
[JsonConverter(typeof(Converter))]
public readonly struct ToolCallTypeEnum(string value) : IEquatable<ToolCallTypeEnum>
{
    public static ToolCallTypeEnum CodeInterpreter { get; } = new("code_interpreter");
    public static ToolCallTypeEnum FileSearch { get; } = new("file_search");
    public static ToolCallTypeEnum Function { get; } = new("function");

    public string Value { get; } = value;
    public override string ToString() => Value;

    public bool Equals(ToolCallTypeEnum other) =>
        string.Equals(Value, other.Value, StringComparison.OrdinalIgnoreCase);

    public override bool Equals(object? obj) => obj is ToolCallTypeEnum other && Equals(other);
    public override int GetHashCode() => StringComparer.OrdinalIgnoreCase.GetHashCode(Value);
    public static bool operator ==(ToolCallTypeEnum left, ToolCallTypeEnum right) => left.Equals(right);
    public static bool operator !=(ToolCallTypeEnum left, ToolCallTypeEnum right) => !(left == right);

    public static implicit operator string(ToolCallTypeEnum format) => format.Value;
    public static implicit operator ToolCallTypeEnum(string value) => new(value);

    public sealed class Converter : JsonConverter<ToolCallTypeEnum>
    {
        public override ToolCallTypeEnum Read(ref Utf8JsonReader reader, Type t, JsonSerializerOptions o) =>
            new(reader.GetString()!);

        public override void Write(Utf8JsonWriter writer, ToolCallTypeEnum value, JsonSerializerOptions o) =>
            writer.WriteStringValue(value.Value);
    }
}
