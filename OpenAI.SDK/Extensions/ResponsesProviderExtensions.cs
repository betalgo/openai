using Betalgo.Ranul.OpenAI.Interfaces;

namespace Betalgo.Ranul.OpenAI.Extensions;

public static class ResponsesProviderExtensions
{
    /// <summary>Accesses Responses from a compatible client; legacy custom implementations remain valid.</summary>
    public static IResponsesService AsResponses(this IOpenAIService service)
    {
        if (service == null) throw new ArgumentNullException(nameof(service));
        if (service is IResponsesProvider provider) return provider.Responses;
        if (service is IResponsesService responses) return responses;
        throw new NotSupportedException("This OpenAI service does not implement the optional Responses capability.");
    }
}
