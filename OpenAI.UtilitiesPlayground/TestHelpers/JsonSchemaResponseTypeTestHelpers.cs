using System.Text.Json;
using System.Text.Json.Serialization;
using OpenAI.Interfaces;
using OpenAI.ObjectModels;
using OpenAI.ObjectModels.RequestModels;
using OpenAI.Utilities.FunctionCalling;

namespace OpenAI.UtilitiesPlayground.TestHelpers;

public static class JsonSchemaResponseTypeTestHelpers
{
    public static async Task RunChatWithJsonSchemaResponseFormat2(IOpenAIService sdk)
    {
        Console.WriteLine("Chat Completion Testing is starting:");
        try
        {
            var completionResult = await sdk.ChatCompletion.CreateCompletion(new()
            {
                Messages = new List<ChatMessage>
            {
                ChatMessage.FromSystem("You are a helpful math tutor. Guide the user through the solution step by step."),
                ChatMessage.FromUser("how can I solve 8x + 7 = -23")
            },
                Model = "gpt-4o-2024-08-06",
                ResponseFormat = new()
                {
                    Type = StaticValues.CompletionStatics.ResponseFormat.JsonSchema,
                    JsonSchema = new()
                    {
                        Name = "math_response",
                        Strict = true,
                        Schema = PropertyDefinitionGenerator.GenerateFromType(typeof(MathResponse))
                    }
                }
            });

            if (completionResult.Successful)
            {
                var response =JsonSerializer.Deserialize<MathResponse>(completionResult.Choices.First().Message.Content!);
                foreach (var responseStep in response?.Steps!)
                {
                    Console.WriteLine(responseStep.Explanation);
                    Console.WriteLine(responseStep.Output);
                }

                Console.WriteLine("Final:" + response.FinalAnswer);

            }
            else
            {
                if (completionResult.Error == null)
                {
                    throw new("Unknown Error");
                }

                Console.WriteLine($"{completionResult.Error.Code}: {completionResult.Error.Message}");
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public class MathResponse
    {
        public MathResponse()
        {
            Steps = new();
        }

        [JsonPropertyName("steps")]
        public List<Step> Steps { get; set; }

        [JsonPropertyName("final_answer")]
        public string FinalAnswer { get; set; }
    }

    public class Step
    {
        [JsonPropertyName("explanation")]
        public string Explanation { get; set; }

        [JsonPropertyName("output")]
        public string Output { get; set; }
    }
}