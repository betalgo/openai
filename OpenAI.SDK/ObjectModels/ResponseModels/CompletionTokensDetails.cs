﻿using System.Text.Json.Serialization;

namespace Betalgo.Ranul.OpenAI.ObjectModels.ResponseModels;

public record CompletionTokensDetails
{
    [JsonPropertyName("reasoning_tokens")]
    public int ReasoningTokens { get; set; }
    [JsonPropertyName("audio_tokens")]
    public int AudioTokens { get; set; }
}