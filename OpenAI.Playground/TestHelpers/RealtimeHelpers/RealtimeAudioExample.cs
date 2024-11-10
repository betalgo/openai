using System.Text.Json;
using Betalgo.Ranul.OpenAI.Managers;
using Betalgo.Ranul.OpenAI.ObjectModels.RealtimeModels;
using Betalgo.Ranul.OpenAI.ObjectModels.SharedModels;

namespace OpenAI.Playground.TestHelpers.RealtimeHelpers;

public class RealtimeAudioExample : IDisposable
{
    private readonly IOpenAIRealtimeService _ai;
    private readonly VoiceInput _voiceInput;
    private readonly VoiceOutput _voiceOutput;

    public RealtimeAudioExample(IOpenAIRealtimeService ai)
    {
        _ai = ai;
        _voiceInput = new(_ai);
        _voiceOutput = new();
    }

    public void Dispose()
    {
        _voiceInput.Dispose();
        _voiceOutput.Dispose();
        _ai.Dispose();
    }


    public async Task Run()
    {
        SetupEventHandlers();
        await _ai.ConnectAsync();
        await _ai.ClientEvents.Session.Update(new()
        {
            Session = new()
            {
                Instructions = "You are a great, upbeat friend. You made jokes all the time and your voices is full of joy.",
                Voice = "verse",
                Modalities = ["text", "audio"],
                Tools =
                [
                    new()
                    {
                        Type = "function",
                        Name = "get_current_weather",
                        Description = "Get the current weather",
                        Parameters = PropertyDefinition.DefineObject(new Dictionary<string, PropertyDefinition>
                        {
                            { "location", PropertyDefinition.DefineString("The city and state, e.g. San Francisco, CA") },
                            { "unit", PropertyDefinition.DefineEnum(["celsius", "fahrenheit"], string.Empty) }
                        }, ["location"], null, null, null)
                    }
                ]
            }
        });

        Console.WriteLine("Press 'R' to start recording, 'S' to stop, 'Q' to quit");

        while (true)
        {
            var key = Console.ReadKey(true).Key;
            if (key == ConsoleKey.R)
            {
                _voiceInput.StartRecording();
                Console.WriteLine("Recording started...");
            }
            else if (key == ConsoleKey.S)
            {
                await StopAndSendAudio();
            }
            else if (key == ConsoleKey.Q)
            {
                break;
            }
        }
    }

    private async Task StopAndSendAudio()
    {
        _voiceInput.StopRecording();
        Console.WriteLine("Recording stopped.");
        await _ai.ClientEvents.InputAudioBuffer.Commit();
        await _ai.ClientEvents.Response.Create();
    }

    private async Task SendPreRecordedAudio(string filePath)
    {
        Console.WriteLine($"Sending pre-recorded audio: {filePath}");
        await _voiceInput.SendAudioFile(filePath);
        await _ai.ClientEvents.InputAudioBuffer.Commit();
    }

    private void SetupEventHandlers()
    {
        // Handle server events related to audio input
        _ai.ServerEvents.Conversation.Item.InputAudioTranscription.OnCompleted += (sender, args) => { Console.WriteLine($"Transcription completed: {args.Transcript}"); };
        _ai.ServerEvents.Conversation.Item.InputAudioTranscription.OnFailed += (sender, args) => { Console.WriteLine($"Transcription failed: {args.Error}"); };
        _ai.ServerEvents.InputAudioBuffer.OnCommitted += (sender, args) => { Console.WriteLine("Audio buffer committed."); };
        _ai.ServerEvents.InputAudioBuffer.OnCleared += (sender, args) => { Console.WriteLine("Audio buffer cleared."); };
        _ai.ServerEvents.InputAudioBuffer.OnSpeechStopped += (sender, args) => { Console.WriteLine("Speech stopped detected."); };
        _ai.ServerEvents.InputAudioBuffer.OnSpeechStarted += async (sender, args) =>
        {
            Console.WriteLine("Speech started detected.");
            _voiceOutput.StopAndClear();

            // Optionally, notify the server to cancel any ongoing responses
            await _ai.ClientEvents.Response.Cancel();
        };
        _ai.ServerEvents.Response.AudioTranscript.OnDelta += (sender, args) =>
        {
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.Write($"{args.Delta}");
            Console.ResetColor();
        };
        _ai.ServerEvents.Response.Audio.OnDelta += (sender, args) =>
        {
            try
            {
                if (!string.IsNullOrEmpty(args.Delta))
                {
                    var audioData = Convert.FromBase64String(args.Delta);
                    _voiceOutput.EnqueueAudioData(audioData);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error processing audio delta: {ex.Message}");
            }
        };
        _ai.ServerEvents.Response.Audio.OnDone += (sender, args) =>
        {
            Console.WriteLine();
            Console.WriteLine("Audio response completed.");
        };


        _ai.ServerEvents.Response.FunctionCallArguments.OnDelta += (sender, args) =>
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"Function call arguments delta: {args.Delta}");
            Console.ResetColor();
        };

        _ai.ServerEvents.Response.FunctionCallArguments.OnDone += async (sender, args) =>
        {
            if (args.Arguments != null)
            {
                Console.WriteLine($"Function call completed: {args.Arguments}");
                if (args.Name == "get_current_weather")
                {
                    await HandleWeatherFunction(args.Arguments, args.CallId);
                }
            }
        };
        _ai.ServerEvents.OnError += (sender, args) => { Console.WriteLine($"Error: {args.Error.Message}"); };
        //for debug
        //_ai.ServerEvents.OnAll += (sender, args) => { Console.WriteLine($"Received response: {args}"); };
    }

    private async Task HandleWeatherFunction(string arguments, string callId)
    {
        try
        {
            var args = JsonSerializer.Deserialize<WeatherArgs>(arguments);
            // Simulate weather API call
            var weatherResult = new
            {
                temperature = args.unit == "celsius" ? 22 : 72,
                unit = args.unit,
                description = "Sunny with light clouds",
                location = args.location
            };

            // Create function output
            await _ai.ClientEvents.Conversation.Item.Create(new()
            {
                Item = new()
                {
                    Type = ItemType.FunctionCallOutput,
                    CallId = callId,
                    Output = JsonSerializer.Serialize(weatherResult)
                }
            });

            // Generate new response
            await _ai.ClientEvents.Response.Create();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error handling weather function: {ex.Message}");
        }
    }

    private class WeatherArgs
    {
        public string location { get; set; }
        public string unit { get; set; }
    }
}