using System.Text.Json;
using Betalgo.Ranul.OpenAI.Managers;
using Betalgo.Ranul.OpenAI.ObjectModels.RealtimeModels;
using Betalgo.Ranul.OpenAI.ObjectModels.SharedModels;

namespace OpenAI.Playground.TestHelpers.RealtimeHelpers;

/// <summary>
/// A comprehensive example implementation of OpenAI's Realtime API for audio interactions.
/// This class demonstrates how to:
/// - Establish and maintain a WebSocket connection with OpenAI's Realtime server
/// - Handle bidirectional audio streaming
/// - Process transcriptions and responses
/// - Implement function calling capabilities
/// - Manage the full lifecycle of a realtime conversation
/// </summary>
public class RealtimeAudioExample : IDisposable
{
    // Core services for the realtime interaction
    private readonly IOpenAIRealtimeService _ai;        // Manages the WebSocket connection and event handling
    private readonly VoiceInput _voiceInput;            // Handles audio input capture and processing
    private readonly VoiceOutput _voiceOutput;          // Manages audio output playback

    /// <summary>
    /// Initializes a new instance of the RealtimeAudioExample.
    /// Sets up the necessary components for audio interaction with OpenAI's Realtime API.
    /// </summary>
    /// <param name="ai">The OpenAI Realtime service instance that will manage the WebSocket connection</param>
    public RealtimeAudioExample(IOpenAIRealtimeService ai)
    {
        _ai = ai;
        _voiceInput = new(_ai);    // Initialize audio input handling
        _voiceOutput = new();       // Initialize audio output handling
    }

    /// <summary>
    /// Implements IDisposable to properly clean up resources.
    /// This is crucial for releasing audio hardware and closing network connections.
    /// </summary>
    public void Dispose()
    {
        _voiceInput.Dispose();      // Release audio input resources
        _voiceOutput.Dispose();     // Release audio output resources
        _ai.Dispose();              // Close WebSocket connection and clean up
    }

    /// <summary>
    /// Main execution method that orchestrates the entire realtime interaction.
    /// This method:
    /// 1. Sets up all necessary event handlers
    /// 2. Establishes the WebSocket connection
    /// 3. Configures the initial session parameters
    /// 4. Handles user input for recording control
    /// </summary>
    public async Task Run()
    {
        // Initialize all event handlers before connecting
        SetupEventHandlers();

        // Establish WebSocket connection to OpenAI's Realtime server
        // This creates a new session and prepares for bi-directional communication
        await _ai.ConnectAsync();

        // Configure the session with initial settings using session.update event
        // This configuration defines how the AI will behave and what capabilities it has
        await _ai.ClientEvents.Session.Update(new()
        {
            Session = new()
            {
                // Define the AI's personality and behavior
                // This is similar to system messages in the regular Chat API
                Instructions = "You are a great, upbeat friend. You made jokes all the time and your voices is full of joy.",

                // Select the voice for audio responses
                // Options in Realtime API: 'alloy', 'echo', 'fable', 'onyx', 'nova', 'shimmer'
                Voice = "verse",

                // Enable both text and audio capabilities
                // This allows the AI to respond with both text transcriptions and spoken audio
                Modalities = ["text", "audio"],

                // Define tools (functions) that the AI can call during conversation
                // This example implements a weather checking function
                Tools =
                [
                    new()
                    {
                        Type = "function",
                        Name = "get_current_weather",
                        Description = "Get the current weather",
                        // Define the function parameters using JSON Schema
                        Parameters = PropertyDefinition.DefineObject(new Dictionary<string, PropertyDefinition>
                        {
                            // Location parameter is required
                            { "location", PropertyDefinition.DefineString("The city and state, e.g. San Francisco, CA") },
                            // Unit parameter is optional but must be either celsius or fahrenheit
                            { "unit", PropertyDefinition.DefineEnum(["celsius", "fahrenheit"], string.Empty) }
                        }, ["location"], null, null, null)
                    }
                ]
            }
        });

        // Main interaction loop - Handle user commands for recording
        Console.WriteLine("Press 'R' to start recording, 'S' to stop, 'Q' to quit");
        while (true)
        {
            var key = Console.ReadKey(true).Key;
            switch (key)
            {
                case ConsoleKey.R:
                    // Start capturing audio input
                    _voiceInput.StartRecording();
                    Console.WriteLine("Recording started...");
                    break;

                case ConsoleKey.S:
                    // Stop recording and process the audio
                    await StopAndSendAudio();
                    break;

                case ConsoleKey.Q:
                    // Exit the application
                    return;
            }
        }
    }

