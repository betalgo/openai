using System.Text.Json;
using System.Text.Json.Serialization;

namespace Betalgo.Ranul.OpenAI.Contracts.Enums;
/// <summary>
///     <see href="">
///         OpenAI API documentation
///     </see>
/// </summary>
[JsonConverter(typeof(Converter))]
public readonly struct RunStatus(string value) : IEquatable<RunStatus>
{
    public static RunStatus Queued { get; } = new("queued");
    public static RunStatus InProgress { get; } = new("in_progress");
    public static RunStatus RequiresAction { get; } = new("requires_action");
    public static RunStatus Cancelling { get; } = new("cancelling");
    public static RunStatus Cancelled { get; } = new("cancelled");
    public static RunStatus Failed { get; } = new("failed");
    public static RunStatus Completed { get; } = new("completed");
    public static RunStatus Expired { get; } = new("expired");
    public static RunStatus Incomplete { get; } = new("incomplete");

    public string Value { get; } = value;
    public override string ToString() => Value;

    public bool Equals(RunStatus other) =>
        string.Equals(Value, other.Value, StringComparison.OrdinalIgnoreCase);

    public override bool Equals(object? obj) => obj is RunStatus other && Equals(other);
    public override int GetHashCode() => StringComparer.OrdinalIgnoreCase.GetHashCode(Value);
    public static bool operator ==(RunStatus left, RunStatus right) => left.Equals(right);
    public static bool operator !=(RunStatus left, RunStatus right) => !(left == right);

    public static implicit operator string(RunStatus format) => format.Value;
    public static implicit operator RunStatus(string value) => new(value);

    public sealed class Converter : JsonConverter<RunStatus>
    {
        public override RunStatus Read(ref Utf8JsonReader reader, Type t, JsonSerializerOptions o) =>
            new(reader.GetString()!);

        public override void Write(Utf8JsonWriter writer, RunStatus value, JsonSerializerOptions o) =>
            writer.WriteStringValue(value.Value);
    }
}
