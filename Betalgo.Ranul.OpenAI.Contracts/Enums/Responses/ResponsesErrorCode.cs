using System.Text.Json;
using System.Text.Json.Serialization;

namespace Betalgo.Ranul.OpenAI.Contracts.Enums.Responses;

/// <summary>
///     The error code for the Responses API.
///     <see href="https://platform.openai.com/docs/api-reference/responses/object">OpenAI API documentation</see>.
///     <see href="https://github.com/betalgo/openai/blob/master/Docs/openapi-split/components/schemas/responseerrorcode.yml">
///         Source Definition
///     </see>
/// </summary>
[JsonConverter(typeof(Converter))]
public readonly struct ResponsesErrorCode(string value) : IEquatable<ResponsesErrorCode>
{
    /// <summary>
    ///     A server error occurred.
    /// </summary>
    public static ResponsesErrorCode ServerError { get; } = new("server_error");

    /// <summary>
    ///     The rate limit was exceeded.
    /// </summary>
    public static ResponsesErrorCode RateLimitExceeded { get; } = new("rate_limit_exceeded");

    /// <summary>
    ///     The prompt was invalid.
    /// </summary>
    public static ResponsesErrorCode InvalidPrompt { get; } = new("invalid_prompt");

    /// <summary>
    ///     Vector store operation timed out.
    /// </summary>
    public static ResponsesErrorCode VectorStoreTimeout { get; } = new("vector_store_timeout");

    /// <summary>
    ///     The image was invalid.
    /// </summary>
    public static ResponsesErrorCode InvalidImage { get; } = new("invalid_image");

    /// <summary>
    ///     The image format was invalid.
    /// </summary>
    public static ResponsesErrorCode InvalidImageFormat { get; } = new("invalid_image_format");

    /// <summary>
    ///     The base64 encoded image was invalid.
    /// </summary>
    public static ResponsesErrorCode InvalidBase64Image { get; } = new("invalid_base64_image");

    /// <summary>
    ///     The image URL was invalid.
    /// </summary>
    public static ResponsesErrorCode InvalidImageUrl { get; } = new("invalid_image_url");

    /// <summary>
    ///     The image was too large.
    /// </summary>
    public static ResponsesErrorCode ImageTooLarge { get; } = new("image_too_large");

    /// <summary>
    ///     The image was too small.
    /// </summary>
    public static ResponsesErrorCode ImageTooSmall { get; } = new("image_too_small");

    /// <summary>
    ///     Failed to parse the image.
    /// </summary>
    public static ResponsesErrorCode ImageParseError { get; } = new("image_parse_error");

    /// <summary>
    ///     The image violated content policy.
    /// </summary>
    public static ResponsesErrorCode ImageContentPolicyViolation { get; } = new("image_content_policy_violation");

    /// <summary>
    ///     The image mode was invalid.
    /// </summary>
    public static ResponsesErrorCode InvalidImageMode { get; } = new("invalid_image_mode");

    /// <summary>
    ///     The image file was too large.
    /// </summary>
    public static ResponsesErrorCode ImageFileTooLarge { get; } = new("image_file_too_large");

    /// <summary>
    ///     The image media type is not supported.
    /// </summary>
    public static ResponsesErrorCode UnsupportedImageMediaType { get; } = new("unsupported_image_media_type");

    /// <summary>
    ///     The image file was empty.
    /// </summary>
    public static ResponsesErrorCode EmptyImageFile { get; } = new("empty_image_file");

    /// <summary>
    ///     Failed to download the image from the provided URL.
    /// </summary>
    public static ResponsesErrorCode FailedToDownloadImage { get; } = new("failed_to_download_image");

    /// <summary>
    ///     The image file was not found.
    /// </summary>
    public static ResponsesErrorCode ImageFileNotFound { get; } = new("image_file_not_found");

    /// <summary>
    ///     The underlying string value of the error code.
    /// </summary>
    public string Value { get; } = value;

    /// <inheritdoc />
    public override string ToString() => Value;

    /// <inheritdoc />
    public bool Equals(ResponsesErrorCode other) => string.Equals(Value, other.Value, StringComparison.OrdinalIgnoreCase);

    /// <inheritdoc />
    public override bool Equals(object? obj) => obj is ResponsesErrorCode other && Equals(other);

    /// <inheritdoc />
    public override int GetHashCode() => StringComparer.OrdinalIgnoreCase.GetHashCode(Value);

    /// <summary>
    ///     Determines whether two <see cref="ResponsesErrorCode" /> values are equal.
    /// </summary>
    public static bool operator ==(ResponsesErrorCode left, ResponsesErrorCode right) => left.Equals(right);

    /// <summary>
    ///     Determines whether two <see cref="ResponsesErrorCode" /> values are not equal.
    /// </summary>
    public static bool operator !=(ResponsesErrorCode left, ResponsesErrorCode right) => !(left == right);

    /// <summary>
    ///     Implicitly converts a <see cref="ResponsesErrorCode" /> to a string.
    /// </summary>
    public static implicit operator string(ResponsesErrorCode code) => code.Value;

    /// <summary>
    ///     Implicitly converts a string to a <see cref="ResponsesErrorCode" />.
    /// </summary>
    public static implicit operator ResponsesErrorCode(string value) => new(value);

    /// <summary>
    ///     JSON converter for <see cref="ResponsesErrorCode" />.
    /// </summary>
    public sealed class Converter : JsonConverter<ResponsesErrorCode>
    {
        /// <inheritdoc />
        public override ResponsesErrorCode Read(ref Utf8JsonReader reader, Type t, JsonSerializerOptions o) => new(reader.GetString()!);

        /// <inheritdoc />
        public override void Write(Utf8JsonWriter writer, ResponsesErrorCode value, JsonSerializerOptions o) => writer.WriteStringValue(value.Value);
    }
}

