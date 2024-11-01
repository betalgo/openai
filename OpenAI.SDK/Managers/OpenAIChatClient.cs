using Betalgo.Ranul.OpenAI.ObjectModels;
using Betalgo.Ranul.OpenAI.ObjectModels.RequestModels;
using Betalgo.Ranul.OpenAI.ObjectModels.ResponseModels;
using Betalgo.Ranul.OpenAI.ObjectModels.SharedModels;
using Microsoft.Extensions.AI;
using System.Text.Json;

namespace Betalgo.Ranul.OpenAI.Managers;

public partial class OpenAIService : IChatClient
{
    private ChatClientMetadata? _chatMetadata;

    /// <inheritdoc/>
    ChatClientMetadata IChatClient.Metadata => _chatMetadata ??= new(nameof(OpenAIService), _httpClient.BaseAddress, this._defaultModelId);

    /// <inheritdoc/>
    TService? IChatClient.GetService<TService>(object? key) where TService : class =>
        this as TService;

    /// <inheritdoc/>
    void IDisposable.Dispose() { }

    /// <inheritdoc/>
    async Task<ChatCompletion> IChatClient.CompleteAsync(
        IList<Microsoft.Extensions.AI.ChatMessage> chatMessages, ChatOptions? options, CancellationToken cancellationToken)
    {
        ChatCompletionCreateRequest request = CreateRequest(chatMessages, options);

        var response = await this.ChatCompletion.CreateCompletion(request, options?.ModelId, cancellationToken);
        ThrowIfNotSuccessful(response);

        string? finishReason = null;
        List<Microsoft.Extensions.AI.ChatMessage> responseMessages = [];
        foreach (ChatChoiceResponse choice in response.Choices)
        {
            finishReason ??= choice.FinishReason;

            Microsoft.Extensions.AI.ChatMessage m = new()
            {
                Role = new(choice.Message.Role),
                AuthorName = choice.Message.Name,
                RawRepresentation = choice
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
            CreatedAt = DateTimeOffset.FromUnixTimeSeconds(response.CreatedAt),
            CompletionId = response.Id,
            FinishReason = finishReason is not null ? new(finishReason) : null,
            ModelId = response.Model,
            RawRepresentation = response,
            Usage = response.Usage is { } usage ? GetUsageDetails(usage) : null,
        };
    }

    /// <inheritdoc/>
    async IAsyncEnumerable<StreamingChatCompletionUpdate> IChatClient.CompleteStreamingAsync(
        IList<Microsoft.Extensions.AI.ChatMessage> chatMessages, ChatOptions? options, CancellationToken cancellationToken)
    {
        ChatCompletionCreateRequest request = CreateRequest(chatMessages, options);

        await foreach (var response in this.ChatCompletion.CreateCompletionAsStream(request, options?.ModelId, cancellationToken: cancellationToken))
        {
            ThrowIfNotSuccessful(response);

            foreach (ChatChoiceResponse choice in response.Choices)
            {
                StreamingChatCompletionUpdate update = new()
                {
                    AuthorName = choice.Delta.Name,
                    CompletionId = response.Id,
                    CreatedAt = DateTimeOffset.FromUnixTimeSeconds(response.CreatedAt),
                    FinishReason = choice.FinishReason is not null ? new(choice.FinishReason) : null,
                    ModelId = response.Model,
                    RawRepresentation = response,
                    Role = choice.Delta.Role is not null ? new(choice.Delta.Role) : null,
                };

                if (choice.Index is not null)
                {
                    update.ChoiceIndex = choice.Index.Value;
                }

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
                        CompletionId = response.Id,
                        Contents = [new UsageContent(GetUsageDetails(usage))],
                        CreatedAt = DateTimeOffset.FromUnixTimeSeconds(response.CreatedAt),
                        FinishReason = choice.FinishReason is not null ? new(choice.FinishReason) : null,
                        ModelId = response.Model,
                        Role = choice.Delta.Role is not null ? new(choice.Delta.Role) : null,
                    };
                }
            }
        }
    }

    private static void ThrowIfNotSuccessful(ChatCompletionCreateResponse response)
    {
        if (!response.Successful)
        {
            throw new InvalidOperationException(response.Error is { } error ?
                $"{response.Error.Code}: {response.Error.Message}" :
                "Unknown error");
        }
    }

    private ChatCompletionCreateRequest CreateRequest(
        IList<Microsoft.Extensions.AI.ChatMessage> chatMessages, ChatOptions? options)
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
            request.StopAsList = options.StopSequences;

            // Non-strongly-typed properties from additional properties
            request.LogitBias = options.AdditionalProperties?.TryGetValue(nameof(request.LogitBias), out object? logitBias) is true ? logitBias : null;
            request.LogProbs = options.AdditionalProperties?.TryGetValue(nameof(request.LogProbs), out bool logProbs) is true ? logProbs : null;
            request.N = options.AdditionalProperties?.TryGetValue(nameof(request.N), out int n) is true ? n : null;
            request.ParallelToolCalls = options.AdditionalProperties?.TryGetValue(nameof(request.ParallelToolCalls), out bool parallelToolCalls) is true ? parallelToolCalls : null;
            request.Seed = options.AdditionalProperties?.TryGetValue(nameof(request.Seed), out int seed) is true ? seed : null;
            request.ServiceTier = options.AdditionalProperties?.TryGetValue(nameof(request.ServiceTier), out string? serviceTier) is true ? serviceTier : null!;
            request.User = options.AdditionalProperties?.TryGetValue(nameof(request.User), out string? user) is true ? user : null!;
            request.TopLogprobs = options.AdditionalProperties?.TryGetValue(nameof(request.TopLogprobs), out int topLogprobs) is true ? topLogprobs : null;

            // Response format
            switch (options.ResponseFormat)
            {
                case ChatResponseFormatText:
                    request.ResponseFormat = new() { Type = StaticValues.CompletionStatics.ResponseFormat.Text };
                    break;

                case ChatResponseFormatJson json when json.Schema is not null:
                    request.ResponseFormat = new()
                    {
                        Type = StaticValues.CompletionStatics.ResponseFormat.JsonSchema,
                        JsonSchema = new JsonSchema()
                        {
                            Name = json.SchemaName ?? "JsonSchema",
                            Schema = JsonSerializer.Deserialize<PropertyDefinition>(json.Schema),
                            Description = json.SchemaDescription,
                        }
                    };
                    break;

                case ChatResponseFormatJson:
                    request.ResponseFormat = new() { Type = StaticValues.CompletionStatics.ResponseFormat.Json };
                    break;
            }

            // Tools
            request.Tools = options.Tools?.OfType<AIFunction>().Select(f =>
            {
                return ToolDefinition.DefineFunction(new FunctionDefinition()
                {
                    Name = f.Metadata.Name,
                    Description = f.Metadata.Description,
                    Parameters = CreateParameters(f)
                });
            }).ToList() is { Count: > 0 } tools ? tools : null;
            if (request.Tools is not null)
            {
                request.ToolChoice =
                    options.ToolMode is RequiredChatToolMode r ? new ToolChoice()
                    {
                        Type = StaticValues.CompletionStatics.ToolChoiceType.Required,
                        Function = r.RequiredFunctionName is null ? null : new ToolChoice.FunctionTool() { Name = r.RequiredFunctionName }
                    } :
                    options.ToolMode is AutoChatToolMode ? new ToolChoice() { Type = StaticValues.CompletionStatics.ToolChoiceType.Auto } :
                    new ToolChoice() { Type = StaticValues.CompletionStatics.ToolChoiceType.None };
            }
        }

        // Messages
        request.Messages = [];
        foreach (var message in chatMessages)
        {
            foreach (var content in message.Contents)
            {
                switch (content)
                {
                    case TextContent tc:
                        request.Messages.Add(new()
                        {
                            Content = tc.Text,
                            Name = message.AuthorName,
                            Role = message.Role.ToString(),
                        });
                        break;

                    case ImageContent ic:
                        request.Messages.Add(new()
                        {
                            Contents = [new()
                            {
                                Type = "image_url",
                                ImageUrl = new MessageImageUrl()
                                {
                                    Url = ic.Uri,
                                    Detail = ic.AdditionalProperties?.TryGetValue(nameof(MessageImageUrl.Detail), out string? detail) is true ? detail : null,
                                },
                            }],
                            Name = message.AuthorName,
                            Role = message.Role.ToString(),
                        });
                        break;

                    case FunctionResultContent frc:
                        request.Messages.Add(new()
                        {
                            ToolCallId = frc.CallId,
                            Content = frc.Result?.ToString(),
                            Name = message.AuthorName,
                            Role = message.Role.ToString(),
                        });
                        break;
                }
            }

            FunctionCallContent[] fccs = message.Contents.OfType<FunctionCallContent>().ToArray();
            if (fccs.Length > 0)
            {
                request.Messages.Add(new()
                {
                    Name = message.AuthorName,
                    Role = message.Role.ToString(),
                    ToolCalls = fccs.Select(fcc => new ToolCall()
                    {
                        Type = "function",
                        Id = fcc.CallId,
                        FunctionCall = new FunctionCall()
                        {
                            Name = fcc.Name,
                            Arguments = JsonSerializer.Serialize(fcc.Arguments)
                        },
                    }).ToList(),
                });
            }
        }

        return request;
    }

    private static PropertyDefinition CreateParameters(AIFunction f)
    {
        List<string> required = [];
        Dictionary<string, PropertyDefinition> properties = [];

        var parameters = f.Metadata.Parameters;

        foreach (AIFunctionParameterMetadata parameter in parameters)
        {
            properties.Add(parameter.Name, parameter.Schema is JsonElement e ?
                e.Deserialize<PropertyDefinition>()! :
                PropertyDefinition.DefineObject(null, null, null, null, null));

            if (parameter.IsRequired)
            {
                required.Add(parameter.Name);
            }
        }

        return PropertyDefinition.DefineObject(properties, required, null, null, null);
    }

    private static void PopulateContents(ObjectModels.RequestModels.ChatMessage source, IList<AIContent> destination)
    {
        if (source.Content is not null)
        {
            destination.Add(new TextContent(source.Content));
        }

        if (source.Contents is { } contents)
        {
            foreach (MessageContent content in contents)
            {
                if (content.Text is string text)
                {
                    destination.Add(new TextContent(text));
                }

                if (content.ImageUrl is { } url)
                {
                    destination.Add(new ImageContent(url.Url));
                }
            }
        }

        if (source.ToolCalls is { } toolCalls)
        {
            foreach (var tc in toolCalls)
            {
                destination.Add(new FunctionCallContent(
                    tc.Id ?? string.Empty,
                    tc.FunctionCall?.Name ?? string.Empty,
                    tc.FunctionCall?.Arguments is string a ? JsonSerializer.Deserialize<Dictionary<string, object?>>(a) : null));
            }
        }
    }

    private static UsageDetails GetUsageDetails(UsageResponse usage)
    {
        var details = new UsageDetails()
        {
            InputTokenCount = usage.PromptTokens,
            OutputTokenCount = usage.CompletionTokens,
            TotalTokenCount = usage.TotalTokens,
        };

        if (usage.PromptTokensDetails is { } promptDetails)
        {
            Dictionary<string, object?> d = new(StringComparer.OrdinalIgnoreCase);
            (details.AdditionalProperties ??= [])[nameof(usage.PromptTokensDetails)] = d;

            if (promptDetails.CachedTokens is int cachedTokens)
            {
                d[nameof(promptDetails.CachedTokens)] = cachedTokens;
            }

            if (promptDetails.AudioTokens is int audioTokens)
            {
                d[nameof(promptDetails.AudioTokens)] = audioTokens;
            }
        }

        if (usage.CompletionTokensDetails is { } completionDetails)
        {
            Dictionary<string, object?> d = new(StringComparer.OrdinalIgnoreCase);
            (details.AdditionalProperties ??= [])[nameof(usage.CompletionTokensDetails)] = d;

            if (completionDetails.ReasoningTokens is int reasoningTokens)
            {
                d[nameof(completionDetails.ReasoningTokens)] = reasoningTokens;
            }

            if (completionDetails.AudioTokens is int audioTokens)
            {
                d[nameof(promptDetails.AudioTokens)] = audioTokens;
            }
        }

        return details;
    }
}