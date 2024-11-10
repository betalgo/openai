namespace Betalgo.Ranul.OpenAI.ObjectModels.RealtimeModels;

/// <summary>
///     Contains constants used by the Realtime API implementation.
/// </summary>
public static class RealtimeConstants
{
    /// <summary>
    ///     WebSocket subprotocols used for connection.
    /// </summary>
    public static class SubProtocols
    {
        public const string Realtime = "realtime";
        public const string Beta = "openai-beta.realtime-v1";
    }

    /// <summary>
    ///     Headers used in API requests.
    /// </summary>
    public static class Headers
    {
        public const string Authorization = "Authorization";
        public const string OpenAIBeta = "OpenAI-Beta";
        public const string OpenAIOrganization = "OpenAI-Organization";
    }

    /// <summary>
    ///     Audio format settings.
    /// </summary>
    public static class Audio
    {
        public const string FormatPcm16 = "pcm16";
        public const string FormatG711Ulaw = "g711_ulaw";
        public const string FormatG711Alaw = "g711_alaw";
        public const int DefaultSampleRate = 24000;
        public const int DefaultBitsPerSample = 16;
        public const int DefaultChannels = 1;
    }

    /// <summary>
    ///     Turn detection settings.
    /// </summary>
    public static class TurnDetection
    {
        public const string TypeServerVad = "server_vad";
        public const double DefaultThreshold = 0.5;
        public const int DefaultPrefixPaddingMs = 300;
        public const int DefaultSilenceDurationMs = 200;
    }
}