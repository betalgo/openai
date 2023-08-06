using System.Globalization;
using OpenAI.Interfaces;
using OpenAI.ObjectModels;
using OpenAI.ObjectModels.RequestModels;
using OpenAI.Utilities.FunctionCalling;

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
            Functions = FunctionCallingHelper.GetFunctionDefinitions<Calculator>(),
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

            if (response.FunctionCall != null)
            {
                Console.WriteLine($"Invoking {response.FunctionCall.Name} with params: {response.FunctionCall.Arguments}");
            }
            else
            {
                Console.WriteLine(response.Content);
            }

            req.Messages.Add(response);

            if (response.FunctionCall != null)
            {
                var functionCall = response.FunctionCall;
                var result = FunctionCallingHelper.CallFunction<float>(functionCall, calculator);
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