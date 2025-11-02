using Betalgo.Ranul.OpenAI.Contracts.Enums;
using Betalgo.Ranul.OpenAI.Interfaces;
using Betalgo.Ranul.OpenAI.ObjectModels;
using Betalgo.Ranul.OpenAI.ObjectModels.RequestModels;
using Betalgo.Ranul.OpenAI.ObjectModels.ResponseModels;
using Betalgo.Ranul.OpenAI.ObjectModels.SharedModels;
using OpenAI.Playground.ExtensionsAndHelpers;

namespace OpenAI.Playground.TestHelpers.AssistantHelpers;

internal static partial class AssistantTestHelper
{
    internal static partial class RunTestHelper
    {
        private static string? CreatedRunId { get; set; }
        private static string? CreatedAssistantId { get; set; }
        private static string? CreatedThreadId { get; set; }

        public static async Task RunTests(IOpenAIService openAI)
        {
            ConsoleExtensions.WriteLine("Run Run Testing is starting:", ConsoleColor.Blue);
            await RunBasicTests(openAI);
            await RunCancelTests(openAI);
            await RunToolTests(openAI);
            await RunThreadAndRunTests(openAI);
            await RunStreamTests(openAI);
            await Cleanup(openAI);
        }

        public static async Task RunBasicTests(IOpenAIService openAI)
        {
            ConsoleExtensions.WriteLine("Run Basics Testing is starting:", ConsoleColor.Blue);
            await CreateRunTest(openAI);
            await ListRunsTest(openAI);
            await RetrieveRunTest(openAI);
            await WaitUntil(openAI, RunStatus.Completed);
            await ListRunStepsTest(openAI);
            await RetrieveRunStepTest(openAI);
            await Cleanup(openAI);
        }

        public static async Task RunCancelTests(IOpenAIService openAI)
        {
            ConsoleExtensions.WriteLine("Run Cancel Testing is starting:", ConsoleColor.Blue);
            await CancelRunTest(openAI);
            await Cleanup(openAI);
        }

        public static async Task RunToolTests(IOpenAIService openAI)
        {
            ConsoleExtensions.WriteLine("Run Tool Testing is starting:", ConsoleColor.Blue);
            await CreateToolRunTest(openAI);
            await ListRunsTest(openAI);
            await RetrieveRunTest(openAI);
            await ModifyRunTest(openAI);
            await WaitUntil(openAI, RunStatus.RequiresAction);
            await SubmitToolOutputsToRunTest(openAI);
            await Cleanup(openAI);
        }

        public static async Task RunStreamTests(IOpenAIService openAI)
        {
            ConsoleExtensions.WriteLine("Run Stream Testing is starting:", ConsoleColor.Blue);
            await CreateRunAsStreamTest(openAI);
            await Cleanup(openAI);
            await CreateThreadAndRunAsStream(openAI);
            await Cleanup(openAI);
            await CreateToolRunTest(openAI);
            await ListRunsTest(openAI);
            await RetrieveRunTest(openAI);
            await ModifyRunTest(openAI);
            await WaitUntil(openAI, RunStatus.RequiresAction);
            await SubmitToolOutputsAsStreamToRunTest(openAI);
            await Cleanup(openAI);
        }

        public static async Task RunThreadAndRunTests(IOpenAIService openAI)
        {
            ConsoleExtensions.WriteLine("Run Thread and Run Testing is starting:", ConsoleColor.Blue);
            await CreateThreadAndRun(openAI);
        }


