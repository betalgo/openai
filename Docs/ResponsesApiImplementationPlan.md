# Responses API Implementation Plan

This document tracks the implementation of remaining Responses API features.

## Reference Documents
- [Migration Guide](Migration_Guide.md)
- [Contracts Guide](CONTRACTS_GUIDE.md)
- [Responses Migration Check](ResponsesMigrationCheck.md) - Phase 1 completed

## OpenAPI Source Files
- **Paths**: `Docs/openapi-split/paths/responses/`
- **Schemas**: `Docs/openapi-split/components/schemas/`

---

## Summary

| Phase | Description | Status |
|-------|-------------|--------|
| Phase 1 | Create Response + Core Contracts | ✅ Completed |
| Phase 2 | CRUD Operations (Retrieve, Delete, Cancel) | ✅ Completed |
| Phase 3 | List Input Items | ✅ Completed |
| Phase 4 | Input Token Counting | ✅ Completed |
| Phase 5 | Streaming Support | ⏳ Pending |

---

## Phase 1: Create Response (COMPLETED)

See [ResponsesMigrationCheck.md](ResponsesMigrationCheck.md) for details.

**Endpoints Implemented:**
- ✅ `POST /responses` - Create a model response

**Contract Files:** 77 files verified and committed.

---

## Phase 2: CRUD Operations (COMPLETED)

### 2.1 Retrieve Response

**Endpoint:** `GET /responses/{response_id}`

**YAML Source:** `Docs/openapi-split/paths/responses/param-response_id/get/getresponse.yml`

| Task | File | Status | Notes |
|------|------|--------|-------|
| Request Contract | `Contracts/Requests/Responses/RetrieveResponseRequest.cs` | ✅ Completed | Query params: include, stream, starting_after, include_obfuscation |
| Endpoint Provider Interface | `SDK/EndpointProviders/IOpenAiEndpointProvider.cs` | ✅ Completed | Added `ResponsesRetrieve(string, string?)` |
| OpenAI Endpoint Provider | `SDK/EndpointProviders/OpenAiEndpointProvider.cs` | ✅ Completed | Implemented endpoint |
| Azure Endpoint Provider | `SDK/EndpointProviders/AzureOpenAiEndpointProvider.cs` | ✅ Completed | Implemented endpoint |
| Service Interface | `SDK/Interfaces/IResponsesService.cs` | ✅ Completed | Added `RetrieveResponse()` method |
| Service Implementation | `SDK/Managers/OpenAIResponsesService.cs` | ✅ Completed | Implemented method |

**Response:** Uses existing `Response.cs` contract.

---

### 2.2 Delete Response

**Endpoint:** `DELETE /responses/{response_id}`

**YAML Source:** `Docs/openapi-split/paths/responses/param-response_id/delete/deleteresponse.yml`

| Task | File | Status | Notes |
|------|------|--------|-------|
| Response Contract | `Contracts/Responses/Responses/DeletedResponse.cs` | ✅ Completed | Properties: id, object, deleted |
| Endpoint Provider Interface | `SDK/EndpointProviders/IOpenAiEndpointProvider.cs` | ✅ Completed | Added `ResponsesDelete(string)` |
| OpenAI Endpoint Provider | `SDK/EndpointProviders/OpenAiEndpointProvider.cs` | ✅ Completed | Implemented endpoint |
| Azure Endpoint Provider | `SDK/EndpointProviders/AzureOpenAiEndpointProvider.cs` | ✅ Completed | Implemented endpoint |
| Service Interface | `SDK/Interfaces/IResponsesService.cs` | ✅ Completed | Added `DeleteResponse()` method |
| Service Implementation | `SDK/Managers/OpenAIResponsesService.cs` | ✅ Completed | Implemented method |

---

### 2.3 Cancel Response

**Endpoint:** `POST /responses/{response_id}/cancel`

