using System.Text.Json;
using Betalgo.OpenAI.ObjectModels.ResponseModels;
using Betalgo.OpenAI.ObjectModels.SharedModels;

namespace Betalgo.OpenAI.Extensions;

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