        public static async Task CreateRunTest(IOpenAIService openAI)
        {
            ConsoleExtensions.WriteLine("Run Create Testing is starting:", ConsoleColor.Cyan);
            var assistantResult = await openAI.Beta.Assistants.AssistantCreate(new()
            {
                Instructions = "You are a personal math tutor. When asked a question, write and run Python code to answer the question.",
                Name = "Math Tutor",
                Tools = [ToolDefinition.DefineCodeInterpreter()],
                Model = Models.Gpt_4_turbo
            });
            if (assistantResult.Successful)
            {
                CreatedAssistantId = assistantResult.Id;
                ConsoleExtensions.WriteLine($"Assistant Created Successfully with ID: {assistantResult.Id}", ConsoleColor.Green);
            }
            else
            {
                ConsoleExtensions.WriteError(assistantResult.Error);
                return;
            }

            var threadResult = await openAI.Beta.Threads.ThreadCreate();
            if (threadResult.Successful)
            {
                CreatedThreadId = threadResult.Id;
                ConsoleExtensions.WriteLine($"Thread Created Successfully with ID: {threadResult.Id}", ConsoleColor.Green);
            }
            else
            {
                ConsoleExtensions.WriteError(threadResult.Error);
                return;
            }

            var result = await openAI.Beta.Runs.RunCreate(CreatedThreadId, new()
            {
                AssistantId = assistantResult.Id
            });

            if (result.Successful)
            {
                CreatedRunId = result.Id;
                ConsoleExtensions.WriteLine($"Run Created Successfully with ID: {result.Id}", ConsoleColor.Green);
            }
            else
            {
                ConsoleExtensions.WriteError(result.Error);
            }
        }

        public static async Task CreateRunAsStreamTest(IOpenAIService openAI)
        {
            ConsoleExtensions.WriteLine("Run Create As Stream Testing is starting:", ConsoleColor.Cyan);
            var assistantResult = await openAI.Beta.Assistants.AssistantCreate(new()
            {
                Instructions = "You are a personal math tutor. When asked a question, write and run Python code to answer the question.",
                Name = "Math Tutor",
                Tools = [ToolDefinition.DefineCodeInterpreter()],
                Model = Models.Gpt_4_turbo
            });
            if (assistantResult.Successful)
            {
                CreatedAssistantId = assistantResult.Id;
                ConsoleExtensions.WriteLine($"Assistant Created Successfully with ID: {assistantResult.Id}", ConsoleColor.Green);
            }
            else
            {
                ConsoleExtensions.WriteError(assistantResult.Error);
                return;
            }

            var threadResult = await openAI.Beta.Threads.ThreadCreate();
            if (threadResult.Successful)
            {
                CreatedThreadId = threadResult.Id;
                ConsoleExtensions.WriteLine($"Thread Created Successfully with ID: {threadResult.Id}", ConsoleColor.Green);
            }
            else
            {
                ConsoleExtensions.WriteError(threadResult.Error);
                return;
            }

            var result = openAI.Beta.Runs.RunCreateAsStream(CreatedThreadId, new()
            {
                AssistantId = assistantResult.Id
            },justDataMode:false);

            await foreach (var run in result)
            {
                if (run.Successful)
                {
                    Console.WriteLine($"Event:{run.StreamEvent}");
                    if (run is RunResponse runResponse)
                    {
                        if (runResponse.Status == null)
                        {
                            Console.Write(".");
                        }
                        else
                        {
                            ConsoleExtensions.WriteLine($"Run Id: {runResponse.Id}, Status: {runResponse.Status}");
                        }
                    }

                    else if (run is RunStepResponse runStepResponse)
                    {
                        if (string.IsNullOrEmpty(runStepResponse.Status))
                        {
                            Console.Write(".");
                        }
                        else
                        {
                            ConsoleExtensions.WriteLine($"Run Step Id: {runStepResponse.Id}, Status: {runStepResponse.Status}");
                        }
                    }

                    else if (run is MessageResponse messageResponse)
                    {
                        if (string.IsNullOrEmpty(messageResponse.Id))
                        {
                            Console.Write(".");
                        }
                        else
                        {
                            ConsoleExtensions.WriteLine($"Message Id: {messageResponse.Id}, Message: {messageResponse.Content?.FirstOrDefault()?.Text?.Value}");
                        }
                    }
                    else
                    {
                        if (run.StreamEvent!=null)
                        {
                            Console.WriteLine(run.StreamEvent);
                        }
                        else
                        {
                            Console.Write(".");
                        }
                    }
                }
                else
                {
                    ConsoleExtensions.WriteError(run.Error);
                }
            }
        }

