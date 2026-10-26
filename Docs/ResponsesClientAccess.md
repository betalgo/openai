# Accessing Responses without breaking existing clients

`OpenAIService` provides `sdk.Responses` and directly implements
`IResponsesService`. Clients obtained through the existing `IOpenAIService`
interface use the optional capability extension:

```csharp
using Betalgo.Ranul.OpenAI.Extensions;

var responses = service.AsResponses();
var result = await responses.CreateResponse(request, cancellationToken);
```

`IResponsesProvider` supplies the optional `Responses` property. The original
`IOpenAIService` contract keeps its existing members, so old custom implementers
and decorators do not need to add a new member merely to upgrade the package.
A custom implementation that does not expose Responses receives an explicit
`NotSupportedException` from `AsResponses()`. A decorator can opt in by implementing
`IResponsesProvider` and forwarding its property deliberately.

Concrete `OpenAIService` examples can continue using `.Responses`. The integration
fixture exercises access through the original interface and an offline compile
fixture implements that interface with only its original members.
