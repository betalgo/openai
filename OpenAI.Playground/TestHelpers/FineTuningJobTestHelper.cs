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

        var jobs = await sdk.FineTuningJob.ListFineTuningJobs();
        //print all jobs
        foreach (var job in jobs.Data)
        {
            Console.WriteLine(job.FineTunedModel);
        } 

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

            var listFineTuningJobEventsStream = await sdk.FineTuningJob.ListFineTuningJobEvents(new FineTuningJobListEventsRequest
            {
                FineTuningJobId = createFineTuningJobResponse.Id
            }, true);

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
                var completionResult = await sdk.ChatCompletion.CreateCompletion(new ChatCompletionCreateRequest
                {
                    Messages = new List<ChatMessage>
                    {
                        ChatMessage.FromSystem("You are Marv, a chatbot that reluctantly answers questions with sarcastic responses."),
                        ChatMessage.FromUser("How many pounds are in a kilogram?"),
                    },
                    MaxTokens = 50,
                    Model = retrieveFineTuningJobResponse.FineTunedModel
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
}