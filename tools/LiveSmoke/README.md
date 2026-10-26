# Verify a candidate through its actual NuGet packages

This separate consumer references a package, never the SDK project. It supports
eight bounded scenarios: `basic`, `stream`, `structured`, `function`, `context`,
`function-generated`, `function-generated-net9`, `manual-context`.
Each has a local HTTP fixture mode and an explicit paid live mode. The default
`--preflight` reports configuration and package identity without sending requests.

Build the candidate SDK, Contracts and Utilities with the **same unique prerelease version**,
then pack all three with `--no-build`. `GeneratePackageOnBuild` means a pack operation
alone may reuse old binaries: explicitly build first and verify the informational
version in the consumer's report. Restore into a new package directory to avoid
silently reusing an earlier package with the same version.

Example from the repository root (replace VERSION and FEED with your values):

```sh
dotnet build OpenAI.SDK/Betalgo.Ranul.OpenAI.csproj -c Release -p:Version=VERSION
dotnet build OpenAI.Utilities/Betalgo.OpenAI.Utilities.csproj -c Release -p:Version=VERSION
dotnet pack OpenAI.SDK/Betalgo.Ranul.OpenAI.csproj -c Release --no-build -p:Version=VERSION -o FEED
dotnet pack Betalgo.Ranul.OpenAI.Contracts/Betalgo.Ranul.OpenAI.Contracts.csproj -c Release --no-build -p:Version=VERSION -o FEED
dotnet pack OpenAI.Utilities/Betalgo.OpenAI.Utilities.csproj -c Release --no-build -p:Version=VERSION -o FEED
dotnet build tools/LiveSmoke/ResponsesSmoke.csproj -c Release -p:CandidateVersion=VERSION --packages FRESH_PACKAGE_DIRECTORY --source FEED --source https://api.nuget.org/v3/index.json
dotnet tools/LiveSmoke/bin/Release/net9.0/ResponsesSmoke.dll --offline structured
```

Run all eight offline scenarios before making live calls. Configure
`OPENAI_API_KEY` and `OPENAI_TEST_MODEL` privately, then replace `--offline` with
`--live` for one selected scenario. Do not put the key into a command argument,
source file, report or shell history. Use a dedicated test project and a local
budget ledger. Choose a model supporting Responses, function tools, strict JSON,
and `reasoning.effort=none`; pricing and model compatibility must be reviewed
before the live run. The harness does not enforce an account-wide dollar cap.

Each generation uses short fixed synthetic prompts, Standard service tier,
256 maximum output tokens and a 60-second workflow deadline. There are no retries
or billed built-in tools. Basic, streaming and structured use one request each.
All function scenarios and stored context use two generation requests and at most two cleanup DELETE
requests. Manual context uses two generation requests with `Store=false`, replays the
original user message plus `first.ToInputItems()` and the follow-up user message,
and never sets `previous_response_id` or issues cleanup deletes.
Stored response cleanup runs even on assertion failure and has its own
20-second deadline. Cleanup failures make the scenario fail.

Reports contain package commit/version, runtime, model, request IDs, usage and
assertion status. Provider error bodies and credential headers are not persisted.
A `LIVE_PASS` proves only that selected workflow on the recorded model/environment;
it does not establish support for every model or platform. Realtime and media
require their own live evidence.


The original `function` scenario keeps its hand-written strict schema.
`function-generated` calls `FunctionCallingHelper.CreateFunctionToolFromType<T>()`;
`function-generated-net9` calls `CreateFunctionToolFromTypeNet9<T>()`. Both use the
packaged Utilities assembly and validate a generated nested object/array schema,
strict object closure, required properties, function call arguments and the
correlated second response. Offline fixtures reject malformed schemas (including
spurious `const:null` constraints). The generated schemas are also sent unchanged
in paid mode, so a live pass verifies API acceptance rather than only local shape.

The manual replay fixture verifies assistant ID, phase, extra message metadata,
content and history order survive the actual outbound request. Offline mode does
not read the API key or model environment variables. Reports include both SDK and
Utilities informational versions; compare both against the candidate packages.

```sh
for scenario in basic stream structured function context function-generated function-generated-net9 manual-context; do
  dotnet tools/LiveSmoke/bin/Release/net9.0/ResponsesSmoke.dll --offline "$scenario" || exit 1
done
```
