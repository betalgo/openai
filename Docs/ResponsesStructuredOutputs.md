# Structured outputs with Responses

Responses places schema configuration directly in `text.format`:

```csharp
Text = new TextConfig
{
    Format = TextFormat.JsonSchema("answer", new Dictionary<string, object>
    {
        ["type"] = "object",
        ["properties"] = new Dictionary<string, object>
        {
            ["answer"] = new Dictionary<string, object> { ["type"] = "string" }
        },
        ["required"] = new[] { "answer" },
        ["additionalProperties"] = false
    }, strict: true)
}
```

Import `Betalgo.Ranul.OpenAI.Contracts.Types` and `.Types.TextFormat`.
The existing `JsonSchemaFormat.JsonSchema` convenience property remains usable;
its fields serialize flat as `name`, `schema`, `description` and `strict`.
The former nested `json_schema` wire shape was invalid for Responses. Chat
Completions has a separate format contract and is unchanged.

Check response status and refusal content before parsing `OutputText` as JSON.
An incomplete response can contain partial JSON. A safety refusal is represented
as refusal content instead of the requested schema. The SDK does not execute
model-generated tool calls or assume a completed response implies useful JSON.

Wire regression tests cover strict true/false, flat request serialization,
response deserialization and plain/JSON-object format round trips. Live package
validation is recorded separately; an offline pass does not prove model support.

Reference checked 2026-10-26:
https://developers.openai.com/api/docs/guides/structured-outputs?api-mode=responses
