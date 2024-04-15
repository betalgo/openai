using OpenAI.ObjectModels.SharedModels;

namespace OpenAI.Extensions;

public static class ModelExtension
{
    public static void ProcessModelId(this IOpenAiModels.IModel modelFromObject, string? modelFromParameter, string? defaultModelId,bool allowNull =false)
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
}