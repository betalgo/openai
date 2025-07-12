using System.Text;
using Betalgo.Ranul.OpenAI.Builders;
using Betalgo.Ranul.OpenAI.Contracts.Enums;
using Betalgo.Ranul.OpenAI.Interfaces;
using Betalgo.Ranul.OpenAI.ObjectModels;
using Betalgo.Ranul.OpenAI.ObjectModels.RequestModels;
using Betalgo.Ranul.OpenAI.ObjectModels.SharedModels;
using OpenAI.Playground.ExtensionsAndHelpers;

namespace OpenAI.Playground.TestHelpers.AssistantHelpers;

internal static class AssistantTestHelper3
{
    public static async Task RunTests(IOpenAIService sdk)
    {
        await RunAssistantApiTest(sdk);
        await RunHowAssistantsWorkTest(sdk);
    }

    /// <summary>
    ///     Test Assistant api
    /// </summary>
    /// <param name="sdk"></param>
    /// <returns></returns>
    public static async Task RunAssistantApiTest(IOpenAIService sdk)
    {
        ConsoleExtensions.WriteLine("Assistant APT Testing is starting:");

        #region Create assistant

        var func = new FunctionDefinitionBuilder("get_corp_location", "get location of corp").AddParameter("name", PropertyDefinition.DefineString("company name, e.g. Betterway")).Validate().Build();

        ConsoleExtensions.WriteLine("Assistant Create Test:", ConsoleColor.DarkCyan);
        var assistantResult = await sdk.Beta.Assistants.AssistantCreate(new()
        {
            Instructions = "You are a professional assistant who provides company information. Company-related data comes from uploaded questions and does not provide vague answers, only clear answers.",
            Name = "Qicha",
            Tools = new() { ToolDefinition.DefineCodeInterpreter(), ToolDefinition.DefineFileSearch(), ToolDefinition.DefineFunction(func) },
            Model = Models.Gpt_3_5_Turbo_1106
        });
        if (assistantResult.Successful)
        {
            var assistant = assistantResult;
            ConsoleExtensions.WriteLine(assistant.ToJson());
        }
        else
        {
            ConsoleExtensions.WriteLine($"{assistantResult.Error?.Code}: {assistantResult.Error?.Message}");
            return;
        }

        var assistantId = assistantResult.Id;
        ConsoleExtensions.WriteLine($"assistantId:{assistantId} ");

        #endregion


        #region // Assistant List

        ConsoleExtensions.WriteLine("Assistant list:", ConsoleColor.DarkCyan);
        var asstListResult = await sdk.Beta.Assistants.AssistantList();
        if (asstListResult.Successful)
        {
            ConsoleExtensions.WriteLine($"asst list: {asstListResult.Data?.ToJson()}");
        }
        else
        {
            ConsoleExtensions.WriteLine($"{asstListResult.Error?.Code}: {asstListResult.Error?.Message}");
            return;
        }

        #endregion

        #region // Assistant modify

        ConsoleExtensions.WriteLine("Assistant modify:", ConsoleColor.DarkCyan);
        var asstResult = await sdk.Beta.Assistants.AssistantModify(assistantId, new()
        {
            Name = "Qicha rename"
        });
        if (asstResult.Successful)
        {
            ConsoleExtensions.WriteLine(asstResult.ToJson());
        }
        else
        {
            ConsoleExtensions.WriteLine($"{asstResult.Error?.Code}: {asstResult.Error?.Message}");
            return;
        }

        #endregion

        #region // Assistant retrieve

        ConsoleExtensions.WriteLine("Assistant retrieve:", ConsoleColor.DarkCyan);
        var asstRetrieveResult = await sdk.Beta.Assistants.AssistantRetrieve(assistantId);
        if (asstRetrieveResult.Successful)
        {
            ConsoleExtensions.WriteLine(asstRetrieveResult.ToJson());
        }
        else
        {
            ConsoleExtensions.WriteLine($"{asstRetrieveResult.Error?.Code}: {asstRetrieveResult.Error?.Message}");
            return;
        }

        #endregion

        #region // Assistant delete

        ConsoleExtensions.WriteLine("Assistant delete:", ConsoleColor.DarkCyan);
        var deleteResult = await sdk.Beta.Assistants.AssistantDelete(assistantId);
        if (deleteResult.Successful)
        {
            ConsoleExtensions.WriteLine(deleteResult.ToJson());
        }
        else
        {
            ConsoleExtensions.WriteLine($"{deleteResult.Error?.Code}: {deleteResult.Error?.Message}");
        }

        #endregion
    }

