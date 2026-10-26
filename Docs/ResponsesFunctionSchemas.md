# Generated Responses function schemas

The Responses function helpers serialize typed parameter schemas without absent JSON Schema keywords. A null-valued keyword such as `const: null` is a constraint, so it must not be emitted merely because a `PropertyDefinition` field was unset. Raw schema strings supplied by the caller remain unchanged.

When a typed helper explicitly sets `strict: true` (the default on `CreateFunctionToolFromType`), it closes generated object schemas with `additionalProperties: false` and lists every property as required, including nested objects, arrays and definitions. This follows the [official function-calling strict-mode requirements](https://developers.openai.com/api/docs/guides/function-calling#strict-mode), checked 26 October 2026. If a property can hold null, express that in its schema/type. Explicit `strict: false` preserves the generated required-field policy. This normalization is confined to Responses helpers; existing Chat Completions schema generation is unchanged.

The .NET 9 exporter initializes a reflection resolver when none was supplied and clones caller options before freezing/exporting. Naming policies and explicitly supplied resolvers are preserved; the caller's options remain mutable. Function arguments have an object root, so a CLR reference type's nullable root is exported as an object parameter document in strict mode.

`ResponsesGeneratedSchemaTests` checks actual outbound HTTP JSON for both type generators, nested required fields, omitted schema keywords, caller options, explicit non-strict behavior and raw-schema pass-through. The default resolver and required-field regressions failed before the fix. This is offline contract verification; no generated schema has been sent to a live API in this change.
