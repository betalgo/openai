# Azure Responses resource routes

Checked 26 October 2026 against Microsoft's [2025-04-01-preview inference specification](https://github.com/Azure/azure-rest-api-specs/blob/59c8e482c0403f60118e2a8dc7c4a88e435db8b0/specification/cognitiveservices/data-plane/AzureOpenAI/inference/preview/2025-04-01-preview/inference.json) and [current Responses REST reference](https://learn.microsoft.com/en-us/rest/api/microsoft-foundry/azureopenai/responses).

The dated specification's server is `https://{endpoint}/openai`; Responses paths are `/responses`, `/responses/{response_id}` and `/responses/{response_id}/input_items`. They are resource-level operations. The caller's deployment name belongs in the request's `model` value, rather than a deployment path segment.

The Azure provider now builds Responses routes by selected API family:

| `OpenAIOptions.ApiVersion` | Responses prefix | Version query |
|---|---|---|
| Dated version, such as `2025-04-01-preview` | `/openai/responses` | Caller-selected dated `api-version` |
| `v1` | `/openai/v1/responses` | Omitted; v1 is implicit |
| `preview` | `/openai/v1/responses` | `api-version=preview` |

This change does not select a newer API version, replace caller model IDs, change authentication, or change Chat Completions deployment routes. Azure options still require `DeploymentId` for existing service configuration, even though Responses does not put it in the path. Set the request `Model` to the intended Responses deployment, or configure the SDK default model explicitly.

Resource routes alone do not establish that every operation/tool is supported by every Azure version. In particular, the dated specification does not establish support for later cancel/token-count features. Check the selected Azure API version and deployment capabilities. The offline HTTP fixtures verify route family, query joining, body model, API-key header, escaped response IDs and unchanged dated Chat routing. No Azure live call was made.
