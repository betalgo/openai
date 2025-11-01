using System.Text.Json;
using System.Text.Json.Serialization;

namespace Betalgo.Ranul.OpenAI.Contracts.Enums;

/// <summary>
///     <see href="">
///         OpenAI API documentation
///     </see>
/// </summary>
[JsonConverter(typeof(Converter))]
public readonly struct VectorStoreChunkingStrategyType(string value) : IEquatable<VectorStoreChunkingStrategyType>
{
    public static VectorStoreChunkingStrategyType Auto { get; } = new("auto");
    public static VectorStoreChunkingStrategyType Static { get; } = new("static");

     public string Value { get; } = value;
    public override string ToString() => Value;
    public bool Equals(VectorStoreChunkingStrategyType other) => string.Equals(Value, other.Value, StringComparison.OrdinalIgnoreCase);
    public override bool Equals(object? obj) => obj is VectorStoreChunkingStrategyType other && Equals(other);
    public override int GetHashCode() => StringComparer.OrdinalIgnoreCase.GetHashCode(Value);
    public static bool operator ==(VectorStoreChunkingStrategyType left, VectorStoreChunkingStrategyType right) => left.Equals(right);
    public static bool operator !=(VectorStoreChunkingStrategyType left, VectorStoreChunkingStrategyType right) => !(left == right);
    public static implicit operator string(VectorStoreChunkingStrategyType format) => format.Value;
    public static implicit operator VectorStoreChunkingStrategyType(string value) => new(value);

    public sealed class Converter : JsonConverter<VectorStoreChunkingStrategyType>
    {
        public override VectorStoreChunkingStrategyType Read(ref Utf8JsonReader reader, Type t, JsonSerializerOptions o) => new(reader.GetString()!);
        public override void Write(Utf8JsonWriter writer, VectorStoreChunkingStrategyType value, JsonSerializerOptions o) => writer.WriteStringValue(value.Value);
    }
}