        public static async Task CreateToolRunTest(IOpenAIService openAI)
        {
            ConsoleExtensions.WriteLine("Run Create Tool Testing is starting:", ConsoleColor.Cyan);
            var assistantResult = await openAI.Beta.Assistants.AssistantCreate(new()
            {
                Instructions = "You are a personal weather Assistant. When asked a question, use tools and reply with the weather.",
                Name = "Weather",
                Tools = [ToolDefinition.DefineCodeInterpreter()],
                Model = Models.Gpt_4_turbo
            });
            if (assistantResult.Successful)
            {
                CreatedAssistantId = assistantResult.Id;
                ConsoleExtensions.WriteLine($"Assistant Created Successfully with ID: {assistantResult.Id}", ConsoleColor.Green);
            }
            else
            {
                ConsoleExtensions.WriteError(assistantResult.Error);
                return;
            }

            var threadResult = await openAI.Beta.Threads.ThreadCreate(new()
            {
                Messages = [new(AssistantMessageRole.User, new("How is the weather in London"))]
            });
            if (threadResult.Successful)
            {
                CreatedThreadId = threadResult.Id;
                ConsoleExtensions.WriteLine($"Thread Created Successfully with ID: {threadResult.Id}", ConsoleColor.Green);
            }
            else
            {
                ConsoleExtensions.WriteError(threadResult.Error);
                return;
            }


            var result = await openAI.Beta.Runs.RunCreate(CreatedThreadId, new()
            {
                AssistantId = assistantResult.Id,
                Tools = Predefined.CurrentWhetherTool()
            });

            if (result.Successful)
            {
                CreatedRunId = result.Id;
                ConsoleExtensions.WriteLine($"Run Created Successfully with ID: {result.Id}", ConsoleColor.Green);
            }
            else
            {
                ConsoleExtensions.WriteError(result.Error);
            }
        }

        public static async Task ListRunsTest(IOpenAIService openAI)
        {
            ConsoleExtensions.WriteLine("List Runs Testing is starting:", ConsoleColor.Cyan);
            if (string.IsNullOrWhiteSpace(CreatedThreadId))
            {
                ConsoleExtensions.WriteLine("Thread Id is not found. Please create a thread first.", ConsoleColor.Red);
                return;
            }

            var allRuns = new List<RunResponse>();
            var hasMore = true;
            var lastId = string.Empty;
            while (hasMore)
            {
                var result = await openAI.Beta.Runs.ListRuns(CreatedThreadId, new() { After = lastId, Limit = 5 });
                if (result.Successful)
                {
                    if (result.Data != null)
                    {
                        allRuns.AddRange(result.Data);
                    }

                    hasMore = result.HasMore;
                    lastId = result.LastId;
                }
                else
                {
                    ConsoleExtensions.WriteError(result.Error);
                    return;
                }
            }

            foreach (var run in allRuns)
            {
                Console.WriteLine($"ID: {run.Id}, Status: {run.Status}");
            }

            if (allRuns.FirstOrDefault(r => r.Id == CreatedRunId) != null)
            {
                ConsoleExtensions.WriteLine("List Runs Test is successful.", ConsoleColor.Green);
            }
            else
            {
                ConsoleExtensions.WriteLine("List Runs Test is failed.", ConsoleColor.Red);
            }
        }

        public static async Task RetrieveRunTest(IOpenAIService openAI)
        {
            ConsoleExtensions.WriteLine("Retrieve Run Testing is starting:", ConsoleColor.Cyan);
            if (string.IsNullOrWhiteSpace(CreatedRunId))
            {
                ConsoleExtensions.WriteLine("Run Id is not found. Please create a run first.", ConsoleColor.Red);
                return;
            }

            if (string.IsNullOrWhiteSpace(CreatedThreadId))
            {
                ConsoleExtensions.WriteLine("Thread Id is not found. Please create a thread first.", ConsoleColor.Red);
                return;
            }

            var result = await openAI.Beta.Runs.RunRetrieve(CreatedThreadId, CreatedRunId);
            if (result.Successful)
            {
                ConsoleExtensions.WriteLine("Retrieve Run Test is successful.", ConsoleColor.Green);
            }
            else
            {
                ConsoleExtensions.WriteError(result.Error);
            }
        }

