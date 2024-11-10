using Betalgo.Ranul.OpenAI.ObjectModels.RealtimeModels;
using NAudio.Wave;

namespace OpenAI.Playground.TestHelpers.RealtimeHelpers;

public class VoiceOutput : IDisposable
{
    private readonly BufferedWaveProvider _bufferedWaveProvider;
    private readonly WaveOutEvent _waveOut;
    private bool _isPlaying;

    public VoiceOutput()
    {
        _waveOut = new();
        _waveOut.PlaybackStopped += OnPlaybackStopped!;
        _bufferedWaveProvider = new(new(RealtimeConstants.Audio.DefaultSampleRate, RealtimeConstants.Audio.DefaultBitsPerSample, RealtimeConstants.Audio.DefaultChannels))
        {
            BufferLength = 10 * 1024 * 1024, // 10 MB buffer - increase if needed
            DiscardOnBufferOverflow = true
        };
        _waveOut.Init(_bufferedWaveProvider);
    }

    public void Dispose()
    {
        _waveOut.Stop();
        _waveOut.Dispose();
    }

    public void EnqueueAudioData(byte[]? data)
    {
        if (data == null || data.Length == 0)
            return;

        _bufferedWaveProvider.AddSamples(data, 0, data.Length);

        if (!_isPlaying)
        {
            _waveOut.Play();
            _isPlaying = true;
        }
    }

    public void StopAndClear()
    {
        if (_isPlaying)
        {
            _waveOut.Stop();
            _isPlaying = false;
        }

        _bufferedWaveProvider.ClearBuffer();
        Console.WriteLine("Playback stopped and buffer cleared.");
    }

    private void OnPlaybackStopped(object sender, StoppedEventArgs e)
    {
        if (_bufferedWaveProvider.BufferedBytes > 0)
        {
            _waveOut.Play();
        }
        else
        {
            _isPlaying = false;
        }
    }
}