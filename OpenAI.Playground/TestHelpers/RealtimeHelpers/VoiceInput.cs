using Betalgo.Ranul.OpenAI.Managers;
using NAudio.Wave;

namespace OpenAI.Playground.TestHelpers.RealtimeHelpers;

public class VoiceInput : IDisposable
{
    private const int MinimumBufferMs = 100;
    private readonly List<byte> _audioBuffer;
    private readonly IOpenAIRealtimeService _client;
    private readonly WaveInEvent _waveIn;
    private bool _isRecording;

    public VoiceInput(IOpenAIRealtimeService client)
    {
        _client = client;
        _waveIn = new()
        {
            WaveFormat = new(24000, 16, 1),
            BufferMilliseconds = 50
        };
        _audioBuffer = [];
        _waveIn.DataAvailable += OnDataAvailable!;
    }

    public void Dispose()
    {
        _waveIn.Dispose();
    }

    public void StartRecording()
    {
        if (_isRecording) return;
        _isRecording = true;
        _audioBuffer.Clear();
        _waveIn.StartRecording();
    }

    public void StopRecording()
    {
        if (!_isRecording) return;
        _isRecording = false;
        _waveIn.StopRecording();

        // Send any remaining buffered audio
        if (_audioBuffer.Count > 0)
        {
            _client.ClientEvents.InputAudioBuffer.Append(_audioBuffer.ToArray());
            _audioBuffer.Clear();
        }
    }

    private void OnDataAvailable(object sender, WaveInEventArgs e)
    {
        if (!_isRecording) return;

        // Add new audio data to the buffer
        _audioBuffer.AddRange(e.Buffer.Take(e.BytesRecorded));

        // Calculate buffer duration in milliseconds
        var bufferDurationMs = _audioBuffer.Count * 1000.0 / _waveIn.WaveFormat.AverageBytesPerSecond;

        // Only send when we have at least MinimumBufferMs of audio
        if (bufferDurationMs >= MinimumBufferMs)
        {
            _client.ClientEvents.InputAudioBuffer.Append(_audioBuffer.ToArray());
            _audioBuffer.Clear();
        }
    }

    public async Task SendAudioFile(string filePath)
    {
        using var audioFileReader = new AudioFileReader(filePath);
        var bufferSize = (int)(audioFileReader.WaveFormat.AverageBytesPerSecond * (MinimumBufferMs / 1000.0));
        var buffer = new byte[bufferSize];
        int bytesRead;

        while ((bytesRead = await audioFileReader.ReadAsync(buffer, 0, buffer.Length)) > 0)
        {
            if (bytesRead < buffer.Length)
            {
                // Handle the last partial buffer
                var lastBuffer = new byte[bytesRead];
                Array.Copy(buffer, lastBuffer, bytesRead);
                buffer = lastBuffer;
            }

            var resampledBuffer = ResampleAudio(buffer, bytesRead, audioFileReader.WaveFormat, _waveIn.WaveFormat);
            await _client.ClientEvents.InputAudioBuffer.Append(resampledBuffer);
        }
    }

    private static byte[] ResampleAudio(byte[] buffer, int bytesRead, WaveFormat sourceFormat, WaveFormat targetFormat)
    {
        if (sourceFormat.SampleRate == targetFormat.SampleRate && sourceFormat.BitsPerSample == targetFormat.BitsPerSample && sourceFormat.Channels == targetFormat.Channels)
        {
            // No resampling needed
            var trimmedBuffer = new byte[bytesRead];
            Array.Copy(buffer, trimmedBuffer, bytesRead);
            return trimmedBuffer;
        }

        using var sourceStream = new RawSourceWaveStream(buffer, 0, bytesRead, sourceFormat);
        using var resampler = new MediaFoundationResampler(sourceStream, targetFormat);
        resampler.ResamplerQuality = 60;

        var resampledBytes = (int)(bytesRead * ((double)targetFormat.AverageBytesPerSecond / sourceFormat.AverageBytesPerSecond));
        var resampledBuffer = new byte[resampledBytes];
        var resampledBytesRead = resampler.Read(resampledBuffer, 0, resampledBytes);

        var trimmedBuffer2 = new byte[resampledBytesRead];
        Array.Copy(resampledBuffer, trimmedBuffer2, resampledBytesRead);
        return trimmedBuffer2;
    }
}