using System.Text.Json;
using System.Text.Json.Serialization;

namespace Betalgo.Ranul.OpenAI.Contracts.Enums;
/// <summary>
///     <see href="">
///         OpenAI API documentation
///     </see>
/// </summary>
[JsonConverter(typeof(Converter))]
public readonly struct RunStatusEnum(string value) : IEquatable<RunStatusEnum>
{
    public static RunStatusEnum Queued { get; } = new("queued");
    public static RunStatusEnum InProgress { get; } = new("in_progress");
    public static RunStatusEnum RequiresAction { get; } = new("requires_action");
    public static RunStatusEnum Cancelling { get; } = new("cancelling");
    public static RunStatusEnum Cancelled { get; } = new("cancelled");
    public static RunStatusEnum Failed { get; } = new("failed");
    public static RunStatusEnum Completed { get; } = new("completed");
    public static RunStatusEnum Expired { get; } = new("expired");
    public static RunStatusEnum Incomplete { get; } = new("incomplete");

    public string Value { get; } = value;
    public override string ToString() => Value;

    public bool Equals(RunStatusEnum other) =>
        string.Equals(Value, other.Value, StringComparison.OrdinalIgnoreCase);

    public override bool Equals(object? obj) => obj is RunStatusEnum other && Equals(other);
    public override int GetHashCode() => StringComparer.OrdinalIgnoreCase.GetHashCode(Value);
    public static bool operator ==(RunStatusEnum left, RunStatusEnum right) => left.Equals(right);
    public static bool operator !=(RunStatusEnum left, RunStatusEnum right) => !(left == right);

    public static implicit operator string(RunStatusEnum format) => format.Value;
    public static implicit operator RunStatusEnum(string value) => new(value);

    public sealed class Converter : JsonConverter<RunStatusEnum>
    {
        public override RunStatusEnum Read(ref Utf8JsonReader reader, Type t, JsonSerializerOptions o) =>
            new(reader.GetString()!);

        public override void Write(Utf8JsonWriter writer, RunStatusEnum value, JsonSerializerOptions o) =>
            writer.WriteStringValue(value.Value);
    }
}
