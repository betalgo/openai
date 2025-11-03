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

            foreach (var choice in response.Choices)
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

                if (response.Usage is { } usage)
                {
                    yield return new()
                    {
                        AuthorName = choice.Delta.Name,
                        Contents = [new UsageContent(GetUsageDetails(usage))],
                        CreatedAt = response.CreatedAt,
                        FinishReason = choice.FinishReason is not null ? new(choice.FinishReason) : null,
                        ModelId = response.Model,
                        ResponseId = response.Id,
                        MessageId = response.Id,
                        Role = choice.Delta.Role is not null ? new(choice.Delta.Role) : null
                    };
                }
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

    private ChatCompletionCreateRequest CreateRequest(IEnumerable<ChatMessage> chatMessages, ChatOptions? options)
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
            foreach (var content in message.Contents)
            {
                string? detail;
                switch (content)
                {
                    case TextContent tc:
                        request.Messages.Add(new()
                        {
                            Content = tc.Text,
                            Name = message.AuthorName,
                            Role = new ChatCompletionRole(message.Role.ToString())
                        });
                        break;

                    case UriContent uc:
                        request.Messages.Add(new()
                        {
                            Contents =
                            [
                                new()
                                {
                                    Type = "image_url",
                                    ImageUrl = new()
                                    {
                                        Url = uc.Uri.ToString(),
                                        Detail = uc.AdditionalProperties?.TryGetValue(nameof(MessageImageUrl.Detail), out detail) is true ? new ImageDetailType(detail):null
                                    }
                                }
                            ],
                            Name = message.AuthorName,
                            Role = new ChatCompletionRole(message.Role.ToString())
                        });
                        break;

                    case DataContent dc:
                        request.Messages.Add(new()
                        {
                            Contents =
                            [
                                new()
                                {
                                    Type = "image_url",
                                    ImageUrl = new()
                                    {
                                        Url = dc.Uri.ToString(),
                                        Detail = dc.AdditionalProperties?.TryGetValue(nameof(MessageImageUrl.Detail), out detail) is true ? new ImageDetailType(detail):null
                                    }
                                }
                            ],
                            Name = message.AuthorName,
                            Role = new ChatCompletionRole(message.Role.ToString())
                        });
                        break;

                    case FunctionResultContent frc:
                        request.Messages.Add(new()
                        {
                            ToolCallId = frc.CallId,
                            Content = frc.Result?.ToString(),
                            Name = message.AuthorName,
                            Role = new ChatCompletionRole(message.Role.ToString())
                        });
                        break;
                }
            }

            var functionCallContents = message.Contents.OfType<FunctionCallContent>().ToArray();
            if (functionCallContents.Length > 0)
            {
                request.Messages.Add(new()
                {
                    Name = message.AuthorName,
                    Role = new ChatCompletionRole(message.Role.ToString()),
                    ToolCalls = functionCallContents.Select(fcc => new ToolCall()
                        {
                            Type = ToolCallType.Function,
                            Id = fcc.CallId,
                            FunctionCall = new()
                            {
                                Name = fcc.Name,
                                Arguments = JsonSerializer.Serialize(fcc.Arguments)
                            }
                        })
                        .ToList()
                });
            }
        }

        return request;
    }

    private static PropertyDefinition CreateParameters(AIFunction f)
    {
        JsonElement openAISchema = s_schemaTransformCache.GetOrCreateTransformedSchema(f);
        return JsonSerializer.Deserialize<PropertyDefinition>(openAISchema) ?? new();
    }

    private static void PopulateContents(ObjectModels.RequestModels.ChatMessage source, IList<AIContent> destination)
    {
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