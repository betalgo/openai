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
}