**YAML Source:** `Docs/openapi-split/paths/responses/param-response_id/cancel/post/cancelresponse.yml`

| Task | File | Status | Notes |
|------|------|--------|-------|
| Endpoint Provider Interface | `SDK/EndpointProviders/IOpenAiEndpointProvider.cs` | ✅ Completed | Added `ResponsesCancel(string)` |
| OpenAI Endpoint Provider | `SDK/EndpointProviders/OpenAiEndpointProvider.cs` | ✅ Completed | Implemented endpoint |
| Azure Endpoint Provider | `SDK/EndpointProviders/AzureOpenAiEndpointProvider.cs` | ✅ Completed | Implemented endpoint |
| Service Interface | `SDK/Interfaces/IResponsesService.cs` | ✅ Completed | Added `CancelResponse()` method |
| Service Implementation | `SDK/Managers/OpenAIResponsesService.cs` | ✅ Completed | Implemented method |

**Response:** Uses existing `Response.cs` contract.

---

## Phase 3: List Input Items (COMPLETED)

**Endpoint:** `GET /responses/{response_id}/input_items`

**YAML Sources:**
- Path: `Docs/openapi-split/paths/responses/param-response_id/input_items/get/listinputitems.yml`
- Schema: `Docs/openapi-split/components/schemas/responseitemlist.yml`

| Task | File | Status | Notes |
|------|------|--------|-------|
| Order Enum | `Contracts/Enums/Responses/InputItemsOrder.cs` | ✅ Completed | Smart Enum: asc, desc |
| Request Contract | `Contracts/Requests/Responses/InputItemsListRequest.cs` | ✅ Completed | Pagination: limit, order, after, include |
| Response Contract | `Contracts/Responses/Responses/ResponseItemList.cs` | ✅ Completed | Properties: object, data, has_more, first_id, last_id |
| Endpoint Provider Interface | `SDK/EndpointProviders/IOpenAiEndpointProvider.cs` | ✅ Completed | Added `ResponsesInputItemsList(string, string?)` |
| OpenAI Endpoint Provider | `SDK/EndpointProviders/OpenAiEndpointProvider.cs` | ✅ Completed | Implemented endpoint with query string |
| Azure Endpoint Provider | `SDK/EndpointProviders/AzureOpenAiEndpointProvider.cs` | ✅ Completed | Implemented endpoint |
| Service Interface | `SDK/Interfaces/IResponsesService.cs` | ✅ Completed | Added `ListInputItems()` method |
| Service Implementation | `SDK/Managers/OpenAIResponsesService.cs` | ✅ Completed | Implemented method |

---

## Phase 4: Input Token Counting (COMPLETED)

**Endpoint:** `POST /responses/input_tokens`

**YAML Sources:**
- Path: `Docs/openapi-split/paths/responses/input_tokens/post/getinputtokencounts.yml`
- Request Schema: `Docs/openapi-split/components/schemas/tokencountsbody.yml`
- Response Schema: `Docs/openapi-split/components/schemas/tokencountsresource.yml`

| Task | File | Status | Notes |
|------|------|--------|-------|
| Request Contract | `Contracts/Requests/Responses/TokenCountsRequest.cs` | ✅ Completed | 11 properties from tokencountsbody.yml |
| Response Contract | `Contracts/Responses/Responses/TokenCountsResponse.cs` | ✅ Completed | Properties: object, input_tokens |
| Endpoint Provider Interface | `SDK/EndpointProviders/IOpenAiEndpointProvider.cs` | ✅ Completed | Added `ResponsesInputTokensCount()` |
| OpenAI Endpoint Provider | `SDK/EndpointProviders/OpenAiEndpointProvider.cs` | ✅ Completed | Implemented endpoint |
| Azure Endpoint Provider | `SDK/EndpointProviders/AzureOpenAiEndpointProvider.cs` | ✅ Completed | Implemented endpoint |
| Service Interface | `SDK/Interfaces/IResponsesService.cs` | ✅ Completed | Added `CountInputTokens()` method |
| Service Implementation | `SDK/Managers/OpenAIResponsesService.cs` | ✅ Completed | Implemented method |

