using OpenAI.Interfaces;
using OpenAI.ObjectModels;
using OpenAI.ObjectModels.RequestModels;

namespace OpenAI.Playground.TestHelpers;

internal static class AudioTestHelper
{
    public static async Task RunSimpleAudioCreateTranscriptionTest(IOpenAIService sdk)
    {
        ConsoleExtensions.WriteLine("Audio Create Transcription Testing is starting:", ConsoleColor.Cyan);

        try
        {
            ConsoleExtensions.WriteLine("Audio Create Transcription Test:", ConsoleColor.DarkCyan);

            const string fileName = "micro-machines.mp3";
            var sampleFile = await FileExtensions.ReadAllBytesAsync($"SampleData/{fileName}");

            ConsoleExtensions.WriteLine($"Uploading file {fileName}", ConsoleColor.DarkCyan);
            var audioResult = await sdk.Audio.CreateTranscription(new AudioCreateTranscriptionRequest
            {
                FileName = fileName,
                File = sampleFile,
                Model = Models.WhisperV1,
                ResponseFormat = StaticValues.AudioStatics.ResponseFormat.VerboseJson
            });


            if (audioResult.Successful)
            {
                Console.WriteLine(string.Join("\n", audioResult.Text));
            }
            else
            {
                if (audioResult.Error == null)
                {
                    throw new Exception("Unknown Error");
                }

                Console.WriteLine($"{audioResult.Error.Code}: {audioResult.Error.Message}");
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public static async Task RunSimpleAudioCreateTranslationTest(IOpenAIService sdk)
    {
        ConsoleExtensions.WriteLine("Audio Create Translation Testing is starting:", ConsoleColor.Cyan);

        try
        {
            ConsoleExtensions.WriteLine("Audio Create Translation Test:", ConsoleColor.DarkCyan);

            const string fileName = "multilingual.mp3";
            var sampleFile = await FileExtensions.ReadAllBytesAsync($"SampleData/{fileName}");

            ConsoleExtensions.WriteLine($"Uploading file {fileName}", ConsoleColor.DarkCyan);
            var audioResult = await sdk.Audio.CreateTranslation(new AudioCreateTranscriptionRequest
            {
                FileName = fileName,
                File = sampleFile,
                Model = Models.WhisperV1,
                ResponseFormat = StaticValues.AudioStatics.ResponseFormat.VerboseJson
            });

            if (audioResult.Successful)
            {
                Console.WriteLine(string.Join("\n", audioResult.Text));
            }
            else
            {
                if (audioResult.Error == null)
                {
                    throw new Exception("Unknown Error");
                }

                Console.WriteLine($"{audioResult.Error.Code}: {audioResult.Error.Message}");
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

     public static async Task RunSimpleAudioCreateSpeechTest(IOpenAIService sdk)
    {
        ConsoleExtensions.WriteLine("Create Speech Testing is starting:", ConsoleColor.Cyan);

        try
        {
            ConsoleExtensions.WriteLine("Audio Create Speech Test:", ConsoleColor.DarkCyan);
            var audioResult = await sdk.Audio.CreateSpeech<Stream>(new AudioCreateSpeechRequest
            {
                Model = Models.Tts_1,
                Input = "The sixth sick sheikh's sixth sheep's sick",
                Voice = StaticValues.AudioStatics.Voice.Alloy,
                ResponseFormat = StaticValues.AudioStatics.ResponseFormat.Mp3,
                Speed = 1.1f
            });

            if (audioResult.Successful)
            {
#if NET6_0_OR_GREATER
                var audio =audioResult.Data!;
                // save stream data as mp3 file
                await using var fileStream = File.Create("SampleData/speech.mp3");
                await audio.CopyToAsync(fileStream);
                //await File.WriteAllBytesAsync("SampleData/speech.mp3", audioByteList);
#endif
                Console.WriteLine("\n Audio content in mp3 format is successfully generated");
            }
            else
            {
                if (audioResult.Error == null)
                {
                    throw new Exception("Unknown Error");
                }

                Console.WriteLine($"{audioResult.Error.Code}: {audioResult.Error.Message}");
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}
