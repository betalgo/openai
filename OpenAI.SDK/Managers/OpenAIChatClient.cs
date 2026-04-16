using System.Runtime.CompilerServices;
using System.Text.Json;
using Betalgo.Ranul.OpenAI.Contracts.Enums;
using Betalgo.Ranul.OpenAI.Contracts.Enums.Image;
using Betalgo.Ranul.OpenAI.ObjectModels;
using Betalgo.Ranul.OpenAI.ObjectModels.RequestModels;
using Betalgo.Ranul.OpenAI.ObjectModels.ResponseModels;
using Betalgo.Ranul.OpenAI.ObjectModels.SharedModels;
using Microsoft.Extensions.AI;
using ChatMessage = Microsoft.Extensions.AI.ChatMessage;

namespace Betalgo.Ranul.OpenAI.Managers;

public partial class OpenAIService : IChatClient
{
    private static readonly AIJsonSchemaTransformCache s_schemaTransformCache = new(new()
    {
        // https://platform.openai.com/docs/guides/structured-outputs?api-mode=responses#supported-schemas
        DisallowAdditionalProperties = true,
        RequireAllProperties = true,
        MoveDefaultKeywordToDescription = true,
    });

    private ChatClientMetadata? _chatMetadata;

    /// <inheritdoc />
    object? IChatClient.GetService(Type serviceType, object? serviceKey) =>
        serviceKey is not null ? null :
        serviceType == typeof(ChatClientMetadata) ? (_chatMetadata ??= new(nameof(OpenAIService), _httpClient.BaseAddress, _defaultModelId)) :
        serviceType?.IsInstanceOfType(this) is true ? this : 
        null;

    /// <inheritdoc />
    void IDisposable.Dispose()
    {
    }

    /// <inheritdoc />
    async Task<ChatResponse> IChatClient.GetResponseAsync(IEnumerable<ChatMessage> messages, ChatOptions? options, CancellationToken cancellationToken)
    {
        var request = CreateRequest(messages, options);

        var response = await ChatCompletion.CreateCompletion(request, options?.ModelId, cancellationToken);
        ThrowIfNotSuccessful(response);

        string? finishReason = null;
        List<ChatMessage> responseMessages = [];
        foreach (var choice in response.Choices)
        {
            finishReason ??= choice.FinishReason;

            ChatMessage m = new()
            {
                Role = new(choice.Message.Role),
                AuthorName = choice.Message.Name,
                RawRepresentation = choice,
                MessageId = response.Id
            };

            PopulateContents(choice.Message, m.Contents);

            if (response.ServiceTier is string serviceTier)
            {
                (m.AdditionalProperties ??= [])[nameof(response.ServiceTier)] = serviceTier;
            }

            if (response.SystemFingerPrint is string fingerprint)
            {
                (m.AdditionalProperties ??= [])[nameof(response.SystemFingerPrint)] = fingerprint;
            }

            responseMessages.Add(m);
        }

        return new(responseMessages)
        {
            CreatedAt = response.CreatedAt,
            FinishReason = finishReason is not null ? new(finishReason) : null,
            ModelId = response.Model,
            RawRepresentation = response,
            ResponseId = response.Id,
            Usage = response.Usage is { } usage ? GetUsageDetails(usage) : null
        };
    }