---

## Phase 5: Streaming Support (Future)

**YAML Source:** `Docs/openapi-split/components/schemas/responsestreamevent.yml`

Streaming support requires implementing 50+ event types. This is a significant effort and should be planned separately.

### Event Types to Implement

| Category | Event Types | Count |
|----------|-------------|-------|
| Response Lifecycle | Created, InProgress, Completed, Failed, Incomplete, Queued, Error | 7 |
| Content | ContentPartAdded, ContentPartDone | 2 |
| Text | TextDelta, TextDone, RefusalDelta, RefusalDone | 4 |
| Audio | AudioDelta, AudioDone, AudioTranscriptDelta, AudioTranscriptDone | 4 |
| Output Items | OutputItemAdded, OutputItemDone | 2 |
| Function Calls | FunctionCallArgumentsDelta, FunctionCallArgumentsDone | 2 |
| Code Interpreter | CodeDelta, CodeDone, Completed, InProgress, Interpreting | 5 |
| File Search | Completed, InProgress, Searching | 3 |
| Web Search | Completed, InProgress, Searching | 3 |
| Image Generation | Completed, Generating, InProgress, PartialImage | 4 |
| MCP | ArgumentsDelta, ArgumentsDone, Completed, Failed, InProgress, ListToolsCompleted, ListToolsFailed, ListToolsInProgress | 8 |
| Reasoning | SummaryPartAdded, SummaryPartDone, SummaryTextDelta, SummaryTextDone, TextDelta, TextDone | 6 |
| Other | OutputTextAnnotationAdded, CustomToolCallInputDelta, CustomToolCallInputDone | 3 |

**Total:** ~53 event types

### Streaming Implementation Tasks

| Task | File | Status | Notes |
|------|------|--------|-------|
| Base Event Interface | `Contracts/Types/Streaming/IStreamEvent.cs` | ⏳ Pending | Base interface for all events |
| Event Contracts | `Contracts/Types/Streaming/*.cs` | ⏳ Pending | 53 event type contracts |
| Stream Extension | `SDK/Extensions/ResponsesStreamExtensions.cs` | ⏳ Pending | CreateResponseAsStreamAsync, RetrieveResponseAsStreamAsync |

---

## Contract File Checklist

### New Enum Files

| File | Status | YAML Source |
|------|--------|-------------|
| `Enums/Responses/InputItemsOrder.cs` | ✅ Completed | listinputitems.yml |

### New Request Files

| File | Status | YAML Source |
|------|--------|-------------|
| `Requests/Responses/RetrieveResponseRequest.cs` | ✅ Completed | getresponse.yml |
| `Requests/Responses/InputItemsListRequest.cs` | ✅ Completed | listinputitems.yml |
| `Requests/Responses/TokenCountsRequest.cs` | ✅ Completed | tokencountsbody.yml |

### New Response Files

| File | Status | YAML Source |
|------|--------|-------------|
| `Responses/Responses/DeletedResponse.cs` | ✅ Completed | deleteresponse.yml (inline) |
| `Responses/Responses/ResponseItemList.cs` | ✅ Completed | responseitemlist.yml |
| `Responses/Responses/TokenCountsResponse.cs` | ✅ Completed | tokencountsresource.yml |

---

## SDK File Checklist

### Endpoint Provider Files

| File | Methods Added | Status |
|------|--------------|--------|
| `IOpenAiEndpointProvider.cs` | ResponsesRetrieve, ResponsesDelete, ResponsesCancel, ResponsesInputItemsList, ResponsesInputTokensCount | ✅ Completed |
| `OpenAiEndpointProvider.cs` | All 5 methods | ✅ Completed |
| `AzureOpenAiEndpointProvider.cs` | All 5 methods | ✅ Completed |

