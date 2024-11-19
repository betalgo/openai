using System.Globalization;
using Betalgo.OpenAI.Utilities.FunctionCalling;
using Betalgo.Ranul.OpenAI.Interfaces;
using Betalgo.Ranul.OpenAI.ObjectModels;
using Betalgo.Ranul.OpenAI.ObjectModels.RequestModels;

namespace OpenAI.UtilitiesPlayground.TestHelpers;

public static class FunctionCallingTestHelpers
{
    public static async Task ExerciseFunctionCalling(IOpenAIService openAIService)
    {
        var calculator = new Calculator();
        var req = new ChatCompletionCreateRequest
        {
            //Functions = FunctionCallingHelper.GetFunctionDefinitions(calculator),
            //Functions = FunctionCallingHelper.GetFunctionDefinitions(typeof(Calculator)),
            Tools = FunctionCallingHelper.GetToolDefinitions<Calculator>(),
            Messages = new List<ChatMessage>
            {
                ChatMessage.FromSystem("You are a helpful assistant."),
                ChatMessage.FromUser("What is 2 + 2 * 6?") // GPT4 is needed for this
                //ChatMessage.FromUser("What is 2 + 6?"),  // GPT3.5 is enough for this
            }
        };

        do
        {
            var reply = await openAIService.ChatCompletion.CreateCompletion(req, Models.Gpt_4_0613);

            if (!reply.Successful)
            {
                Console.WriteLine(reply.Error?.Message);
                break;
            }

            var response = reply.Choices.First().Message;

            if (response.ToolCalls != null)
            {
                Console.WriteLine($"Invoking {response.ToolCalls.First().FunctionCall.Name} with params: {response.ToolCalls.First().FunctionCall.Arguments}");
            }
            else
            {
                Console.WriteLine(response.Content);
            }

            req.Messages.Add(response);

            if (response.ToolCalls != null)
            {
                var functionCall = response.ToolCalls.First().FunctionCall;
                var result = FunctionCallingHelper.CallFunction<float>(functionCall!, calculator);
                response.Content = result.ToString(CultureInfo.CurrentCulture);
            }
        } while (req.Messages.Last().FunctionCall != null);
    }


    public class Calculator
    {
        public enum AdvancedOperators
        {
            Multiply,
            Divide
        }

        [FunctionDescription("Adds two numbers.")]
        public float Add(float a, float b)
        {
            return a + b;
        }

        [FunctionDescription("Subtracts two numbers.")]
        public float Subtract(float a, float b)
        {
            return a - b;
        }

        [FunctionDescription("Performs advanced math operators on two numbers.")]
        public float AdvancedMath(float a, float b, AdvancedOperators advancedOperator)
        {
            return advancedOperator switch
            {
                AdvancedOperators.Multiply => a * b,
                AdvancedOperators.Divide => a / b,
                _ => throw new ArgumentOutOfRangeException(nameof(advancedOperator), advancedOperator, null)
            };
        }
    }
}