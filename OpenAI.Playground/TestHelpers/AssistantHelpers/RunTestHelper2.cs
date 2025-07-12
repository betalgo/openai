using Betalgo.Ranul.OpenAI.Builders;
using Betalgo.Ranul.OpenAI.Contracts.Enums;
using Betalgo.Ranul.OpenAI.Interfaces;
using Betalgo.Ranul.OpenAI.ObjectModels;
using Betalgo.Ranul.OpenAI.ObjectModels.RequestModels;
using Betalgo.Ranul.OpenAI.ObjectModels.SharedModels;
using OpenAI.Playground.ExtensionsAndHelpers;

namespace OpenAI.Playground.TestHelpers.AssistantHelpers;

internal static class RunTestHelper2
{
    public static async Task RunRunCreateTest(IOpenAIService sdk)
    {
        ConsoleExtensions.WriteLine("Run create Testing is starting:", ConsoleColor.Cyan);


        try
        {
            ConsoleExtensions.WriteLine("Run Create Test:", ConsoleColor.DarkCyan);
            var threadResult = await sdk.Beta.Threads.ThreadCreate();
            var threadId = threadResult.Id;
            var func = new FunctionDefinitionBuilder("get_corp_location", "get location of corp").AddParameter("name", PropertyDefinition.DefineString("company name, e.g. Betterway")).Validate().Build();
            var assistantResult = await sdk.Beta.Assistants.AssistantCreate(new()
            {
                Instructions = "You are a professional assistant who provides company information. Company-related data comes from uploaded questions and does not provide vague answers, only clear answers.",
                Name = "Qicha",
                Tools = new() { ToolDefinition.DefineCodeInterpreter(), ToolDefinition.DefineFileSearch(), ToolDefinition.DefineFunction(func) },
                Model = Models.Gpt_3_5_Turbo_1106
            });
            var runResult = await sdk.Beta.Runs.RunCreate(threadId, new()
            {
                AssistantId = assistantResult.Id
            });
            if (runResult.Successful)
            {
                ConsoleExtensions.WriteLine(runResult.ToJson());
            }
            else
            {
                if (runResult.Error == null)
                {
                    throw new("Unknown Error");
                }

                ConsoleExtensions.WriteLine($"{runResult.Error.Code}: {runResult.Error.Message}");
            }

            var runId = runResult.Id;
            ConsoleExtensions.WriteLine($"runId: {runId}");

            var doneStatusList = new List<RunStatusEnum>()
                { RunStatusEnum.Cancelled, RunStatusEnum.Completed, RunStatusEnum.Failed, RunStatusEnum.Expired };
            var runStatus = RunStatusEnum.Queued;
            var attemptCount = 0;
            var maxAttempts = 10;

            do
            {
                var runRetrieveResult = await sdk.Beta.Runs.RunRetrieve(threadId, runId);
                runStatus = runRetrieveResult.Status;
                if (doneStatusList.Contains(runStatus))
                {
                    break;
                }

                var requireAction = runRetrieveResult.RequiredAction;
                if (runStatus == RunStatusEnum.RequiresAction && requireAction.Type == RequiredActionTypeEnum.SubmitToolOutputs)
                {
                    var toolCalls = requireAction.SubmitToolOutputs.ToolCalls;
                    foreach (var toolCall in toolCalls)
                    {
                        ConsoleExtensions.WriteLine($"ToolCall:{toolCall?.ToJson()}");
                        if (toolCall.FunctionCall == null) return;

                        var funcName = toolCall.FunctionCall.Name;
                        if (funcName == "get_corp_location")
                        {
                            await sdk.Beta.Runs.RunCancel(threadId, runRetrieveResult.Id);
                            // Do submit tool
                        }
                    }
                }

                await Task.Delay(1000);
                attemptCount++;
                if (attemptCount >= maxAttempts)
                {
                    throw new("The maximum number of attempts has been reached.");
                }
            } while (!doneStatusList.Contains(runStatus));
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }


    public static async Task RunRunCancelTest(IOpenAIService sdk)
    {
        ConsoleExtensions.WriteLine("Run cancel Testing is starting:", ConsoleColor.Cyan);
        var threadResult = await sdk.Beta.Threads.ThreadCreate();
        var threadId = threadResult.Id;
        var func = new FunctionDefinitionBuilder("get_corp_location", "get location of corp").AddParameter("name", PropertyDefinition.DefineString("company name, e.g. Betterway")).Validate().Build();
        var assistantResult = await sdk.Beta.Assistants.AssistantCreate(new()
        {
            Instructions = "You are a professional assistant who provides company information. Company-related data comes from uploaded questions and does not provide vague answers, only clear answers.",
            Name = "Qicha",
            Tools = new() { ToolDefinition.DefineCodeInterpreter(), ToolDefinition.DefineFileSearch(), ToolDefinition.DefineFunction(func) },
            Model = Models.Gpt_3_5_Turbo_1106
        });
        var runCreateResult = await sdk.Beta.Runs.RunCreate(threadId, new()
        {
            AssistantId = assistantResult.Id
        });

        ConsoleExtensions.WriteLine("Run Cancel Test:", ConsoleColor.DarkCyan);
        var runResult = await sdk.Beta.Runs.RunCancel(threadId, runCreateResult.Id);
        if (runResult.Successful)
        {
            ConsoleExtensions.WriteLine(runResult.ToJson());
        }
        else
        {
            if (runResult.Error == null)
            {
                throw new("Unknown Error");
            }

            ConsoleExtensions.WriteLine($"{runResult.Error.Code}: {runResult.Error.Message}");
        }
    }
}