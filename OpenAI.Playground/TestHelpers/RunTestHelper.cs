using OpenAI.Builders;
using OpenAI.Interfaces;
using OpenAI.ObjectModels;
using OpenAI.ObjectModels.RequestModels;
using OpenAI.ObjectModels.SharedModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OpenAI.Playground.TestHelpers
{
    internal static class RunTestHelper
    {
        public static async Task RunRunCreateTest(IOpenAIService sdk)
        {
            ConsoleExtensions.WriteLine("Run create Testing is starting:", ConsoleColor.Cyan);

            try
            {
                var threadId = "thread_eG76zeIGn8XoMN8yYOR1VxfG";
                var assistantId = $"asst_wnD0KzUtotn40AtnvLMufh9j";
                ConsoleExtensions.WriteLine("Run Create Test:", ConsoleColor.DarkCyan);
                var runResult = await sdk.Beta.Runs.RunCreate(threadId, new RunCreateRequest()
                {
                    AssistantId = assistantId,
                });
                if (runResult.Successful)
                {
                    ConsoleExtensions.WriteLine(runResult.ToJson());
                }
                else
                {
                    if (runResult.Error == null)
                    {
                        throw new Exception("Unknown Error");
                    }

                    ConsoleExtensions.WriteLine($"{runResult.Error.Code}: {runResult.Error.Message}");
                }

                var runId = runResult.Id;
                ConsoleExtensions.WriteLine($"runId: {runId}");

                var doneStatusList = new List<string>() { StaticValues.AssistatntsStatics.RunStatus.Cancelled, StaticValues.AssistatntsStatics.RunStatus.Completed, StaticValues.AssistatntsStatics.RunStatus.Failed, StaticValues.AssistatntsStatics.RunStatus.Expired };
                var runStatus = StaticValues.AssistatntsStatics.RunStatus.Queued;
                do
                {
                    var runRetrieveResult = await sdk.Beta.Runs.RunRetrieve(threadId, runId);
                    runStatus = runRetrieveResult.Status;
                    if (doneStatusList.Contains(runStatus)) { break; }

                    /*
                     * When a run has the status: "requires_action" and required_action.type is submit_tool_outputs, 
                     * this endpoint can be used to submit the outputs from the tool calls once they're all completed. 
                     * All outputs must be submitted in a single request.
                     */
                    var requireAction = runRetrieveResult.RequiredAction;
                    if (runStatus == StaticValues.AssistatntsStatics.RunStatus.RequiresAction && requireAction.Type == StaticValues.AssistatntsStatics.RequiredActionTypes.SubmitToolOutputs)
                    {
                        var toolCalls = requireAction.SubmitToolOutputs.ToolCalls;
                        foreach (var toolCall in toolCalls)
                        {
                            ConsoleExtensions.WriteLine($"ToolCall:{toolCall?.ToJson()}");
                            if (toolCall.FunctionCall == null) return;

                            var funcName = toolCall.FunctionCall.Name;
                            if (funcName == "get_current_weather")
                            {
                                //do sumbit tool
                            }
                        }
                    }
                    await Task.Delay(1000);
                } while (!doneStatusList.Contains(runStatus));
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

    }
}
