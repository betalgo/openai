using Betalgo.Ranul.OpenAI.Managers;
using NAudio.Wave;

namespace OpenAI.Playground.TestHelpers.RealtimeHelpers;

/// <summary>
/// Handles voice input capture and processing for real-time communication with OpenAI's API.
/// This class manages audio recording, buffering, and transmission of audio data.
/// </summary>
public class VoiceInput : IDisposable
{
    // Minimum amount of audio to buffer before sending (in milliseconds)
    private const int MinimumBufferMs = 100;

    // Buffer to store audio data before sending
    private readonly List<byte> _audioBuffer;

    // Reference to the OpenAI real-time service client
    private readonly IOpenAIRealtimeService _client;

    // NAudio's wave input device for capturing audio
    private readonly WaveInEvent _waveIn;

    // Flag to track recording state
    private bool _isRecording;

    /// <summary>
    /// Initializes a new instance of VoiceInput with specified OpenAI client.
    /// </summary>
    /// <param name="client">The OpenAI real-time service client</param>
    public VoiceInput(IOpenAIRealtimeService client)
    {
        _client = client;
        // Configure audio input with specific format:
        // - 24000 Hz sample rate
        // - 16 bits per sample
        // - 1 channel (mono)
        _waveIn = new()
        {
            WaveFormat = new(24000, 16, 1),
            BufferMilliseconds = 50  // How often to receive audio data
        };
        _audioBuffer = [];
        _waveIn.DataAvailable += OnDataAvailable!;
    }

    /// <summary>
    /// Releases resources used by the voice input system
    /// </summary>
    public void Dispose()
    {
        _waveIn.Dispose();
    }

    /// <summary>
    /// Starts recording audio from the default input device
    /// </summary>
    public void StartRecording()
    {
        if (_isRecording) return;
        _isRecording = true;
        _audioBuffer.Clear();
        _waveIn.StartRecording();
    }

    /// <summary>
    /// Stops recording audio and sends any remaining buffered data
    /// </summary>
    public void StopRecording()
    {
        if (!_isRecording) return;
        _isRecording = false;
        _waveIn.StopRecording();

        // Send any remaining buffered audio before stopping
        if (_audioBuffer.Count > 0)
        {
            _client.ClientEvents.InputAudioBuffer.Append(_audioBuffer.ToArray());
            _audioBuffer.Clear();
        }
    }

    /// <summary>
    /// Handles incoming audio data from the recording device
    /// </summary>
    private void OnDataAvailable(object sender, WaveInEventArgs e)
    {
        if (!_isRecording) return;

        // Add new audio data to the buffer
        _audioBuffer.AddRange(e.Buffer.Take(e.BytesRecorded));

        // Calculate current buffer duration in milliseconds
        var bufferDurationMs = _audioBuffer.Count * 1000.0 / _waveIn.WaveFormat.AverageBytesPerSecond;

        // Only send when we have accumulated enough audio data
        if (bufferDurationMs >= MinimumBufferMs)
        {
            _client.ClientEvents.InputAudioBuffer.Append(_audioBuffer.ToArray());
            _audioBuffer.Clear();
        }
    }

    /// <summary>
    /// Sends an audio file to the OpenAI API by streaming it in chunks
    /// </summary>
    /// <param name="filePath">Path to the audio file to send</param>
    public async Task SendAudioFile(string filePath)
    {
        using var audioFileReader = new AudioFileReader(filePath);
        // Calculate buffer size based on minimum buffer duration
        var bufferSize = (int)(audioFileReader.WaveFormat.AverageBytesPerSecond * (MinimumBufferMs / 1000.0));
        var buffer = new byte[bufferSize];
        int bytesRead;

        // Read and send the file in chunks
        while ((bytesRead = await audioFileReader.ReadAsync(buffer, 0, buffer.Length)) > 0)
        {
            if (bytesRead < buffer.Length)
            {
                // Handle the last chunk if it's smaller than the buffer
                var lastBuffer = new byte[bytesRead];
                Array.Copy(buffer, lastBuffer, bytesRead);
                buffer = lastBuffer;
            }

            // Resample the audio to match required format and send
            var resampledBuffer = ResampleAudio(buffer, bytesRead, audioFileReader.WaveFormat, _waveIn.WaveFormat);
            await _client.ClientEvents.InputAudioBuffer.Append(resampledBuffer);
        }
    }

    /// <summary>
    /// Resamples audio data to match the target format required by the API
    /// </summary>
    /// <param name="buffer">Original audio data</param>
    /// <param name="bytesRead">Number of bytes in the buffer</param>
    /// <param name="sourceFormat">Original audio format</param>
    /// <param name="targetFormat">Desired output format</param>
    /// <returns>Resampled audio data</returns>
    private static byte[] ResampleAudio(byte[] buffer, int bytesRead, WaveFormat sourceFormat, WaveFormat targetFormat)
    {
        // Skip resampling if formats match
        if (sourceFormat.SampleRate == targetFormat.SampleRate &&
            sourceFormat.BitsPerSample == targetFormat.BitsPerSample &&
            sourceFormat.Channels == targetFormat.Channels)
        {
            var trimmedBuffer = new byte[bytesRead];
            Array.Copy(buffer, trimmedBuffer, bytesRead);
            return trimmedBuffer;
        }

        // Perform resampling using MediaFoundation
        using var sourceStream = new RawSourceWaveStream(buffer, 0, bytesRead, sourceFormat);
        using var resampler = new MediaFoundationResampler(sourceStream, targetFormat);
        resampler.ResamplerQuality = 60;  // Set high quality resampling

        // Calculate and allocate buffer for resampled audio
        var resampledBytes = (int)(bytesRead * ((double)targetFormat.AverageBytesPerSecond / sourceFormat.AverageBytesPerSecond));
        var resampledBuffer = new byte[resampledBytes];
        var resampledBytesRead = resampler.Read(resampledBuffer, 0, resampledBytes);

        // Trim the buffer to actual size and return
        var trimmedBuffer2 = new byte[resampledBytesRead];
        Array.Copy(resampledBuffer, trimmedBuffer2, resampledBytesRead);
        return trimmedBuffer2;
    }
}