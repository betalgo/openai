using Betalgo.Ranul.OpenAI.ObjectModels.RealtimeModels;
using NAudio.Wave;

namespace OpenAI.Playground.TestHelpers.RealtimeHelpers;

/// <summary>
/// Handles real-time audio playback for OpenAI's audio responses
/// Manages buffering and streaming of audio data
/// </summary>
public class VoiceOutput : IDisposable
{
    // Core components for audio handling
    private readonly BufferedWaveProvider _bufferedWaveProvider;  // Manages audio data buffering
    private readonly WaveOutEvent _waveOut;                      // Handles audio output device
    private bool _isPlaying;                                     // Tracks current playback status

    /// <summary>
    /// Initializes the voice output system with OpenAI's default audio settings
    /// </summary>
    public VoiceOutput()
    {
        // Initialize audio output device
        _waveOut = new();
        // Register for playback stopped events
        _waveOut.PlaybackStopped += OnPlaybackStopped!;

        // Configure audio buffer with OpenAI's default settings
        _bufferedWaveProvider = new(new(
            RealtimeConstants.Audio.DefaultSampleRate,       // Standard sample rate
            RealtimeConstants.Audio.DefaultBitsPerSample,    // Bit depth for audio
            RealtimeConstants.Audio.DefaultChannels          // Number of audio channels
        ))
        {
            BufferLength = 10 * 1024 * 1024,    // Set 10 MB buffer size for smooth playback
            DiscardOnBufferOverflow = true       // Prevent buffer overflow by discarding excess data
        };

        // Connect the buffer to the audio output
        _waveOut.Init(_bufferedWaveProvider);
    }

    /// <summary>
    /// Cleanup resources when object is disposed
    /// </summary>
    public void Dispose()
    {
        // Stop playback and release audio device resources
        _waveOut.Stop();
        _waveOut.Dispose();
    }

    /// <summary>
    /// Add new audio data to the playback queue
    /// Automatically starts playback if not already playing
    /// </summary>
    /// <param name="data">Raw audio data bytes to be played</param>
    public void EnqueueAudioData(byte[]? data)
    {
        // Ignore empty or null data
        if (data == null || data.Length == 0)
            return;

        // Add new audio data to the buffer
        _bufferedWaveProvider.AddSamples(data, 0, data.Length);

        // Start playback if not already playing
        if (!_isPlaying)
        {
            _waveOut.Play();
            _isPlaying = true;
        }
    }

    /// <summary>
    /// Stops playback and clears any remaining buffered audio
    /// </summary>
    public void StopAndClear()
    {
        // Stop playback if currently playing
        if (_isPlaying)
        {
            _waveOut.Stop();
            _isPlaying = false;
        }

        // Clear any remaining audio from buffer
        _bufferedWaveProvider.ClearBuffer();
        Console.WriteLine("Playback stopped and buffer cleared.");
    }

    /// <summary>
    /// Event handler for when playback stops
    /// Restarts playback if there's more data in buffer
    /// </summary>
    private void OnPlaybackStopped(object sender, StoppedEventArgs e)
    {
        // If there's more audio in the buffer, continue playing
        if (_bufferedWaveProvider.BufferedBytes > 0)
        {
            _waveOut.Play();
        }
        // Otherwise, mark playback as stopped
        else
        {
            _isPlaying = false;
        }
    }
}