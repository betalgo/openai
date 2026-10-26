# Responses convenience helpers

`GetTextAsync()` collects text deltas and `GetFinalTextAsync()` collects completed text parts. Both return text only when the response completes successfully. Failed or incomplete responses throw `ResponseStreamException`; inspect its `Response` for status, API error and incomplete details, or `ErrorEvent` for a standalone streaming error. A stream that ends without successful completion throws `EndOfStreamException`. Cancellation remains an `OperationCanceledException`.

Use `GetFinalResponseAsync()` when the application needs to handle terminal states itself. It returns the terminal completed, failed or incomplete response without converting a failed state into an exception. Incremental display can still use `WithTextAccumulation()` and inspect the accompanying events.

`response.ToInputItems()` snapshots output items for a later request's `Input` list. It preserves the output converter's serialized fields and item order, including output-only tool types, opaque payloads, IDs and message phase. The replay items are JSON snapshots; edits to the source response do not change them. Only fields retained by the original output DTO/converter can be replayed. The helper copies all output items; the caller still decides which responses belong in its conversation history and handles tool results separately.
