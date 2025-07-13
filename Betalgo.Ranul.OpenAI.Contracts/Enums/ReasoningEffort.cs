﻿using System.Text.Json;
using System.Text.Json.Serialization;

namespace Betalgo.Ranul.OpenAI.Contracts.Enums;

/// <summary>
///     <b>o-series models only</b>
///     <para />
///     Constrains effort on reasoning for
///     <see href="https://platform.openai.com/docs/guides/reasoning">reasoning models</see>.
///     Currently supported values are <c>low</c>, <c>medium</c>, and <c>high</c>. Reducing
///     reasoning effort can result in faster responses and fewer tokens used
///     on reasoning in a response.
///
/// </summary>

[JsonConverter(typeof(Converter))]
public readonly struct ReasoningEffort : IEquatable<ReasoningEffort>
{
    /// <summary>
    /// Underlying string value of the ReasoningEffortEnum.
    /// </summary>
    public string Value { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="value"/> struct.
    /// </summary>
    /// <param name="value">Underlying string value.</param>
    public ReasoningEffort(string value)
    {
        Value = value ?? throw new ArgumentNullException(nameof(value));
    }


    /// <summary>
    /// Literal <c>low</c>.
    /// </summary>
    public static ReasoningEffort Low { get; } = new("low");

    /// <summary>
    /// Literal <c>medium</c>.
    /// </summary>
    public static ReasoningEffort Medium { get; } = new("medium");

    /// <summary>
    /// Literal <c>high</c>.
    /// </summary>
    public static ReasoningEffort High { get; } = new("high");
    public override string ToString() => Value; public bool Equals(ReasoningEffort other) =>
string.Equals(Value, other.Value, StringComparison.OrdinalIgnoreCase); public override bool Equals(object? obj) =>
obj is ReasoningEffort other && Equals(other); public override int GetHashCode() =>
StringComparer.OrdinalIgnoreCase.GetHashCode(Value); public static bool operator ==(ReasoningEffort left, ReasoningEffort right) => left.Equals(right);
    public static bool operator !=(ReasoningEffort left, ReasoningEffort right) => !(left == right);
    public static implicit operator string(ReasoningEffort format) => format.Value;
    public static implicit operator ReasoningEffort(string value) => new(value);
    /// <summary>System-Text-Json converter for <see cref="ReasoningEffort"/>.</summary>
    public sealed class Converter : JsonConverter<ReasoningEffort>
    {
        public override ReasoningEffort Read(ref Utf8JsonReader reader, Type t, JsonSerializerOptions o) =>
              new(reader.GetString()!);

        public override void Write(Utf8JsonWriter writer, ReasoningEffort value, JsonSerializerOptions o) =>
              writer.WriteStringValue(value.Value);
    }
}