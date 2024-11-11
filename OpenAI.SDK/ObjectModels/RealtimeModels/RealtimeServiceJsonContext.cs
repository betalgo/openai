#if NET6_0_OR_GREATER

using System.Text.Json;
using System.Text.Json.Serialization;
using Betalgo.Ranul.OpenAI.ObjectModels.RequestModels;

namespace Betalgo.Ranul.OpenAI.ObjectModels.RealtimeModels;

[JsonSerializable(typeof(ErrorEvent))]
[JsonSerializable(typeof(SessionEvent))]
[JsonSerializable(typeof(ConversationCreatedEvent))]
[JsonSerializable(typeof(ConversationItemCreatedEvent))]
[JsonSerializable(typeof(InputAudioTranscriptionCompletedEvent))]
[JsonSerializable(typeof(InputAudioTranscriptionFailedEvent))]
[JsonSerializable(typeof(ConversationItemTruncatedEvent))]
[JsonSerializable(typeof(ConversationItemDeletedEvent))]
[JsonSerializable(typeof(AudioBufferCommittedEvent))]
[JsonSerializable(typeof(AudioBufferClearedEvent))]
[JsonSerializable(typeof(AudioBufferSpeechStartedEvent))]
[JsonSerializable(typeof(AudioBufferSpeechStoppedEvent))]
[JsonSerializable(typeof(ResponseEvent))]
[JsonSerializable(typeof(ResponseOutputItemAddedEvent))]
[JsonSerializable(typeof(ResponseOutputItemDoneEvent))]
[JsonSerializable(typeof(ResponseContentPartEvent))]
[JsonSerializable(typeof(TextStreamEvent))]
[JsonSerializable(typeof(AudioTranscriptStreamEvent))]
[JsonSerializable(typeof(AudioStreamEvent))]
[JsonSerializable(typeof(FunctionCallStreamEvent))]
[JsonSerializable(typeof(RateLimitsEvent))]
[JsonSerializable(typeof(SessionUpdateRequest))]

// Additional Request Events
[JsonSerializable(typeof(AudioBufferAppendRequest))]
[JsonSerializable(typeof(AudioBufferCommitRequest))]
[JsonSerializable(typeof(AudioBufferClearRequest))]
[JsonSerializable(typeof(ConversationItemCreateRequest))]
[JsonSerializable(typeof(ConversationItemTruncateRequest))]
[JsonSerializable(typeof(ConversationItemDeleteRequest))]
[JsonSerializable(typeof(ResponseCreateRequest))]
[JsonSerializable(typeof(ResponseCancelRequest))]
[JsonSerializable(typeof(ResponseConfig))]

// Model Classes
[JsonSerializable(typeof(SessionConfig))]
[JsonSerializable(typeof(AudioTranscriptionConfig))]
[JsonSerializable(typeof(TurnDetectionConfig))]
[JsonSerializable(typeof(JsonSchema))]
[JsonSerializable(typeof(JsonSchemaProperty))]
[JsonSerializable(typeof(ConversationItem))]
[JsonSerializable(typeof(ContentPart))]
[JsonSerializable(typeof(SessionResource))]
[JsonSerializable(typeof(ConversationResource))]
[JsonSerializable(typeof(ResponseResource))]
[JsonSerializable(typeof(RateLimit))]
[JsonSerializable(typeof(MaxOutputTokens))]

// Enum Types
[JsonSerializable(typeof(AudioFormat))]
[JsonSerializable(typeof(Status))]
[JsonSerializable(typeof(ContentType))]
[JsonSerializable(typeof(ItemType))]
[JsonSerializable(typeof(Role))]

// Collections and Dictionary Types
[JsonSerializable(typeof(List<string>))]
[JsonSerializable(typeof(List<ContentPart>))]
[JsonSerializable(typeof(List<FunctionDefinition>))]
[JsonSerializable(typeof(List<ConversationItem>))]
[JsonSerializable(typeof(List<RateLimit>))]
[JsonSerializable(typeof(Dictionary<string, JsonSchemaProperty>))]
[JsonSerializable(typeof(Dictionary<string, string>))]
[JsonSourceGenerationOptions(WriteIndented = false, DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull, AllowTrailingCommas = false, PropertyNameCaseInsensitive = false, ReadCommentHandling = JsonCommentHandling.Skip)]
internal partial class RealtimeServiceJsonContext : JsonSerializerContext
{
}
#endif