    /// <summary>
    ///     How Assistants work
    ///     see <a href:="https://platform.openai.com/docs/assistants/how-it-works">how-it-works</a>
    /// </summary>
    /// <param name="sdk"></param>
    /// <returns></returns>
    public static async Task RunHowAssistantsWorkTest(IOpenAIService sdk)
    {
        ConsoleExtensions.WriteLine("How assistant work Testing is starting:", ConsoleColor.DarkCyan);

        #region //upload file

        const string fileName = "betterway_corp.csv";
        var sampleFile = await FileExtensions.ReadAllBytesAsync($"SampleData/{fileName}");
        var sampleFileAsString = Encoding.UTF8.GetString(sampleFile);

        // HACK: drp052424 - CSV format is not supported for Assistant, so we tell OpenAI it's a TXT. A better solution would be to update the test asset.
        var fileNameForOpenAI = Path.ChangeExtension(fileName, ".txt");

        ConsoleExtensions.WriteLine($"Uploading file: {fileName}", ConsoleColor.DarkCyan);
        var uploadFilesResponse = await sdk.Files.FileUpload(UploadFilePurposes.UploadFilePurpose.Assistants, sampleFile, fileNameForOpenAI);
        if (uploadFilesResponse.Successful)
        {
            ConsoleExtensions.WriteLine($"{fileName} uploaded", ConsoleColor.DarkGreen);
        }
        else
        {
            ConsoleExtensions.WriteLine($"{fileName} failed", ConsoleColor.DarkRed);
            return;
        }

        var uploadFileId = uploadFilesResponse.Id;
        ConsoleExtensions.WriteLine($"uploadFileId:{uploadFileId}, purpose:{uploadFilesResponse.Purpose}");

        #endregion

        #region //create assistants

        var func = new FunctionDefinitionBuilder("get_corp_location", "get location of corp").AddParameter("name", PropertyDefinition.DefineString("company name, e.g. Betterway")).Validate().Build();

        ConsoleExtensions.WriteLine("Assistant Create Test:", ConsoleColor.DarkCyan);
        var assistantResult = await sdk.Beta.Assistants.AssistantCreate(new()
        {
            Instructions = "You are a professional assistant who provides company information. Company-related data comes from uploaded questions and does not provide vague answers, only clear answers.",
            Name = "Qicha",
            Model = Models.Gpt_3_5_Turbo_1106,
            Tools = [ToolDefinition.DefineCodeInterpreter(), ToolDefinition.DefineFileSearch(), ToolDefinition.DefineFunction(func)]
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

        #region //create thread message

        ConsoleExtensions.WriteLine("Message Create Test:", ConsoleColor.DarkCyan);
        var messageResult = await sdk.Beta.Messages.CreateMessage(threadId, new()
        {
            Role = AssistantMessageRoleEnum.User,
            Content = new("Where is Zhejiang Jiacheng Supply Chain Co., LTD."),
            // Tools must be specified for Attachments
            Attachments =
            [
                new()
                {
                    FileId = uploadFileId,
                    Tools = [ToolDefinition.DefineFileSearch()]
                }
            ]
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

        #region //create run

        ConsoleExtensions.WriteLine("Run Create Test:", ConsoleColor.DarkCyan);
        var runResult = await sdk.Beta.Runs.RunCreate(threadId, new()
        {
            AssistantId = assistantId
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

        #region //waiting for run completed

        ConsoleExtensions.WriteLine("waiting for run completed:", ConsoleColor.DarkCyan);
        var runningStatusList = new List<RunStatusEnum>()
        {
            RunStatusEnum.Queued,
            RunStatusEnum.InProgress,
            RunStatusEnum.RequiresAction
        };

        //Get task information
        var runRetrieveResult = await sdk.Beta.Runs.RunRetrieve(threadId, runId);
        while (runningStatusList.Contains(runRetrieveResult.Status))
        {
            /*
             * When a run has the status: "requires_action" and required_action.type is submit_tool_outputs,
             * this endpoint can be used to submit the outputs from the tool calls once they're all completed.
             * All outputs must be submitted in a single request.
             */
            var requireAction = runRetrieveResult.RequiredAction;
            if (runRetrieveResult.Status == RunStatusEnum.RequiresAction && requireAction != null && requireAction.Type == RequiredActionTypeEnum.SubmitToolOutputs)
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
                            Output = @"Zhejiang Jiacheng Supply Chain Co., Ltd. is located in Jiacheng International Business Center, 
No.615 Bayi North Street, Wucheng District, Jinhua City, Zhejiang Province"
                        };
                        toolOutputs.Add(toolOutput);
                    }
                }

                //All outputs must be submitted in a single request.
                if (toolOutputs.Any())
                {
                    await sdk.Beta.Runs.RunSubmitToolOutputs(threadId, runId, new()
                    {
                        ToolOutputs = toolOutputs
                    });
                }

                await Task.Delay(500);
            }

            runRetrieveResult = await sdk.Beta.Runs.RunRetrieve(threadId, runId);
            if (!runningStatusList.Contains(runRetrieveResult.Status))
            {
                break;
            }
        }

        #endregion

        #region // message list

        //Get the final message result
        ConsoleExtensions.WriteLine("Message list:", ConsoleColor.DarkCyan);
        var messageListResult = await sdk.Beta.Messages.ListMessages(threadId);
        if (messageListResult.Successful)
        {
            var msgRespList = messageListResult.Data;
            var ask = msgRespList?.FirstOrDefault(msg => msg.Role == AssistantMessageRoleEnum.User);
            var replys = msgRespList?.Where(msg => msg.CreatedAt > ask?.CreatedAt && msg.Role == AssistantMessageRoleEnum.Assistant).ToList() ?? new List<MessageResponse>();
            ConsoleExtensions.WriteLine(replys.ToJson());
        }
        else
        {
            ConsoleExtensions.WriteLine($"{messageListResult.Error?.Code}: {messageListResult.Error?.Message}");
        }

        #endregion
    }
}