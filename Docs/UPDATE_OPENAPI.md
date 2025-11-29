# Updating OpenAPI Documentation

This project uses [Blueflow Chopper](https://github.com/betalgo/Blueflow) to split the large OpenAPI specification into smaller, manageable files. This is helpful for AI-assisted development.

## Prerequisites

You need the .NET SDK installed.

## How to Update

We use a local tool to ensure everyone uses the same version.

1.  **One-time setup** (if you haven't done this before):
    ```bash
    dotnet tool restore
    ```

2.  **Run the update command**:

    **Windows (PowerShell):**
    ```powershell
    dotnet blueflow-chopper --url "https://app.stainless.com/api/spec/documented/openai/openapi.documented.yml" --output "Docs/openapi-split" --clean
    ```

    **Mac/Linux (Bash):**
    ```bash
    dotnet blueflow-chopper --url "https://app.stainless.com/api/spec/documented/openai/openapi.documented.yml" --output "Docs/openapi-split" --clean
    ```

The split files will be generated in `Docs/openapi-split`.

