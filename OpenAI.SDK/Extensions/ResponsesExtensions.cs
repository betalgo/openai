using System.Text.Json;
using System.Text.Json.Serialization;
using Betalgo.Ranul.OpenAI.Contracts.Responses.Responses;
using Betalgo.Ranul.OpenAI.Contracts.Types;
using Betalgo.Ranul.OpenAI.Contracts.Types.Content;
using Betalgo.Ranul.OpenAI.Contracts.Types.Items;

namespace Betalgo.Ranul.OpenAI;

/// <summary>
///     Extension methods for <see cref="Response" />.
/// </summary>
public static class ResponsesExtensions
{
    /// <summary>
    ///     Copies output items into detached input items for replay in a later Responses request.
    ///     Preserves all fields serialized by the output item converters, including tool calls and opaque items.
    ///     The returned items do not share mutable content with the response.
    /// </summary>
    /// <param name="response">The response whose output should be replayed.</param>
    /// <returns>Replay input items in their original output order; empty when output is absent.</returns>
    public static List<IInputItem> ToInputItems(this Response response)
    {
        if (response == null) throw new ArgumentNullException(nameof(response));
        return response.Output?.Select(item => (IInputItem)new ReplayInputItem(JsonSerializer.SerializeToElement(item))).ToList()
               ?? new List<IInputItem>();
    }

    // Input DTOs need not represent every output type or future field. A detached JSON snapshot avoids
    // losing information through an output-to-input DTO conversion (or sharing mutable tool-call objects).
    [JsonConverter(typeof(ReplayInputItemConverter))]
    private sealed class ReplayInputItem(JsonElement data) : IInputItem
    {
        public JsonElement Data { get; } = data;
        public string Type => Data.GetProperty("type").GetString()!;
    }

    private sealed class ReplayInputItemConverter : JsonConverter<ReplayInputItem>
    {
        public override ReplayInputItem? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            using var document = JsonDocument.ParseValue(ref reader);
            return new ReplayInputItem(document.RootElement.Clone());
        }

        public override void Write(Utf8JsonWriter writer, ReplayInputItem value, JsonSerializerOptions options) => value.Data.WriteTo(writer);
    }

    /// <summary>
    ///     Gets all text content from the output items.
    /// </summary>
    /// <param name="response">The response to extract text from.</param>
    /// <returns>The concatenated text from all output message items.</returns>
    public static string? GetOutputText(this Response response)
    {
        if (!string.IsNullOrEmpty(response.OutputText))
            return response.OutputText;

        if (response.Output == null || response.Output.Count == 0)
            return null;

        var texts = new List<string>();
        foreach (var item in response.Output)
        {
            if (item is OutputMessageItem message)
            {
                foreach (var content in message.Content)
                {
                    if (content is OutputTextContent textContent)
                    {
                        texts.Add(textContent.Text);
                    }
                }
            }
        }

        return texts.Count > 0 ? string.Join("", texts) : null;
    }
}

