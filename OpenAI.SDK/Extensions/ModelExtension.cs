using OpenAI.ObjectModels.SharedModels;

namespace OpenAI.Extensions;

public static class ModelExtension
{
    public static void ProcessModelId(this IOpenAiModels.IModel modelFromObject, string? modelFromParameter, string? defaultModelId)
    {
        modelFromObject.Model = modelFromParameter ?? modelFromObject.Model ?? defaultModelId ?? throw new ArgumentNullException("Model Id");
    }
}