### Service Files

| File | Methods Added | Status |
|------|--------------|--------|
| `IResponsesService.cs` | RetrieveResponse, DeleteResponse, CancelResponse, ListInputItems, CountInputTokens | ✅ Completed |
| `OpenAIResponsesService.cs` | All 5 methods | ✅ Completed |

---

## Implementation Notes

### Conventions Followed

1. **Smart Enums**: Used `readonly struct` pattern with `IEquatable<T>`, not C# `enum`
2. **Constructors**: Empty constructor + required parameters constructor
3. **JSON Serialization**: `[JsonPropertyName("snake_case")]` with PascalCase C# properties
4. **Documentation**: XML summary from YAML description, links to OpenAI API reference and source YAML
5. **Contracts**: Pure DTOs only - no logic, extension methods go in SDK
6. **Validation**: Every contract verified against `Docs/openapi-split/` YAML files

### Patterns Followed

- **Delete Operations**: Followed `DeletionStatusResponse` pattern from Assistants API
- **Pagination**: Followed `PaginationRequest` pattern with `GetQueryParameters()` method
- **Retrieve Operations**: Followed `AssistantRetrieve` pattern in `OpenAIAssistantService.cs`
- **Cancel Operations**: Similar to `RunCancel` pattern

---

## Progress Log

| Date | Phase | Task | Status |
|------|-------|------|--------|
| - | 1 | Initial Responses API contracts | ✅ Completed |
| Dec 22, 2025 | 2.1 | RetrieveResponse endpoint | ✅ Completed |
| Dec 22, 2025 | 2.2 | DeleteResponse endpoint | ✅ Completed |
| Dec 22, 2025 | 2.3 | CancelResponse endpoint | ✅ Completed |
| Dec 22, 2025 | 3 | ListInputItems endpoint | ✅ Completed |
| Dec 22, 2025 | 4 | CountInputTokens endpoint | ✅ Completed |
| - | 5 | Streaming Support | ⏳ Not Started |

---

## New Files Created (Phases 2-4)

### Contracts Project
1. `Betalgo.Ranul.OpenAI.Contracts/Enums/Responses/InputItemsOrder.cs`
2. `Betalgo.Ranul.OpenAI.Contracts/Requests/Responses/RetrieveResponseRequest.cs`
3. `Betalgo.Ranul.OpenAI.Contracts/Requests/Responses/InputItemsListRequest.cs`
4. `Betalgo.Ranul.OpenAI.Contracts/Requests/Responses/TokenCountsRequest.cs`
5. `Betalgo.Ranul.OpenAI.Contracts/Responses/Responses/DeletedResponse.cs`
6. `Betalgo.Ranul.OpenAI.Contracts/Responses/Responses/ResponseItemList.cs`
7. `Betalgo.Ranul.OpenAI.Contracts/Responses/Responses/TokenCountsResponse.cs`

### SDK Project (Modified)
1. `OpenAI.SDK/EndpointProviders/IOpenAiEndpointProvider.cs` - Added 5 methods
2. `OpenAI.SDK/EndpointProviders/OpenAiEndpointProvider.cs` - Added 5 methods
3. `OpenAI.SDK/EndpointProviders/AzureOpenAiEndpointProvider.cs` - Added 5 methods
4. `OpenAI.SDK/Interfaces/IResponsesService.cs` - Added 5 methods
5. `OpenAI.SDK/Managers/OpenAIResponsesService.cs` - Added 5 methods

---

## Next Steps

1. ~~Start with Phase 2 (CRUD Operations) - simplest to implement~~ ✅
2. ~~Progress to Phase 3 (List Input Items) - requires pagination~~ ✅
3. ~~Complete Phase 4 (Token Counting) - new request/response types~~ ✅
4. Plan Phase 5 (Streaming) separately due to complexity (~53 event types)

---

*Last Updated: December 22, 2025*
