using Betalgo.Ranul.OpenAI.Contracts.Requests.Responses;
using Betalgo.Ranul.OpenAI.ObjectModels.SharedModels;

namespace Betalgo.Ranul.OpenAI.Extensions;

public static class ModelExtension
{
    public static void ProcessModelId(this IOpenAIModels.IModel modelFromObject, string? modelFromParameter, string? defaultModelId, bool allowNull = false)
    {
        if (allowNull)
        {
            modelFromObject.Model = modelFromParameter ?? modelFromObject.Model ?? defaultModelId;
        }
        else
        {
            modelFromObject.Model = modelFromParameter ?? modelFromObject.Model ?? defaultModelId ?? throw new ArgumentNullException("Model Id");
        }
    }

    public static void ProcessModelId(this CreateResponse request, string? modelFromParameter, string? defaultModelId)
    {
        if (request == null)
        {
            throw new ArgumentNullException(nameof(request));
        }

        request.Model = modelFromParameter ?? request.Model ?? defaultModelId ?? throw new ArgumentNullException("Model Id");
    }

    public static void ProcessModelId(this TokenCountsRequest request, string? modelFromParameter, string? defaultModelId)
    {
        if (request == null)
        {
            throw new ArgumentNullException(nameof(request));
        }

        request.Model = modelFromParameter ?? request.Model ?? defaultModelId ?? throw new ArgumentNullException("Model Id");
    }
}
