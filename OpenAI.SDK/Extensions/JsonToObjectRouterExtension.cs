using System.Text.Json;
using Betalgo.Ranul.OpenAI.ObjectModels.ResponseModels;
using Betalgo.Ranul.OpenAI.ObjectModels.SharedModels;

namespace Betalgo.Ranul.OpenAI.Extensions;

public static class JsonToObjectRouterExtension
{
    public static Type Route(string json)
    {
        var apiResponse = JsonSerializer.Deserialize<ObjectBaseResponse>(json);

        return apiResponse?.ObjectTypeName switch
        {
            "thread.run.step" => typeof(RunStepResponse),
            "thread.run" => typeof(RunResponse),
            "thread.message" => typeof(MessageResponse),
            "thread.message.delta" => typeof(MessageResponse),
            _ => typeof(BaseResponse)
        };
    }
}