# Responses integration tests

A normal `dotnet test OpenAI.IntegrationTests` runs offline configuration tests and
skips paid API tests. Merely configuring credentials never enables live tests.
The offline tests inject an HTTP handler and verify the authorization header from
the actual DI-created SDK client; no OpenAI requests are made.

## Live tests

Configure credentials locally using an environment variable or user secrets.
Never put an API key into committed appsettings files.

```sh
dotnet user-secrets set OpenAIServiceOptions:ApiKey '<key>' --project OpenAI.IntegrationTests
export OPENAI_RUN_LIVE_TESTS=1
dotnet test OpenAI.IntegrationTests --filter 'FullyQualifiedName~BasicResponseTests'
```

The fixture reads providers in this order: appsettings.json,
appsettings.Development.json, user secrets, environment variables (last wins).
It then selects the first nonblank key from:

1. `OPENAI_API_KEY`
2. `OpenAIServiceOptions:ApiKey` (`OpenAIServiceOptions__ApiKey` in the environment)
3. Legacy `OpenAI:ApiKey` (`OpenAI__ApiKey` in the environment)

The chosen key is both validated and assigned to the SDK options. Other SDK
options retain normal `OpenAIServiceOptions` configuration binding.

Model selection is independent of credentials:

| Setting | Fallback |
| --- | --- |
| `OPENAI_TEST_MODEL` | `gpt-6-luna`, the current verified core model; review explicit overrides |
| `OPENAI_TEST_REASONING_MODEL` | No fallback; reasoning test remains skipped without an explicit environment model ID |
| `OPENAI_TEST_WEB_SEARCH_MODEL` | No fallback; web-search test remains skipped without an explicit environment model ID |
| `OPENAI_TEST_CODE_INTERPRETER_MODEL` | No fallback; code test remains skipped without an explicit environment model ID |
| `OPENAI_TEST_MAX_OUTPUT_TOKENS` | 256; accepts 1–4096 |

Legacy `OpenAI:DefaultModel` and `OpenAI:ReasoningModel` settings no longer select
paid models. A capability model environment setting plus `OPENAI_RUN_LIVE_TESTS=1`
is an explicit opt-in to that capability's charges. Merely selecting a core model
does not enable hosted tools. Review model compatibility, the token cap and any
hosted-tool/container fees before setting these capability environment variables.

The configured output cap applies to generated-response tests, including reasoning
and tools. Small 20/50-token limits have been replaced because reasoning overhead
can consume them before a final answer. HTTP requests have a 60-second timeout;
streaming tests also pass a 60-second cancellation token covering stream reads.
There are no automatic retries. These controls bound individual requests, not
total monetary spend or repeated test runs.

Ordinary tests set `store: false`. Stateful tests use `StoredResponseScope` to
record IDs before assertions and perform cleanup even if assertions or later calls
fail. Cleanup gets a separate 15-second bound, attempts all known IDs in reverse
order and reports failures. It cannot clean up a server resource whose ID never
reached the client. Explicit deletion tests forget an ID only after successful
deletion. Hosted code containers remain subject to the platform's lifecycle and
fees; response deletion does not imply container deletion.

Tool tests force a hosted call and require an actual completed typed call; text-only
answers cannot pass. The code test explicitly requests an automatic container and
checks the numeric result. The reasoning test requires measured reasoning-token
usage and the correct arithmetic result, rather than a prose-length heuristic.
These stronger assertions have offline build/configuration coverage and still
require separately recorded live evidence on the reviewed capability models.

Run focused filters first and track usage externally. The full suite makes many
requests, includes built-in tools and stored responses, and may incur additional
charges. Existing prompts and assertions include model-dependent assumptions
(e.g. short exact answers and reasoning-token expectations). Choosing a different
model does not establish that those assertions are valid for it. Skipped tests and
offline passes are not evidence that live API features work.