    /// <summary>
    /// Handles the process of stopping audio recording and sending it to OpenAI.
    /// This method:
    /// 1. Stops the audio recording
    /// 2. Commits the recorded audio buffer to create a user message
    /// 3. Requests an AI response
    /// </summary>
    private async Task StopAndSendAudio()
    {
        // Stop capturing audio input
        _voiceInput.StopRecording();
        Console.WriteLine("Recording stopped.");

        // Commit the audio buffer to create a user message
        // This triggers the input_audio_buffer.commit event
        await _ai.ClientEvents.InputAudioBuffer.Commit();

        // Request an AI response for the committed audio
        // This triggers the response.create event
        await _ai.ClientEvents.Response.Create();
    }

    /// <summary>
    /// Utility method to send pre-recorded audio files to the API.
    /// This is useful for testing or processing existing audio files.
    /// </summary>
    /// <param name="filePath">Path to the audio file to be sent</param>
    private async Task SendPreRecordedAudio(string filePath)
    {
        Console.WriteLine($"Sending pre-recorded audio: {filePath}");
        // Send the audio file contents
        await _voiceInput.SendAudioFile(filePath);
        // Commit the audio buffer to create a user message
        await _ai.ClientEvents.InputAudioBuffer.Commit();
    }

    /// <summary>
    /// Sets up all event handlers for the realtime session.
    /// This method configures handlers for:
    /// - Audio input processing and transcription
    /// - Speech detection
    /// - AI response processing
    /// - Function calls
    /// - Error handling
    /// 
    /// Each event handler corresponds to specific server events as defined in the OpenAI Realtime API documentation.
    /// </summary>
    private void SetupEventHandlers()
    {
        // AUDIO INPUT HANDLING EVENTS

        // Handle successful audio transcriptions
        // This event is triggered when input audio is successfully converted to text
        _ai.ServerEvents.Conversation.Item.InputAudioTranscription.OnCompleted += (sender, args) => {
            Console.WriteLine($"Transcription completed: {args.Transcript}");
        };

        // Handle failed transcription attempts
        // This helps identify issues with audio quality or processing
        _ai.ServerEvents.Conversation.Item.InputAudioTranscription.OnFailed += (sender, args) => {
            Console.WriteLine($"Transcription failed: {args.Error}");
        };

        // AUDIO BUFFER STATE EVENTS

        // Triggered when audio buffer is successfully committed
        // This indicates the audio has been properly sent to the server
        _ai.ServerEvents.InputAudioBuffer.OnCommitted += (sender, args) => {
            Console.WriteLine("Audio buffer committed.");
        };

        // Triggered when audio buffer is cleared
        // This happens when starting fresh or discarding unused audio
        _ai.ServerEvents.InputAudioBuffer.OnCleared += (sender, args) => {
            Console.WriteLine("Audio buffer cleared.");
        };

        // SPEECH DETECTION EVENTS

        // Handle speech end detection
        // This helps in identifying when the user has finished speaking
        _ai.ServerEvents.InputAudioBuffer.OnSpeechStopped += (sender, args) => {
            Console.WriteLine("Speech stopped detected.");
        };

        // Handle speech start detection
        // This is useful for implementing real-time interaction
        _ai.ServerEvents.InputAudioBuffer.OnSpeechStarted += async (sender, args) =>
        {
            Console.WriteLine("Speech started detected.");
            // Clear any ongoing audio output when user starts speaking
            _voiceOutput.StopAndClear();

            // Cancel any in-progress AI responses
            // This ensures a more natural conversation flow
            await _ai.ClientEvents.Response.Cancel();
        };

        // AI RESPONSE HANDLING EVENTS

        // Handle incoming text transcripts from the AI
        // This shows what the AI is saying in text form
        _ai.ServerEvents.Response.AudioTranscript.OnDelta += (sender, args) =>
        {
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.Write($"{args.Delta}");
            Console.ResetColor();
        };

        // AUDIO OUTPUT HANDLING

        // Process incoming audio data from the AI
        // This handles the AI's voice response in chunks
        _ai.ServerEvents.Response.Audio.OnDelta += (sender, args) =>
        {
            try
            {
                if (!string.IsNullOrEmpty(args.Delta))
                {
                    // Convert base64 audio data to bytes and queue for playback
                    var audioData = Convert.FromBase64String(args.Delta);
                    _voiceOutput.EnqueueAudioData(audioData);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error processing audio delta: {ex.Message}");
            }
        };

        // Handle completion of audio response
        _ai.ServerEvents.Response.Audio.OnDone += (sender, args) =>
        {
            Console.WriteLine();
            Console.WriteLine("Audio response completed.");
        };

        // FUNCTION CALLING EVENTS

        // Handle incoming function call arguments
        // This shows the AI's attempts to use tools/functions
        _ai.ServerEvents.Response.FunctionCallArguments.OnDelta += (sender, args) =>
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"Function call arguments delta: {args.Delta}");
            Console.ResetColor();
        };