        public static async Task WaitUntil(IOpenAIService openAI, RunStatus status)
        {
            ConsoleExtensions.WriteLine("Wait Until Run is completed Testing is starting:", ConsoleColor.Cyan);
            if (string.IsNullOrWhiteSpace(CreatedRunId))
            {
                ConsoleExtensions.WriteLine("Run Id is not found. Please create a run first.", ConsoleColor.Red);
                return;
            }

            if (string.IsNullOrWhiteSpace(CreatedThreadId))
            {
                ConsoleExtensions.WriteLine("Thread Id is not found. Please create a thread first.", ConsoleColor.Red);
                return;
            }

            var result = await openAI.Beta.Runs.RunRetrieve(CreatedThreadId, CreatedRunId);
            while (result.Status != status)
            {
                ConsoleExtensions.WriteLine($"Run Status is {result.Status}. Waiting for 5 seconds to check again.", ConsoleColor.Yellow);
                await Task.Delay(5000);
                result = await openAI.Beta.Runs.RunRetrieve(CreatedThreadId, CreatedRunId);
            }

            ConsoleExtensions.WriteLine("Wait Until Run is completed Test is successful.", ConsoleColor.Green);
        }

        public static async Task ModifyRunTest(IOpenAIService openAI)
        {
            ConsoleExtensions.WriteLine("Modify Run Testing is starting:", ConsoleColor.Cyan);
            if (string.IsNullOrWhiteSpace(CreatedRunId))
            {
                ConsoleExtensions.WriteLine("Run Id is not found. Please create a run first.", ConsoleColor.Red);
                return;
            }

            if (string.IsNullOrWhiteSpace(CreatedThreadId))
            {
                ConsoleExtensions.WriteLine("Thread Id is not found. Please create a thread first.", ConsoleColor.Red);
                return;
            }

            await Task.Delay(5000); // Wait for the run to be in a state where it can be modified.
            var result = await openAI.Beta.Runs.RunModify(CreatedThreadId, CreatedRunId, new()
            {
                Metadata = new()
                {
                    { "modified", "true" },
                    { "user", "abc123" }
                }
            });
            if (result.Successful)
            {
                if (result.Metadata != null && result.Metadata.ContainsKey("modified") && result.Metadata.ContainsKey("user"))
                {
                    ConsoleExtensions.WriteLine("Modify Run Test is successful.", ConsoleColor.Green);
                }
                else
                {
                    ConsoleExtensions.WriteLine("Modify Run Test is failed.", ConsoleColor.Red);
                }
            }
            else
            {
                ConsoleExtensions.WriteError(result.Error);
            }
        }

        public static async Task SubmitToolOutputsToRunTest(IOpenAIService openAI)
        {
            ConsoleExtensions.WriteLine("Submit Tool Outputs To Run Testing is starting:", ConsoleColor.Cyan);
            if (string.IsNullOrWhiteSpace(CreatedRunId))
            {
                ConsoleExtensions.WriteLine("Run Id is not found. Please create a run first.", ConsoleColor.Red);
                return;
            }

            if (string.IsNullOrWhiteSpace(CreatedThreadId))
            {
                ConsoleExtensions.WriteLine("Thread Id is not found. Please create a thread first.", ConsoleColor.Red);
                return;
            }

            var retrieveResult = await openAI.Beta.Runs.RunRetrieve(CreatedThreadId, CreatedRunId);
            var result = await openAI.Beta.Runs.RunSubmitToolOutputs(CreatedThreadId, CreatedRunId, new()
            {
                ToolOutputs =
                [
                    new()
                    {
                        ToolCallId = retrieveResult.RequiredAction!.SubmitToolOutputs.ToolCalls.First().Id,
                        Output = "70 degrees and sunny."
                    }
                ]
            });
            if (result.Successful)
            {
                ConsoleExtensions.WriteLine("Submit Tool Outputs To Run Test is successful.", ConsoleColor.Green);
            }
            else
            {
                ConsoleExtensions.WriteError(result.Error);
            }
        }

