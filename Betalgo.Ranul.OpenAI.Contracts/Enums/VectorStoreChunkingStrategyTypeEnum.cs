using System.Text.Json;
using System.Text.Json.Serialization;

namespace Betalgo.Ranul.OpenAI.Contracts.Enums;
/// <summary>
///     <see href="">
///         OpenAI API documentation
///     </see>
/// </summary>
[JsonConverter(typeof(Converter))]
public readonly struct VectorStoreChunkingStrategyTypeEnum(string value) : IEquatable<VectorStoreChunkingStrategyTypeEnum>
{
    public static VectorStoreChunkingStrategyTypeEnum Auto { get; } = new("auto");
    public static VectorStoreChunkingStrategyTypeEnum Static { get; } = new("static");

    public string Value { get; } = value;
    public override string ToString() => Value;

    public bool Equals(VectorStoreChunkingStrategyTypeEnum other) =>
        string.Equals(Value, other.Value, StringComparison.OrdinalIgnoreCase);

    public override bool Equals(object? obj) => obj is VectorStoreChunkingStrategyTypeEnum other && Equals(other);
    public override int GetHashCode() => StringComparer.OrdinalIgnoreCase.GetHashCode(Value);
    public static bool operator ==(VectorStoreChunkingStrategyTypeEnum left, VectorStoreChunkingStrategyTypeEnum right) => left.Equals(right);
    public static bool operator !=(VectorStoreChunkingStrategyTypeEnum left, VectorStoreChunkingStrategyTypeEnum right) => !(left == right);

    public static implicit operator string(VectorStoreChunkingStrategyTypeEnum format) => format.Value;
    public static implicit operator VectorStoreChunkingStrategyTypeEnum(string value) => new(value);

    public sealed class Converter : JsonConverter<VectorStoreChunkingStrategyTypeEnum>
    {
        public override VectorStoreChunkingStrategyTypeEnum Read(ref Utf8JsonReader reader, Type t, JsonSerializerOptions o) =>
            new(reader.GetString()!);

        public override void Write(Utf8JsonWriter writer, VectorStoreChunkingStrategyTypeEnum value, JsonSerializerOptions o) =>
            writer.WriteStringValue(value.Value);
    }
}
