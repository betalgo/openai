using OpenAI.Builders;
using OpenAI.Interfaces;
using OpenAI.ObjectModels;
using OpenAI.ObjectModels.RequestModels;
using OpenAI.ObjectModels.SharedModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OpenAI.Playground.TestHelpers
{
    internal static class AssistantTestHelper
    {
        public static async Task RunAssistantCreateTest(IOpenAIService sdk)
        {
            ConsoleExtensions.WriteLine("Assistant create Testing is starting:", ConsoleColor.Cyan);

            var td1 = new ToolDefinition()
            {
                Type = StaticValues.ToolCallTypes.CodeInterpreter,
            };
            var td2 = new ToolDefinition()
            {
                Type = StaticValues.ToolCallTypes.Retrieval,
            };
            var td3 = new ToolDefinition()
            {
                Type = StaticValues.ToolCallTypes.Function,
                Function = new FunctionDefinitionBuilder("get_current_weather", "Get the current weather")
                .AddParameter("location", PropertyDefinition.DefineString("The city and state, e.g. San Francisco, CA"))
                .AddParameter("format", PropertyDefinition.DefineEnum(new List<string> { "celsius", "fahrenheit" }, "The temperature unit to use. Infer this from the users location."))
                .Validate()
                .Build(),
            };

            try
            {
                ConsoleExtensions.WriteLine("Assistant Create Test:", ConsoleColor.DarkCyan);
                var assistantResult = await sdk.Beta.Assistants.AssistantCreate(new AssistantCreateRequest
                {
                    Instructions = "You are a personal math tutor. When asked a question, write and run Python code to answer the question.",
                    Name = "Math Tutor",
                    Tools = new List<ToolDefinition>() { td1, td2, td3 },
                    Model = Models.Gpt_3_5_Turbo_1106
                });

                if (assistantResult.Successful)
                {
                    var assistant = assistantResult;
                    ConsoleExtensions.WriteLine(assistant.ToJson());
                }
                else
                {
                    if (assistantResult.Error == null)
                    {
                        throw new Exception("Unknown Error");
                    }

                    ConsoleExtensions.WriteLine($"{assistantResult.Error.Code}: {assistantResult.Error.Message}");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        /// <summary>
        /// How Assistants work
        /// see <a href:="https://platform.openai.com/docs/assistants/how-it-works">how-it-works</a>
        /// </summary>
        /// <param name="sdk"></param>
        /// <returns></returns>
        public static async Task RunHowAssistantsWorkTest(IOpenAIService sdk)
        {
            ConsoleExtensions.WriteLine("Assistant work Testing is starting:", ConsoleColor.Cyan);

            #region//upload file
            const string fileName = "betterway_corp.csv";
            var sampleFile = await FileExtensions.ReadAllBytesAsync($"SampleData/{fileName}");
            var sampleFileAsString = Encoding.UTF8.GetString(sampleFile);

            ConsoleExtensions.WriteLine($"Uploading file: {fileName}", ConsoleColor.DarkCyan);
            var uploadFilesResponse = await sdk.Files.FileUpload(UploadFilePurposes.UploadFilePurpose.Assistants, sampleFile, fileName);
            if (uploadFilesResponse.Successful)
            {
                ConsoleExtensions.WriteLine($"{fileName} uploaded", ConsoleColor.DarkGreen);
            }
            else
            {
                ConsoleExtensions.WriteLine($"{fileName} failed", ConsoleColor.DarkRed);
                return;
            }
            var uplaodFileId = uploadFilesResponse.Id;
            ConsoleExtensions.WriteLine($"uplaodFileId:{uplaodFileId}, purpose:{uploadFilesResponse.Purpose}");
            #endregion

            #region//create assistants
            var td1 = new ToolDefinition()
            {
                Type = StaticValues.ToolCallTypes.CodeInterpreter,
            };
            var td2 = new ToolDefinition()
            {
                Type = StaticValues.ToolCallTypes.Retrieval,
            };
            var td3 = new ToolDefinition()
            {
                Type = StaticValues.ToolCallTypes.Function,
                Function = new FunctionDefinitionBuilder("get_corp_location", "获取公司位置")
                .AddParameter("name", PropertyDefinition.DefineString("公司名称，例如：佳程供应链"))
                .Validate()
                .Build(),
            };

            ConsoleExtensions.WriteLine("Assistant Create Test:", ConsoleColor.DarkCyan);
            var assistantResult = await sdk.Beta.Assistants.AssistantCreate(new AssistantCreateRequest
            {
                Name = "企查查",
                Instructions = "你是一个专业提供公司信息的助手。公司相关数据来自上传的问题，不提供模糊不清的回答，只提供明确的答案",
                Model = Models.Gpt_3_5_Turbo_1106,
                Tools = new List<ToolDefinition>() { td1, td2, td3 },
                FileIds = new List<string>() { uplaodFileId },
            });

            if (assistantResult.Successful)
            {
                ConsoleExtensions.WriteLine(assistantResult.ToJson());
            }
            else
            {
                ConsoleExtensions.WriteLine($"{assistantResult.Error?.Code}: {assistantResult.Error?.Message}");
                return;
            }
            var assistantId = assistantResult.Id;
            ConsoleExtensions.WriteLine($"assistantId:{assistantId} ");
            #endregion

            #region //create thread
            ConsoleExtensions.WriteLine("Thread Create Test:", ConsoleColor.DarkCyan);
            var threadResult = await sdk.Beta.Threads.ThreadCreate();
            if (threadResult.Successful)
            {
                ConsoleExtensions.WriteLine(threadResult.ToJson());
            }
            else
            {
                ConsoleExtensions.WriteLine($"{threadResult.Error?.Code}: {threadResult.Error?.Message}");
                return;
            }
            var threadId = threadResult.Id;
            ConsoleExtensions.WriteLine($"threadId: {threadId}");
            #endregion

            #region//create thread message
            ConsoleExtensions.WriteLine("Message Create Test:", ConsoleColor.DarkCyan);
            var messageResult = await sdk.Beta.Messages.MessageCreate(threadId, new MessageCreateRequest
            {
                Role = StaticValues.AssistatntsStatics.MessageStatics.Roles.User,
                Content = "浙江佳程供应链有限公司具体在哪",
                FileIds = new List<string>() { uplaodFileId },
            });

            if (messageResult.Successful)
            {
                ConsoleExtensions.WriteLine(messageResult.ToJson());
            }
            else
            {
                ConsoleExtensions.WriteLine($"{messageResult.Error?.Code}: {messageResult.Error?.Message}");
                return;
            }
            var messageId = messageResult.Id;
            #endregion

            #region//create run
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
                ConsoleExtensions.WriteLine($"{runResult.Error?.Code}: {runResult.Error?.Message}");
                return;
            }

            var runId = runResult.Id;
            ConsoleExtensions.WriteLine($"runId: {runId}");
            #endregion

            #region//waiting for run completed
            ConsoleExtensions.WriteLine("waiting for run completed:", ConsoleColor.DarkCyan);
            var doneStatusList = new List<string>() { StaticValues.AssistatntsStatics.RunStatus.Cancelled, StaticValues.AssistatntsStatics.RunStatus.Completed, StaticValues.AssistatntsStatics.RunStatus.Failed, StaticValues.AssistatntsStatics.RunStatus.Expired };

            //获取任务信息
            var runRetrieveResult = await sdk.Beta.Runs.RunRetrieve(threadId, runId);
            var runStatus = runRetrieveResult.Status;

            while (!doneStatusList.Contains(runStatus))
            {
                /*
                * When a run has the status: "requires_action" and required_action.type is submit_tool_outputs, 
                * this endpoint can be used to submit the outputs from the tool calls once they're all completed. 
                * All outputs must be submitted in a single request.
                */
                var requireAction = runRetrieveResult.RequiredAction;
                if (runStatus == StaticValues.AssistatntsStatics.RunStatus.RequiresAction
                    && requireAction != null && requireAction.Type == StaticValues.AssistatntsStatics.RequiredActionTypes.SubmitToolOutputs)
                {
                    var myFunc = new List<string>() { "get_corp_location" };
                    var toolOutputs = new List<ToolOutput>();
                    foreach (var toolCall in requireAction.SubmitToolOutputs.ToolCalls)
                    {
                        ConsoleExtensions.WriteLine($"ToolCall:{toolCall?.ToJson()}");
                        if (toolCall?.FunctionCall == null) continue;

                        var funcName = toolCall.FunctionCall.Name;
                        if (myFunc.Contains(funcName))
                        {
                            //do sumbit tool
                            var toolOutput = new ToolOutput()
                            {
                                ToolCallId = toolCall.Id,
                                Output = "浙江佳程供应链有限公司,位于浙江省金华市婺城区八一北街615号佳程国际商务中心[from ToolOutput]",
                            };
                            toolOutputs.Add(toolOutput);
                        }
                    }

                    //All outputs must be submitted in a single request.
                    if (toolOutputs.Any())
                    {
                        await sdk.Beta.Runs.RunSubmitToolOutputs(threadId, runId, new SubmitToolOutputsToRunRequest()
                        {
                            ToolOutputs = toolOutputs,
                        });
                    }

                    await Task.Delay(500);
                }

                runRetrieveResult = await sdk.Beta.Runs.RunRetrieve(threadId, runId);
                runStatus = runRetrieveResult.Status;
                if (doneStatusList.Contains(runStatus)) { break; }

            }

            #endregion


            //获取最终消息结果
            ConsoleExtensions.WriteLine("Message list:", ConsoleColor.DarkCyan);
            var messageListResult = await sdk.Beta.Messages.MessageList(threadId);
            if (messageListResult.Successful)
            {
                ConsoleExtensions.WriteLine(messageListResult.ToJson());
            }
            else
            {
                ConsoleExtensions.WriteLine($"{messageListResult.Error?.Code}: {messageListResult.Error?.Message}");
                return;
            }

        }
    }
}