    /// <inheritdoc />
    async IAsyncEnumerable<ChatResponseUpdate> IChatClient.GetStreamingResponseAsync(IEnumerable<ChatMessage> messages, ChatOptions? options, [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        var request = CreateRequest(messages, options);

        await foreach (var response in ChatCompletion.CreateCompletionAsStream(request, options?.ModelId, cancellationToken: cancellationToken))
        {
            ThrowIfNotSuccessful(response);

            var choices = response.Choices ?? [];

            foreach (var choice in choices)
            {
                ChatResponseUpdate update = new()
                {
                    AuthorName = choice.Delta.Name,
                    CreatedAt = response.CreatedAt,
                    FinishReason = choice.FinishReason is not null ? new(choice.FinishReason) : null,
                    ModelId = response.Model,
                    RawRepresentation = response,
                    ResponseId = response.Id,
                    MessageId = response.Id,
                    Role = choice.Delta.Role is not null ? new(choice.Delta.Role) : null
                };

                if (response.ServiceTier is string serviceTier)
                {
                    (update.AdditionalProperties ??= [])[nameof(response.ServiceTier)] = serviceTier;
                }

                if (response.SystemFingerPrint is string fingerprint)
                {
                    (update.AdditionalProperties ??= [])[nameof(response.SystemFingerPrint)] = fingerprint;
                }

                PopulateContents(choice.Delta, update.Contents);

                yield return update;
            }

            if (response.Usage is { } usage)
            {
                var usageChoice = choices.FirstOrDefault();
                var usageAuthorName = usageChoice?.Delta?.Name;
                var usageRole = usageChoice?.Delta?.Role;
                var usageFinishReason = usageChoice?.FinishReason;

                yield return new()
                {
                    AuthorName = usageAuthorName,
                    Contents = [new UsageContent(GetUsageDetails(usage))],
                    CreatedAt = response.CreatedAt,
                    FinishReason = usageFinishReason is not null ? new(usageFinishReason) : null,
                    ModelId = response.Model,
                    ResponseId = response.Id,
                    MessageId = response.Id,
                    Role = usageRole is not null ? new(usageRole) : null
                };
            }
        }
    }

    private static void ThrowIfNotSuccessful(ChatCompletionCreateResponse response)
    {
        if (!response.Successful)
        {
            throw new InvalidOperationException(response.Error is { } error ? $"{response.Error.Code}: {response.Error.Message}" : "Betalgo.Ranul Unknown error");
        }
    }

    internal ChatCompletionCreateRequest CreateRequest(IEnumerable<ChatMessage> chatMessages, ChatOptions? options)
    {
        ChatCompletionCreateRequest request = new()
        {
            Model = options?.ModelId ?? _defaultModelId
        };

        if (options is not null)
        {
            // Strongly-typed properties from options
            request.MaxCompletionTokens = options.MaxOutputTokens;
            request.Temperature = options.Temperature;
            request.TopP = options.TopP;
            request.FrequencyPenalty = options.FrequencyPenalty;
            request.PresencePenalty = options.PresencePenalty;
            request.Seed = (int?)options.Seed;
            request.StopAsList = options.StopSequences;
            request.ParallelToolCalls = options.AllowMultipleToolCalls;

            // Non-strongly-typed properties from additional properties
            request.LogitBias = options.AdditionalProperties?.TryGetValue(nameof(request.LogitBias), out var logitBias) is true ? logitBias : null;
            request.LogProbs = options.AdditionalProperties?.TryGetValue(nameof(request.LogProbs), out bool logProbs) is true ? logProbs : null;
            request.N = options.AdditionalProperties?.TryGetValue(nameof(request.N), out int n) is true ? n : null;
            request.ServiceTier = options.AdditionalProperties?.TryGetValue(nameof(request.ServiceTier), out string? serviceTier) is true ? serviceTier : null!;
            request.User = options.AdditionalProperties?.TryGetValue(nameof(request.User), out string? user) is true ? user : null!;
            request.TopLogprobs = options.AdditionalProperties?.TryGetValue(nameof(request.TopLogprobs), out int topLogprobs) is true ? topLogprobs : null;

            // Response format
            switch (options.ResponseFormat)
            {
                case ChatResponseFormatText:
                    request.ResponseFormat = new() { Type = Contracts.Enums.ResponseFormat.Text };
                    break;

                case ChatResponseFormatJson { Schema: not null } json:
                    request.ResponseFormat = new()
                    {
                        Type = Contracts.Enums.ResponseFormat.JsonSchema,
                        JsonSchema = new()
                        {
                            Name = json.SchemaName ?? "JsonSchema",
                            Schema = JsonSerializer.Deserialize<PropertyDefinition>(json.Schema.Value),
                            Description = json.SchemaDescription
                        }
                    };
                    break;

                case ChatResponseFormatJson:
                    request.ResponseFormat = new() { Type = Contracts.Enums.ResponseFormat.JsonObject };
                    break;
            }

            // Tools
            request.Tools = options.Tools
                ?.OfType<AIFunction>()
                .Select(f =>
                {
                    return ToolDefinition.DefineFunction(new()
                    {
                        Name = f.Name,
                        Description = f.Description,
                        Parameters = CreateParameters(f)
                    });
                })
                .ToList() is { Count: > 0 } tools
                ? tools
                : null;
            if (request.Tools is not null)
            {
                request.ToolChoice = options.ToolMode is RequiredChatToolMode r ? new()
                    {
                        Type = ToolChoiceType.Required,
                        Function = r.RequiredFunctionName is null ? null : new ToolChoice.FunctionTool() { Name = r.RequiredFunctionName }
                    } :
                    options.ToolMode is AutoChatToolMode or null ? new() { Type = ToolChoiceType.Auto } :
                    new ToolChoice() { Type = ToolChoiceType.None };
            }
        }

        // Messages
        request.Messages = [];
        foreach (var message in chatMessages)
        {
            foreach (var requestMessage in ConvertMessageByRole(message))
            {
                request.Messages.Add(requestMessage);
            }
        }

        return request;

        static IList<MessageContent> EnsureContents(ObjectModels.RequestModels.ChatMessage target)
        {
            target.Contents ??= [];

            if (target.Content is string existingText)
            {
                target.Contents.Add(MessageContent.TextContent(existingText));
                target.Content = null;
            }

            return target.Contents;
        }

        static IEnumerable<ObjectModels.RequestModels.ChatMessage> ConvertMessageByRole(ChatMessage source)
        {
            var role = new ChatCompletionRole(source.Role.ToString());

            if (role == ChatCompletionRole.Assistant)
            {
                return ConvertAssistantMessage(source);
            }

            if (role == ChatCompletionRole.Tool)
            {
                return ConvertToolMessage(source);
            }

            return ConvertBasicMessage(source, role);
        }

        static IEnumerable<ObjectModels.RequestModels.ChatMessage> ConvertAssistantMessage(ChatMessage source)
        {
            ObjectModels.RequestModels.ChatMessage? assistantMessage = null;

            foreach (var content in source.Contents)
            {
                switch (content)
                {
                    case TextContent tc:
                        assistantMessage ??= CreateMessage(source, ChatCompletionRole.Assistant);
                        AddText(assistantMessage, tc.Text);
                        break;

                    case TextReasoningContent rc:
                        assistantMessage ??= CreateMessage(source, ChatCompletionRole.Assistant);
                        assistantMessage.ReasoningContent = assistantMessage.ReasoningContent is null
                            ? rc.Text
                            : $"{assistantMessage.ReasoningContent}\n{rc.Text}";
                        break;

                    case UriContent uc:
                        assistantMessage ??= CreateMessage(source, ChatCompletionRole.Assistant);
                        AddImageFromUri(assistantMessage, uc.Uri, uc.AdditionalProperties);
                        break;

                    case DataContent dc:
                        assistantMessage ??= CreateMessage(source, ChatCompletionRole.Assistant);
                        AddImageFromString(assistantMessage, dc.Uri, dc.AdditionalProperties);
                        break;

                    case FunctionCallContent fcc:
                        assistantMessage ??= CreateMessage(source, ChatCompletionRole.Assistant);
                        (assistantMessage.ToolCalls ??= []).Add(new ToolCall()
                        {
                            Type = ToolCallType.Function,
                            Id = fcc.CallId,
                            FunctionCall = new()
                            {
                                Name = fcc.Name,
                                Arguments = SerializeFunctionArguments(fcc.Arguments)
                            }
                        });
                        break;

                    case FunctionResultContent:
                        // Ignore unsupported content in assistant messages.
                        break;

                    default:
                        // Ignore unsupported content in assistant messages.
                        break;
                }
            }

            if (assistantMessage is not null)
            {
                yield return assistantMessage;
            }
        }

        static IEnumerable<ObjectModels.RequestModels.ChatMessage> ConvertToolMessage(ChatMessage source)
        {
            foreach (var content in source.Contents)
            {
                if (content is FunctionResultContent frc)
                {
                    if (string.IsNullOrWhiteSpace(frc.CallId))
                    {
                        // Ignore invalid tool result entries missing call id.
                        continue;
                    }

                    yield return new ObjectModels.RequestModels.ChatMessage
                    {
                        ToolCallId = frc.CallId,
                        Content = SerializeFunctionResult(frc.Result),
                        Name = source.AuthorName,
                        Role = ChatCompletionRole.Tool
                    };
                }
            }
        }

        static IEnumerable<ObjectModels.RequestModels.ChatMessage> ConvertBasicMessage(ChatMessage source, ChatCompletionRole role)
        {
            ObjectModels.RequestModels.ChatMessage? target = null;

            foreach (var content in source.Contents)
            {
                switch (content)
                {
                    case TextContent tc:
                        target ??= CreateMessage(source, role);
                        AddText(target, tc.Text);
                        break;

                    case TextReasoningContent rc:
                        target ??= CreateMessage(source, role);
                        target.ReasoningContent = target.ReasoningContent is null
                            ? rc.Text
                            : $"{target.ReasoningContent}\n{rc.Text}";
                        break;

                    case UriContent uc:
                        target ??= CreateMessage(source, role);
                        AddImageFromUri(target, uc.Uri, uc.AdditionalProperties);
                        break;

                    case DataContent dc:
                        target ??= CreateMessage(source, role);
                        AddImageFromString(target, dc.Uri, dc.AdditionalProperties);
                        break;

                    case FunctionCallContent:
                    case FunctionResultContent:
                        // Ignore unsupported content for non-assistant/non-tool roles.
                        break;

                    default:
                        // Ignore unknown content types.
                        break;
                }
            }

            if (target is not null)
            {
                yield return target;
            }
        }

        static ObjectModels.RequestModels.ChatMessage CreateMessage(ChatMessage source, ChatCompletionRole role)
        {
            return new()
            {
                Name = source.AuthorName,
                Role = role
            };
        }

        static void AddText(ObjectModels.RequestModels.ChatMessage target, string text)
        {
            if (target.Contents is { Count: > 0 })
            {
                target.Contents.Add(MessageContent.TextContent(text));
                return;
            }

            if (target.Content is null)
            {
                target.Content = text;
                return;
            }

            target.Contents =
            [
                MessageContent.TextContent(target.Content),
                MessageContent.TextContent(text)
            ];
            target.Content = null;
        }

        static void AddImageFromUri(ObjectModels.RequestModels.ChatMessage target, Uri uri, IDictionary<string, object?>? additionalProperties)
        {
            AddImageFromString(target, uri.ToString(), additionalProperties);
        }

        static void AddImageFromString(ObjectModels.RequestModels.ChatMessage target, string uri, IDictionary<string, object?>? additionalProperties)
        {
            string? detail = additionalProperties?.TryGetValue(nameof(MessageImageUrl.Detail), out var detailObject) is true
                ? detailObject as string
                : null;

            EnsureContents(target).Add(new()
            {
                Type = "image_url",
                ImageUrl = new()
                {
                    Url = uri,
                    Detail = detail is not null
                        ? new ImageDetailType(detail)
                        : null
                }
            });
        }

        static string SerializeFunctionArguments(object? arguments)
        {
            if (arguments is null)
            {
                return "{}";
            }

            if (arguments is JsonElement element)
            {
                return element.GetRawText();
            }

            if (arguments is string rawJson)
            {
                return rawJson;
            }

            return JsonSerializer.Serialize(arguments);
        }

        static string? SerializeFunctionResult(object? result)
        {
            return result switch
            {
                null => null,
                string text => text,
                JsonElement jsonElement => jsonElement.GetRawText(),
                _ => JsonSerializer.Serialize(result)
            };
        }
    }

    private static PropertyDefinition CreateParameters(AIFunction f)
    {
        JsonElement openAISchema = s_schemaTransformCache.GetOrCreateTransformedSchema(f);
        return JsonSerializer.Deserialize<PropertyDefinition>(openAISchema) ?? new();
    }

    private static void PopulateContents(ObjectModels.RequestModels.ChatMessage source, IList<AIContent> destination)
    {
        if (!string.IsNullOrWhiteSpace(source.ReasoningContent))
        {
            destination.Add(new TextReasoningContent(source.ReasoningContent));
        }

        if (source.Content is not null)
        {
            destination.Add(new TextContent(source.Content));
        }

        if (source.Contents is { } contents)
        {
            foreach (var content in contents)
            {
                if (content.Text is string text)
                {
                    destination.Add(new TextContent(text));
                }

                if (content.ImageUrl is { } url)
                {
                    destination.Add(new UriContent(url.Url, "image/*"));
                }
            }
        }

        if (source.ToolCalls is { } toolCalls)
        {
            foreach (var tc in toolCalls)
            {
                destination.Add(new FunctionCallContent(tc.Id ?? string.Empty, tc.FunctionCall?.Name ?? string.Empty, tc.FunctionCall?.Arguments is string a ? JsonSerializer.Deserialize<Dictionary<string, object?>>(a) : null));
            }
        }
    }

    private static UsageDetails GetUsageDetails(UsageResponse usage)
    {
        var details = new UsageDetails()
        {
            InputTokenCount = usage.PromptTokens,
            OutputTokenCount = usage.CompletionTokens,
            TotalTokenCount = usage.TotalTokens
        };

        if (usage.PromptTokensDetails is { } promptDetails)
        {
            if (promptDetails.CachedTokens is int cachedTokens)
            {
                (details.AdditionalCounts ??= [])[$"{nameof(usage.PromptTokensDetails)}.{nameof(promptDetails.CachedTokens)}"] = cachedTokens;
            }

            if (promptDetails.AudioTokens is int audioTokens)
            {
                (details.AdditionalCounts ??= [])[$"{nameof(usage.PromptTokensDetails)}.{nameof(promptDetails.AudioTokens)}"] = audioTokens;
            }
        }

        if (usage.CompletionTokensDetails is { } completionDetails)
        {
            if (completionDetails.ReasoningTokens is int reasoningTokens)
            {
                (details.AdditionalCounts ??= [])[$"{nameof(usage.CompletionTokensDetails)}.{nameof(completionDetails.ReasoningTokens)}"] = reasoningTokens;
            }

            if (completionDetails.AudioTokens is int audioTokens)
            {
                (details.AdditionalCounts ??= [])[$"{nameof(usage.CompletionTokensDetails)}.{nameof(completionDetails.AudioTokens)}"] = audioTokens;
            }
        }

        return details;
    }
}