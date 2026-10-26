# Build and test the library

Install the .NET 9 SDK to build the current solution and run its test project.
The SDK and Contracts projects also build for .NET 8 and .NET Standard 2.0;
building the solution checks each declared target. Running a test on .NET 9
does not exercise every target or establish device-specific compatibility.

From the repository root:

```sh
dotnet restore OpenAI.sln
dotnet build OpenAI.sln --configuration Release --no-restore
dotnet test OpenAI.Utilities.Tests/OpenAI.Utilities.Tests.csproj --configuration Release --no-build --no-restore --logger trx --results-directory TestResults
```

The test command above runs the current offline Utilities tests. It does not
require an API key or call OpenAI. The playground projects contain examples
that make service requests; building those projects does not run them.

## Pull request validation

The `Validate` workflow runs on pull requests to `dev` or `master`, pushes to
`dev`, and manual dispatch. It builds the full solution and runs the offline
test project on Linux, Windows, and macOS. Test results are retained as workflow
artifacts, including when tests fail. The workflow does not publish packages
or use API credentials. Existing compiler/package warnings are reported;
this change does not introduce a warnings-as-errors policy.

When adding another offline test project, explicitly add it to this workflow's
test step. Keep paid live integration checks separate so a routine contributor
check cannot unexpectedly make API calls. Add each project to the solution
when its build belongs in the standard validation path.

## Verifying a release candidate against the service

Offline tests check behavior that can be reproduced with local data. Claiming
support for a new API workflow also requires a live check through the actual
library package:

1. Build and pack the candidate SDK and its Contracts dependency.
2. Restore those packages into a fresh consumer application from a local feed.
3. Configure a dedicated OpenAI test project/key outside source control, with
   bounded usage. Do not put keys in test reports or package artifacts.
4. Exercise each workflow included in the release scope against the real API.
   For Responses, cover basic output, streaming, function-call round trips,
   structured output, and the advertised follow-up context behavior.
5. Record the commit, package version, OS/runtime, model, timestamp, pass/fail
   results, usage and request IDs where available. Mark unrun cases explicitly.
6. Verify the published packages in a clean consumer application after release.

Do not report offline validation as live API verification. Faults such as
malformed payloads or dropped streams should also have repeatable local
regression tests. Platform-specific claims require the corresponding environment.