        public static async Task SubmitToolOutputsAsStreamToRunTest(IOpenAIService openAI)
        {
            ConsoleExtensions.WriteLine("Submit Tool Outputs To Run Testing is starting:", ConsoleColor.Cyan);
            if (string.IsNullOrWhiteSpace(CreatedRunId))
            {
                ConsoleExtensions.WriteLine("Run Id is not found. Please create a run first.", ConsoleColor.Red);
                return;
            }

            if (string.IsNullOrWhiteSpace(CreatedThreadId))
            {
                ConsoleExtensions.WriteLine("Thread Id is not found. Please create a thread first.", ConsoleColor.Red);
                return;
            }

            var retrieveResult = await openAI.Beta.Runs.RunRetrieve(CreatedThreadId, CreatedRunId);
            var result = openAI.Beta.Runs.RunSubmitToolOutputsAsStream(CreatedThreadId, CreatedRunId, new()
            {
                ToolOutputs =
                [
                    new()
                    {
                        ToolCallId = retrieveResult.RequiredAction!.SubmitToolOutputs.ToolCalls.First().Id,
                        Output = "70 degrees and sunny."
                    }
                ]
            });

            await foreach (var run in result)
            {
                if (run.Successful)
                {
                    Console.WriteLine($"Event:{run.StreamEvent}");
                    if (run is RunResponse runResponse)
                    {
                        if (runResponse.Status == null)
                        {
                            Console.Write(".");
                        }
                        else
                        {
                            ConsoleExtensions.WriteLine($"Run Id: {runResponse.Id}, Status: {runResponse.Status}");
                        }
                    }

                    else if (run is RunStepResponse runStepResponse)
                    {
                        if (string.IsNullOrEmpty(runStepResponse.Status))
                        {
                            Console.Write(".");
                        }
                        else
                        {
                            ConsoleExtensions.WriteLine($"Run Step Id: {runStepResponse.Id}, Status: {runStepResponse.Status}");
                        }
                    }

                    else if (run is MessageResponse messageResponse)
                    {
                        if (string.IsNullOrEmpty(messageResponse.Id))
                        {
                            Console.Write(".");
                        }
                        else
                        {
                            ConsoleExtensions.WriteLine($"Message Id: {messageResponse.Id}, Message: {messageResponse.Content?.FirstOrDefault()?.Text?.Value}");
                        }
                    }
                    else
                    {
                        if (run.StreamEvent != null)
                        {
                            Console.WriteLine(run.StreamEvent);
                        }
                        else
                        {
                            Console.Write(".");
                        }
                    }
                }
                else
                {
                    ConsoleExtensions.WriteError(run.Error);
                }
            }
        }

        public static async Task CancelRunTest(IOpenAIService openAI)
        {
            ConsoleExtensions.WriteLine("Run Cancel Testing is starting:", ConsoleColor.Cyan);

            var createThreadResult = await openAI.Beta.Threads.ThreadCreate();
            var assistantResult = await openAI.Beta.Assistants.AssistantCreate(new()
            {
                Instructions = "You are a personal math tutor. When asked a question, write and run Python code to answer the question.",
                Name = "Math Tutor",
                Tools = [ToolDefinition.DefineCodeInterpreter()],
                Model = Models.Gpt_4_turbo
            });
            if (createThreadResult.Successful)
            {
                CreatedAssistantId = assistantResult.Id;
            }

            var createRunResult = await openAI.Beta.Runs.RunCreate(createThreadResult.Id, new() { AssistantId = assistantResult.Id });
            var result = await openAI.Beta.Runs.RunCancel(createThreadResult.Id, createRunResult.Id);

            if (result.Successful)
            {
                if (result.Status == RunStatus.Cancelling)
                {
                    ConsoleExtensions.WriteLine("Run Cancel Test is successful.", ConsoleColor.Green);
                }
                else
                {
                    ConsoleExtensions.WriteLine($"Run Cancel Test is failed. The status is {result.Status}.", ConsoleColor.Red);
                }
            }
            else
            {
                ConsoleExtensions.WriteError(result.Error);
            }
        }

