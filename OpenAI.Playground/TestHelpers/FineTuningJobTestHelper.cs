using OpenAI.Interfaces;
using OpenAI.ObjectModels;
using OpenAI.ObjectModels.RequestModels;
using OpenAI.ObjectModels.ResponseModels.FineTuningJobResponseModels;

namespace OpenAI.Playground.TestHelpers;

internal static class FineTuningJobTestHelper
{
    public static async Task RunCaseStudyIsTheModelMakingUntrueStatements(IOpenAIService sdk)
    {
        ConsoleExtensions.WriteLine("Run Case Study Is The Model Making Untrue Statements:", ConsoleColor.Cyan);

        try
        {
            const string fileName = "FineTuningJobSample2.jsonl";
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

            ConsoleExtensions.WriteLine($"Wait 5 seconds for the file to be ready", ConsoleColor.DarkYellow);
            await Task.Delay(5_000);

            var createFineTuningJobResponse = await sdk.FineTuningJob.CreateFineTuningJob(new FineTuningJobCreateRequest
            {
                TrainingFile = uploadFilesResponse.Id,
                Model = Models.Gpt_3_5_Turbo
            });

            var listFineTuningJobEventsStream = await sdk.FineTuningJob.ListFineTuningJobEvents(createFineTuningJobResponse.Id, true);
            using var streamReader = new StreamReader(listFineTuningJobEventsStream);
            while (!streamReader.EndOfStream)
            {
                Console.WriteLine(await streamReader.ReadLineAsync());
            }

            FineTuningJobResponse retrieveFineTuningJobResponse;
            do
            {
                retrieveFineTuningJobResponse = await sdk.FineTuningJob.RetrieveFineTuningJob(createFineTuningJobResponse.Id);
                if (retrieveFineTuningJobResponse.Status == "succeeded" || retrieveFineTuningJobResponse.Status == "cancelled" || retrieveFineTuningJobResponse.Status == "failed")
                {
                    ConsoleExtensions.WriteLine($"Fine-tune Status for {createFineTuningJobResponse.Id}: {retrieveFineTuningJobResponse.Status}.", ConsoleColor.Yellow);
                    break;
                }

                ConsoleExtensions.WriteLine($"Fine-tune Status for {createFineTuningJobResponse.Id}: {retrieveFineTuningJobResponse.Status}. Wait 10 more seconds", ConsoleColor.DarkYellow);
                await Task.Delay(10_000);
            } while (true);

            do
            {
                var completionResult = await sdk.Completions.CreateCompletion(new CompletionCreateRequest
                {
                    MaxTokens = 1,
                    Prompt = @"https://t.co/f93xEd2 Excited to share my latest blog post! ->",
                    Model = retrieveFineTuningJobResponse.FineTunedModel,
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

    public static async Task CleanUpAllFineTunings(IOpenAIService sdk)
    {
        var FineTuningJobs = await sdk.FineTuningJob.ListFineTuningJobs();
        foreach (var datum in FineTuningJobs.Data)
        {
            await sdk.FineTuningJob.DeleteFineTuningJob(datum.FineTunedModel);
        }
    }
}