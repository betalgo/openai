﻿using Betalgo.Ranul.OpenAI.ObjectModels.RequestModels;
using Betalgo.Ranul.OpenAI.ObjectModels.SharedModels;

namespace Betalgo.Ranul.OpenAI.Builders;

/// <summary>
///     FunctionDefinitionBuilder is used to build and validate a FunctionDefinition object.
/// </summary>
public class FunctionDefinitionBuilder
{
    /// <summary>
    ///     String constant for validation of function name.
    /// </summary>
    private const string ValidNameChars = "abcdefghijklmnopqrstuvwxyz" + "ABCDEFGHIJKLMNOPQRSTUVWXYZ" + "0123456789-_";

    private readonly FunctionDefinition _definition;

    /// <summary>
    ///     Initializes a new instance of FunctionDefinitionBuilder.
    /// </summary>
    /// <param name="name">The name of the function</param>
    /// <param name="description">The optional description of the function</param>
    /// <param name="strict">Whether to enable strict schema adherence when generating the function call. If set to true, the model will follow the exact schema defined in the parameters field. Only a subset of JSON Schema is supported when strict is true. Learn more about Structured Outputs in the function calling guide.</param>
    public FunctionDefinitionBuilder(string name, string? description = null, bool? strict =null)
    {
        _definition = new()
        {
            Name = name,
            Description = description,
            Strict = strict,
            Parameters = new()
            {
                Properties = new Dictionary<string, PropertyDefinition>()
            }
        };
    }

    public FunctionDefinitionBuilder AddParameter(string name, PropertyDefinition value, bool required = true)
    {
        var pars = _definition.Parameters!;
        pars.Properties![name] = value;

        if (required)
        {
            pars.Required ??= new List<string>();
            pars.Required.Add(name);
        }

        return this;
    }

    /// <summary>
    ///     Validates the function definition.
    /// </summary>
    /// <returns>The FunctionDefinitionBuilder instance for fluent configuration</returns>
    public FunctionDefinitionBuilder Validate()
    {
        ValidateName(_definition.Name);
        return this;
    }

    /// <summary>
    ///     Builds the FunctionDefinition object.
    /// </summary>
    /// <returns>The built FunctionDefinition object</returns>
    public FunctionDefinition Build()
    {
        return _definition;
    }

    /// <summary>
    ///     Validates the name of the function.
    /// </summary>
    /// <param name="functionName">The name of the function to validate</param>
    public static void ValidateName(string functionName)
    {
        var invalidChars = functionName.Where(ch => !ValidNameChars.Contains(ch)).ToList();
        if (functionName.Length > 64 || invalidChars.Count > 0)
        {
            var message = "The name of the function must be a-z, A-Z, 0-9, or contain underscores and dashes, with a maximum length of 64.";
            if (functionName.Length > 64)
            {
                message = "Function name is too long. " + message;
            }

            if (invalidChars.Count > 0)
            {
                message = $"Function name contains invalid characters: {string.Join(",", invalidChars)}. " + message;
            }

            throw new ArgumentOutOfRangeException(nameof(functionName), message);
        }
    }
}