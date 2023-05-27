using OpenAI.GPT3.Interfaces;
using OpenAI.GPT3.ObjectModels;
using OpenAI.GPT3.ObjectModels.RequestModels;
using OpenAI.GPT3.ObjectModels.ResponseModels.FineTuneResponseModels;

namespace OpenAI.Playground.TestHelpers;

internal static class FineTuningTestHelper
{
    public static async Task RunCaseStudyIsTheModelMakingUntrueStatements(IOpenAIService sdk)
    {
        ConsoleExtensions.WriteLine("Run Case Study Is The Model Making Untrue Statements:", ConsoleColor.Cyan);

        try
        {
            const string fileName = "FineTuningSample1.jsonl";
            var sampleFile = await FileExtensions.ReadAllBytesAsync($"SampleData/{fileName}");

            ConsoleExtensions.WriteLine($"Uploading file {fileName}", ConsoleColor.DarkCyan);
            var uploadFilesResponse = await sdk.Files.FileUpload(UploadFilePurposes.UploadFilePurpose.FineTune, sampleFile, fileName);
            if (uploadFilesResponse.Successful)
            {
                ConsoleExtensions.WriteLine($"{fileName} uploaded", ConsoleColor.DarkGreen);
            }
            else
            {
                ConsoleExtensions.WriteLine($"{fileName} failed", ConsoleColor.DarkRed);
            }

            var createFineTuneResponse = await sdk.FineTunes.CreateFineTune(new FineTuneCreateRequest
            {
                TrainingFile = uploadFilesResponse.Id,
                Model = Models.Ada
            });

            await foreach (var eventResponse in sdk.FineTunes.ListFineTuneEventsStream(createFineTuneResponse.Id))
            {
                Console.WriteLine($"{(eventResponse.CreatedAt != null ? DateTimeOffset.FromUnixTimeSeconds(eventResponse.CreatedAt.Value).ToLocalTime() : string.Empty)} : {eventResponse.Level} : {eventResponse.Object} : {eventResponse.Message}");
            }

            FineTuneResponse retrieveFineTuneResponse;
            do
            {
                retrieveFineTuneResponse = await sdk.FineTunes.RetrieveFineTune(createFineTuneResponse.Id);
                if (retrieveFineTuneResponse.Status == "succeeded" || retrieveFineTuneResponse.Status == "cancelled" || retrieveFineTuneResponse.Status == "failed")
                {
                    ConsoleExtensions.WriteLine($"Fine-tune Status for {createFineTuneResponse.Id}: {retrieveFineTuneResponse.Status}.", ConsoleColor.Yellow);
                    break;
                }

                ConsoleExtensions.WriteLine($"Fine-tune Status for {createFineTuneResponse.Id}: {retrieveFineTuneResponse.Status}. Wait 10 more seconds", ConsoleColor.DarkYellow);
                await Task.Delay(10_000);
            } while (true);

            do
            {
                var completionResult = await sdk.Completions.CreateCompletion(new CompletionCreateRequest
                {
                    MaxTokens = 1,
                    Prompt = @"https://t.co/f93xEd2 Excited to share my latest blog post! ->",
                    Model = retrieveFineTuneResponse.FineTunedModel,
                    LogProbs = 2
                });
                if (completionResult.Successful)
                {
                    Console.WriteLine(completionResult.Choices.FirstOrDefault());
                    break;
                }

                ConsoleExtensions.WriteLine($"failed{completionResult.Error?.Message}", ConsoleColor.DarkRed);
            } while (true);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public static async Task RunCaseListFineTuneEvents(IOpenAIService sdk)
    {
        try
        {
            var jobs = await sdk.FineTunes.ListFineTunes();
            if (!jobs.Successful)
            {
                return;
            }

            foreach (var jobInfo in jobs.Data)
            {
                var jobEvents = await sdk.FineTunes.ListFineTuneEvents(jobInfo.Id);
                if (jobEvents.Successful)
                {
                    foreach (var eventResponse in jobEvents.Data)
                    {
                        Console.WriteLine($"{(eventResponse.CreatedAt != null ? DateTimeOffset.FromUnixTimeSeconds(eventResponse.CreatedAt.Value).ToLocalTime() : string.Empty)} : {eventResponse.Level} : {eventResponse.Object} : {eventResponse.Message}");
                    }
                }
                else
                {
                    ConsoleExtensions.WriteLine($"{jobInfo.Id} failed", ConsoleColor.DarkRed);
                }
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public static async Task RunCaseListFineTuneEventsStream(IOpenAIService sdk)
    {
        try
        {
            var jobs = await sdk.FineTunes.ListFineTunes();
            if (!jobs.Successful)
            {
                return;
            }

            foreach (var jobInfo in jobs.Data)
            {
                await foreach (var eventResponse in sdk.FineTunes.ListFineTuneEventsStream(jobInfo.Id))
                {
                    Console.WriteLine($"{(eventResponse.CreatedAt != null ? DateTimeOffset.FromUnixTimeSeconds(eventResponse.CreatedAt.Value).ToLocalTime() : string.Empty)} : {eventResponse.Level} : {eventResponse.Object} : {eventResponse.Message}");
                }
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public static async Task CleanUpAllFineTunings(IOpenAIService sdk)
    {
        var fineTunes = await sdk.FineTunes.ListFineTunes();
        foreach (var datum in fineTunes.Data)
        {
            await sdk.FineTunes.DeleteFineTune(datum.FineTunedModel);
        }
    }
}