        // Process completed function calls
        _ai.ServerEvents.Response.FunctionCallArguments.OnDone += async (sender, args) =>
        {
            if (args.Arguments != null)
            {
                Console.WriteLine($"Function call completed: {args.Arguments}");
                // Handle weather function calls specifically
                if (args.Name == "get_current_weather")
                {
                    await HandleWeatherFunction(args.Arguments, args.CallId);
                }
            }
        };

        // ERROR HANDLING

        // Global error handler for any API errors
        _ai.ServerEvents.OnError += (sender, args) => {
            Console.WriteLine($"Error: {args.Error.Message}");
        };
    }

    /// <summary>
    /// Handles weather function calls from the AI.
    /// This method:
    /// 1. Parses the function arguments
    /// 2. Simulates a weather API call
    /// 3. Returns the results to the AI
    /// 4. Triggers a new response based on the weather data
    /// </summary>
    /// <param name="arguments">JSON string containing the function arguments</param>
    /// <param name="callId">Unique identifier for the function call</param>
    private async Task HandleWeatherFunction(string arguments, string callId)
    {
        try
        {
            // Parse the weather query arguments
            var args = JsonSerializer.Deserialize<WeatherArgs>(arguments);

            // Simulate getting weather data
            // In a real application, this would call an actual weather API
            var weatherResult = new
            {
                temperature = args.unit == "celsius" ? 22 : 72,
                unit = args.unit,
                description = "Sunny with light clouds",
                location = args.location
            };

            // Send the weather data back to the conversation
            // This creates a function_call_output item in the conversation
            await _ai.ClientEvents.Conversation.Item.Create(new()
            {
                Item = new()
                {
                    Type = ItemType.FunctionCallOutput,
                    CallId = callId,
                    Output = JsonSerializer.Serialize(weatherResult)
                }
            });

            // Request a new AI response based on the weather data
            await _ai.ClientEvents.Response.Create();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error handling weather function: {ex.Message}");
        }
    }

    /// <summary>
    /// Data model for weather function arguments.
    /// This class maps to the JSON schema defined in the function parameters.
    /// </summary>
    private class WeatherArgs
    {
        public string location { get; set; }    // Required: city and state
        public string unit { get; set; }        // Optional: celsius or fahrenheit
    }
}