        public static async Task ListRunStepsTest(IOpenAIService openAI)
        {
            ConsoleExtensions.WriteLine("List Run Steps Testing is starting:", ConsoleColor.Cyan);
            if (string.IsNullOrWhiteSpace(CreatedRunId))
            {
                ConsoleExtensions.WriteLine("Run Id is not found. Please create a run first.", ConsoleColor.Red);
                return;
            }

            if (string.IsNullOrWhiteSpace(CreatedThreadId))
            {
                ConsoleExtensions.WriteLine("Thread Id is not found. Please create a thread first.", ConsoleColor.Red);
                return;
            }

            var allRuns = new List<RunStepResponse>();
            var hasMore = true;
            var lastId = string.Empty;
            while (hasMore)
            {
                var result = await openAI.Beta.RunSteps.RunStepsList(CreatedThreadId, CreatedRunId, new() { After = lastId, Limit = 5 });
                if (result.Successful)
                {
                    if (result.Data != null)
                    {
                        allRuns.AddRange(result.Data);
                    }

                    hasMore = result.HasMore;
                    lastId = result.LastId;
                }
                else
                {
                    ConsoleExtensions.WriteError(result.Error);
                    return;
                }
            }

            foreach (var run in allRuns)
            {
                Console.WriteLine($"ID: {run.Id}, Status: {run.Status}");
            }
        }

        public static async Task RetrieveRunStepTest(IOpenAIService openAI)
        {
            ConsoleExtensions.WriteLine("Retrieve Run Step Testing is starting:", ConsoleColor.Cyan);
            if (string.IsNullOrWhiteSpace(CreatedRunId))
            {
                ConsoleExtensions.WriteLine("Run Id is not found. Please create a run first.", ConsoleColor.Red);
                return;
            }

            if (string.IsNullOrWhiteSpace(CreatedThreadId))
            {
                ConsoleExtensions.WriteLine("Thread Id is not found. Please create a thread first.", ConsoleColor.Red);
                return;
            }

            var resultStepsList = await openAI.Beta.RunSteps.RunStepsList(CreatedThreadId, CreatedRunId);
            var result = await openAI.Beta.RunSteps.RunStepRetrieve(CreatedThreadId, CreatedRunId, resultStepsList.Data!.First().Id);
            if (result.Successful)
            {
                ConsoleExtensions.WriteLine("Retrieve Run Step Test is successful.", ConsoleColor.Green);
            }
            else
            {
                ConsoleExtensions.WriteError(result.Error);
            }
        }

        public static async Task CreateThreadAndRun(IOpenAIService sdk)
        {
            ConsoleExtensions.WriteLine("Create Thread and Run Testing is starting:", ConsoleColor.Cyan);
            var assistantResult = await sdk.Beta.Assistants.AssistantCreate(new()
            {
                Instructions = "You are a personal math tutor. When asked a question, write and run Python code to answer the question.",
                Name = "Math Tutor",
                Tools = [ToolDefinition.DefineCodeInterpreter()],
                Model = Models.Gpt_4_turbo
            });
            CreatedAssistantId = assistantResult.Id;
            var runResult = await sdk.Beta.Runs.CreateThreadAndRun(new()
            {
                AssistantId = assistantResult.Id,
                Thread = new()
                {
                    Messages =
                    [
                        new()
                        {
                            Role = AssistantMessageRole.User,
                            Content = new("Explain deep learning to a 5 year old.")
                        }
                    ]
                }
            });

            if (runResult.Successful)
            {
                ConsoleExtensions.WriteLine("Create Thread and Run Test is successful.", ConsoleColor.Green);
            }
            else
            {
                ConsoleExtensions.WriteError(runResult.Error);
            }
        }

