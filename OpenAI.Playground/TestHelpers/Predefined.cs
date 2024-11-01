using Betalgo.Ranul.OpenAI.ObjectModels.RequestModels;
using Betalgo.Ranul.OpenAI.ObjectModels.SharedModels;

namespace OpenAI.Playground.TestHelpers;

public static class Predefined
{
    public static List<ToolDefinition> CurrentWhetherTool()
    {
        return
        [
            ToolDefinition.DefineFunction(new()
            {
                Name = "get_current_weather",
                Description = "Get the current weather",
                Parameters = PropertyDefinition.DefineObject(new Dictionary<string, PropertyDefinition>
                {
                    { "location", PropertyDefinition.DefineString("The city and state, e.g. San Francisco, CA") },
                    { "unit", PropertyDefinition.DefineEnum(["celsius", "fahrenheit"], string.Empty) }
                }, ["location"], null, null, null)
            })
        ];
    }
}