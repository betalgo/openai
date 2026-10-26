using System.Text.Json.Serialization;
using Betalgo.Ranul.OpenAI.Contracts.Interfaces;

namespace Betalgo.Ranul.OpenAI.Contracts.Types.Streaming;

/// <summary>
///     Marker interface for all Responses API streaming events.
///     <see href="https://platform.openai.com/docs/api-reference/responses-streaming">OpenAI API documentation</see>.
///     <see href="https://github.com/betalgo/openai/blob/master/Docs/openapi-split/components/schemas/responsestreamevent.yml">
///         Source Definition
///     </see>
/// </summary>
[JsonConverter(typeof(ResponseStreamEventConverter))]
public interface IResponseStreamEvent : IStreamEvent
{
}