        public static async Task CreateThreadAndRunAsStream(IOpenAIService sdk)
        {
            ConsoleExtensions.WriteLine("Create Thread and Run As Stream Testing is starting:", ConsoleColor.Cyan);
            var assistantResult = await sdk.Beta.Assistants.AssistantCreate(new()
            {
                Instructions = "You are a personal math tutor. When asked a question, write and run Python code to answer the question.",
                Name = "Math Tutor",
                Tools = [ToolDefinition.DefineCodeInterpreter()],
                Model = Models.Gpt_4_turbo
            });
            CreatedAssistantId = assistantResult.Id;
            var runResult = sdk.Beta.Runs.CreateThreadAndRunAsStream(new()
            {
                AssistantId = assistantResult.Id,
                Thread = new()
                {
                    Messages =
                    [
                        new()
                        {
                            Role = AssistantMessageRole.User,
                            Content = new("Explain deep learning to a 5 year old.")
                        }
                    ]
                }
            });

            await foreach (var run in runResult)
            {
                if (run.Successful)
                {
                    Console.WriteLine($"Event:{run.StreamEvent}");
                    if (run is RunResponse runResponse)
                    {
                        if (runResponse.Status == null)
                        {
                            Console.Write(".");
                        }
                        else
                        {
                            ConsoleExtensions.WriteLine($"Run Id: {runResponse.Id}, Status: {runResponse.Status}");
                        }
                    }

                    else if (run is RunStepResponse runStepResponse)
                    {
                        if (string.IsNullOrEmpty(runStepResponse.Status))
                        {
                            Console.Write(".");
                        }
                        else
                        {
                            ConsoleExtensions.WriteLine($"Run Step Id: {runStepResponse.Id}, Status: {runStepResponse.Status}");
                        }
                    }

                    else if (run is MessageResponse messageResponse)
                    {
                        if (string.IsNullOrEmpty(messageResponse.Id))
                        {
                            Console.Write(".");
                        }
                        else
                        {
                            ConsoleExtensions.WriteLine($"Message Id: {messageResponse.Id}, Message: {messageResponse.Content?.FirstOrDefault()?.Text?.Value}");
                        }
                    }
                    else
                    {
                        if (run.StreamEvent != null)
                        {
                            Console.WriteLine(run.StreamEvent);
                        }
                        else
                        {
                            Console.Write(".");
                        }
                    }
                }
                else
                {
                    ConsoleExtensions.WriteError(run.Error);
                }
            }

            ConsoleExtensions.WriteLine("Create Thread and Run  As Stream Test is successful.", ConsoleColor.Green);
        }

        public static async Task Cleanup(IOpenAIService sdk)
        {
            ConsoleExtensions.WriteLine("Cleanup Testing is starting:", ConsoleColor.Cyan);
            if (!string.IsNullOrWhiteSpace(CreatedThreadId))
            {
                var threadResult = await sdk.Beta.Threads.ThreadDelete(CreatedThreadId);
                if (threadResult.Successful)
                {
                    CreatedThreadId = null;
                    ConsoleExtensions.WriteLine("Thread Deleted Successfully.", ConsoleColor.Green);
                }
                else
                {
                    ConsoleExtensions.WriteError(threadResult.Error);
                }
            }

            if (!string.IsNullOrWhiteSpace(CreatedAssistantId))
            {
                var assistantResult = await sdk.Beta.Assistants.AssistantDelete(CreatedAssistantId);
                if (assistantResult.Successful)
                {
                    CreatedAssistantId = null;
                    ConsoleExtensions.WriteLine("Assistant Deleted Successfully.", ConsoleColor.Green);
                }
                else
                {
                    ConsoleExtensions.WriteError(assistantResult.Error);
                }
            }
        }
    }
}