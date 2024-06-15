using System.Text.Json;
using OpenAI.ObjectModels.ResponseModels;
using OpenAI.ObjectModels.SharedModels;

namespace OpenAI.Extensions;

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