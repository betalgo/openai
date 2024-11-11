namespace Betalgo.Ranul.OpenAI.ObjectModels.RealtimeModels;

/// <summary>
/// Contains constants used by the OpenAI Realtime API implementation for real-time communication 
/// with GPT-4 class models over WebSocket, supporting both audio and text transcriptions.
/// </summary>
public static class RealtimeConstants
{
    /// <summary>
    /// WebSocket subprotocols used for establishing connection with the OpenAI Realtime WebSocket server.
    /// </summary>
    public static class SubProtocols
    {
        /// <summary>
        /// The primary realtime subprotocol identifier.
        /// </summary>
        public const string Realtime = "realtime";

        /// <summary>
        /// The beta version subprotocol identifier for OpenAI realtime API.
        /// </summary>
        public const string Beta = "openai-beta.realtime-v1";
    }

    /// <summary>
    /// Standard headers required for authentication and configuration in API requests.
    /// </summary>
    public static class Headers
    {
        /// <summary>
        /// The authorization header for API authentication.
        /// </summary>
        public const string Authorization = "Authorization";

        /// <summary>
        /// Header indicating beta feature usage with OpenAI API.
        /// </summary>
        public const string OpenAIBeta = "OpenAI-Beta";

        /// <summary>
        /// Header for specifying the OpenAI organization identifier.
        /// </summary>
        public const string OpenAIOrganization = "OpenAI-Organization";
    }

    /// <summary>
    /// Audio format configurations supported by the Realtime API for input and output audio processing.
    /// </summary>
    public static class Audio
    {
        /// <summary>
        /// PCM 16-bit audio format identifier.
        /// Used for both input_audio_format and output_audio_format in session configuration.
        /// </summary>
        public const string FormatPcm16 = "pcm16";

        /// <summary>
        /// G.711 μ-law audio format identifier.
        /// Used for both input_audio_format and output_audio_format in session configuration.
        /// </summary>
        public const string FormatG711Ulaw = "g711_ulaw";

        /// <summary>
        /// G.711 A-law audio format identifier.
        /// Used for both input_audio_format and output_audio_format in session configuration.
        /// </summary>
        public const string FormatG711Alaw = "g711_alaw";

        /// <summary>
        /// Default sample rate for audio processing, specified in Hz.
        /// </summary>
        public const int DefaultSampleRate = 24000;

        /// <summary>
        /// Default number of bits per sample for audio processing.
        /// </summary>
        public const int DefaultBitsPerSample = 16;

        /// <summary>
        /// Default number of audio channels.
        /// </summary>
        public const int DefaultChannels = 1;
    }

    /// <summary>
    /// Configuration settings for server-side Voice Activity Detection (VAD) turn detection.
    /// These settings control how the server detects speech segments in audio input.
    /// </summary>
    public static class TurnDetection
    {
        /// <summary>
        /// Identifier for server-side Voice Activity Detection.
        /// The only currently supported type for turn detection as specified in the API documentation.
        /// </summary>
        public const string TypeServerVad = "server_vad";

        /// <summary>
        /// Default activation threshold for Voice Activity Detection (VAD).
        /// Represents the sensitivity of speech detection, ranging from 0.0 to 1.0.
        /// </summary>
        public const double DefaultThreshold = 0.5;

        /// <summary>
        /// Default amount of audio to include before speech starts, in milliseconds.
        /// This padding ensures the beginning of speech is not cut off.
        /// </summary>
        public const int DefaultPrefixPaddingMs = 300;

        /// <summary>
        /// Default duration of silence required to detect the end of speech, in milliseconds.
        /// Used to determine when a speech segment has completed.
        /// </summary>
        public const int DefaultSilenceDurationMs = 200;
    }
}