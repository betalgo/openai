using System.Reflection;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
#if NET9_0_OR_GREATER
using System.Text.Json.Schema;
#endif
using Betalgo.Ranul.OpenAI.Builders;
using Betalgo.Ranul.OpenAI.Contracts.Types.Items;
using Betalgo.Ranul.OpenAI.Contracts.Types.Tools;
using Betalgo.Ranul.OpenAI.ObjectModels.RequestModels;
using Betalgo.Ranul.OpenAI.ObjectModels.SharedModels;

namespace Betalgo.OpenAI.Utilities.FunctionCalling;

/// <summary>
///     Helper methods for Function Calling.
///     Supports both Chat Completions API (ToolDefinition) and Responses API (FunctionTool/ITool).
/// </summary>
public static class FunctionCallingHelper
{
    /// <summary>
    ///     Returns a <see cref="FunctionDefinition" /> from the provided method, using any
    ///     <see cref="FunctionDescriptionAttribute" /> and <see cref="ParameterDescriptionAttribute" /> attributes
    /// </summary>
    /// <param name="methodInfo">the method to create the <see cref="FunctionDefinition" /> from</param>
    /// <returns>the <see cref="FunctionDefinition" /> created.</returns>
    public static FunctionDefinition GetFunctionDefinition(MethodInfo methodInfo)
    {
        var methodDescriptionAttribute = methodInfo.GetCustomAttribute<FunctionDescriptionAttribute>();

        var result = new FunctionDefinitionBuilder(methodDescriptionAttribute?.Name ?? methodInfo.Name, methodDescriptionAttribute?.Description);

        var parameters = methodInfo.GetParameters().ToList();

        foreach (var parameter in parameters)
        {
            var parameterDescriptionAttribute = parameter.GetCustomAttribute<ParameterDescriptionAttribute>();
            var description = parameterDescriptionAttribute?.Description;

            PropertyDefinition definition;

            switch (parameter.ParameterType, parameterDescriptionAttribute?.Type == null)
            {
                case (_, false):
                    definition = new()
                    {
                        Type = parameterDescriptionAttribute!.Type!,
                        Description = description
                    };

                    break;
                case ({ } t, _) when t.IsAssignableFrom(typeof(int)):
                    definition = PropertyDefinition.DefineInteger(description);
                    break;
                case ({ } t, _) when t.IsAssignableFrom(typeof(float)):
                    definition = PropertyDefinition.DefineNumber(description);
                    break;
                case ({ } t, _) when t.IsAssignableFrom(typeof(bool)):
                    definition = PropertyDefinition.DefineBoolean(description);
                    break;
                case ({ } t, _) when t.IsAssignableFrom(typeof(string)):
                    definition = PropertyDefinition.DefineString(description);
                    break;
                case ({ IsEnum: true }, _):

                    var enumValues = string.IsNullOrEmpty(parameterDescriptionAttribute?.Enum) ? Enum.GetNames(parameter.ParameterType).ToList() : parameterDescriptionAttribute.Enum.Split(',').Select(x => x.Trim()).ToList();

                    definition = PropertyDefinition.DefineEnum(enumValues, description);

                    break;
                default:
                    throw new($"Parameter type '{parameter.ParameterType}' not supported");
            }

            result.AddParameter(parameterDescriptionAttribute?.Name ?? parameter.Name!, definition, parameterDescriptionAttribute?.Required ?? true);
        }

        return result.Build();
    }

    public static ToolDefinition GetToolDefinition(MethodInfo methodInfo)
    {
        return new()
        {
            Type = "function",
            Function = GetFunctionDefinition(methodInfo)
        };
    }

    /// <summary>
    ///     Enumerates the methods in the provided object, and a returns a <see cref="List{FunctionDefinition}" /> of
    ///     <see cref="FunctionDefinition" /> for all methods
    ///     marked with a <see cref="FunctionDescriptionAttribute" />
    /// </summary>
    /// <param name="obj">the object to analyze</param>
    public static List<ToolDefinition> GetToolDefinitions(object obj)
    {
        var type = obj.GetType();
        return GetToolDefinitions(type);
    }

    /// <summary>
    ///     Enumerates the methods in the provided type, and a returns a <see cref="List{FunctionDefinition}" /> of
    ///     <see cref="FunctionDefinition" /> for all methods
    /// </summary>
    /// <typeparam name="T">The type to analyze</typeparam>
    /// <returns></returns>
    public static List<ToolDefinition> GetToolDefinitions<T>()
    {
        return GetToolDefinitions(typeof(T));
    }

    /// <summary>
    ///     Enumerates the methods in the provided type, and a returns a <see cref="List{FunctionDefinition}" /> of
    ///     <see cref="FunctionDefinition" /> for all methods
    /// </summary>
    /// <param name="type">The type to analyze</param>
    public static List<ToolDefinition> GetToolDefinitions(Type type)
    {
        var methods = type.GetMethods();

        var result = methods.Select(method => new
            {
                method,
                methodDescriptionAttribute = method.GetCustomAttribute<FunctionDescriptionAttribute>()
            })
            .Where(t => t.methodDescriptionAttribute != null)
            .Select(t => GetToolDefinition(t.method))
            .ToList();

        return result;
    }


    /// <summary>
    ///     Calls the function on the provided object, using the provided <see cref="FunctionCall" /> and returns the result of
    ///     the call
    /// </summary>
    /// <param name="functionCall">The FunctionCall provided by the LLM</param>
    /// <param name="obj">the object with the method / function to be executed</param>
    /// <typeparam name="T">The return type</typeparam>
    public static T? CallFunction<T>(FunctionCall functionCall, object obj)
    {
        if (functionCall == null)
        {
            throw new ArgumentNullException(nameof(functionCall));
        }

        if (functionCall.Name == null)
        {
            throw new InvalidFunctionCallException("Function Name is null");
        }

        if (obj == null)
        {
            throw new ArgumentNullException(nameof(obj));
        }

        var methodInfo = obj.GetMethod(functionCall);
        if (methodInfo == null)
        {
            throw new InvalidFunctionCallException($"Method '{functionCall.Name}' on type '{obj.GetType()}' not found");
        }


        if (!methodInfo.ReturnType.IsAssignableTo(typeof(T)))
        {
            throw new InvalidFunctionCallException($"Method '{functionCall.Name}' on type '{obj.GetType()}' has return type '{methodInfo.ReturnType}' but expected '{typeof(T)}'");
        }

        var parameters = methodInfo.GetParameters().ToList();
        var arguments = functionCall.ParseArguments();
        var args = new List<object?>();

        foreach (var parameter in parameters)
        {
            var parameterDescriptionAttribute = parameter.GetCustomAttribute<ParameterDescriptionAttribute>();

            var name = parameterDescriptionAttribute?.Name ?? parameter.Name!;
            var argument = arguments.FirstOrDefault(x => x.Key == name);

            object? value;
            if (argument.Key == null)
            {
                if (parameter.IsOptional)
                {
                    value = parameter.DefaultValue;
                }
                else
                {
                    throw new($"Argument '{name}' not found");
                }
            }
            else
            {
                value = parameter.ParameterType.IsEnum ? Enum.Parse(parameter.ParameterType, argument.Value.ToString()!) : ((JsonElement)argument.Value).Deserialize(parameter.ParameterType);
            }

            args.Add(value);
        }

        var result = (T?)methodInfo.Invoke(obj, args.ToArray());
        return result;
    }

    private static MethodInfo? GetMethod(this object obj, FunctionCall functionCall)
    {
        var type = obj.GetType();

        // Attempt to find the method directly by name first
        if (functionCall.Name != null)
        {
            var methodByName = type.GetMethod(functionCall.Name);
            if (methodByName != null)
            {
                return methodByName;
            }
        }

        // If not found, then look for methods with the custom attribute
        var methodsWithAttributes = type.GetMethods().FirstOrDefault(m => m.GetCustomAttributes(typeof(FunctionDescriptionAttribute), false).FirstOrDefault() is FunctionDescriptionAttribute attr && attr.Name == functionCall.Name);

        return methodsWithAttributes;
    }

    #region Responses API Support

    /// <summary>
    ///     Creates a <see cref="FunctionTool" /> (Responses API) from the provided method,
    ///     using any <see cref="FunctionDescriptionAttribute" /> and <see cref="ParameterDescriptionAttribute" /> attributes.
    /// </summary>
    /// <param name="methodInfo">The method to create the <see cref="FunctionTool" /> from.</param>
    /// <returns>The <see cref="FunctionTool" /> created.</returns>
    public static FunctionTool GetFunctionTool(MethodInfo methodInfo)
    {
        var funcDef = GetFunctionDefinition(methodInfo);
        return ConvertToFunctionTool(funcDef);
    }

    /// <summary>
    ///     Calls the function on the provided object using a Responses API <see cref="FunctionToolCallItem" />.
    ///     This reuses the same reflection and argument parsing path as chat function calling.
    /// </summary>
    /// <param name="functionToolCall">The function tool call provided by the model.</param>
    /// <param name="obj">The object with the method / function to be executed.</param>
    /// <typeparam name="T">The return type.</typeparam>
    public static T? CallFunction<T>(FunctionToolCallItem functionToolCall, object obj)
    {
        if (functionToolCall == null)
        {
            throw new ArgumentNullException(nameof(functionToolCall));
        }

        return CallFunction<T>(new FunctionCall
        {
            Name = functionToolCall.Name,
            Arguments = functionToolCall.Arguments
        }, obj);
    }

    /// <summary>
    ///     Creates a Responses API <see cref="FunctionCallOutputItemParam" /> from a tool call result.
    /// </summary>
    /// <param name="functionToolCall">The originating function tool call.</param>
    /// <param name="result">The execution result.</param>
    /// <returns>A function call output item for the next Responses request.</returns>
    public static FunctionCallOutputItemParam CreateFunctionCallOutput(FunctionToolCallItem functionToolCall, object? result)
    {
        if (functionToolCall == null)
        {
            throw new ArgumentNullException(nameof(functionToolCall));
        }

        if (string.IsNullOrWhiteSpace(functionToolCall.CallId))
        {
            throw new InvalidFunctionCallException("Function Call Id is null");
        }

        return new FunctionCallOutputItemParam(functionToolCall.CallId, result is string ? result : JsonSerializer.Serialize(result));
    }

    /// <summary>
    ///     Enumerates the methods in the provided object and returns a list of <see cref="ITool" />
    ///     for all methods marked with a <see cref="FunctionDescriptionAttribute" />.
    ///     Use this for the Responses API.
    /// </summary>
    /// <param name="obj">The object to analyze.</param>
    /// <returns>A list of <see cref="ITool" /> (as <see cref="FunctionTool" />).</returns>
    public static List<ITool> GetFunctionTools(object obj)
    {
        var type = obj.GetType();
        return GetFunctionTools(type);
    }

    /// <summary>
    ///     Enumerates the methods in the provided type and returns a list of <see cref="ITool" />
    ///     for all methods marked with a <see cref="FunctionDescriptionAttribute" />.
    ///     Use this for the Responses API.
    /// </summary>
    /// <typeparam name="T">The type to analyze.</typeparam>
    /// <returns>A list of <see cref="ITool" /> (as <see cref="FunctionTool" />).</returns>
    public static List<ITool> GetFunctionTools<T>()
    {
        return GetFunctionTools(typeof(T));
    }

    /// <summary>
    ///     Enumerates the methods in the provided type and returns a list of <see cref="ITool" />
    ///     for all methods marked with a <see cref="FunctionDescriptionAttribute" />.
    ///     Use this for the Responses API.
    /// </summary>
    /// <param name="type">The type to analyze.</param>
    /// <returns>A list of <see cref="ITool" /> (as <see cref="FunctionTool" />).</returns>
    public static List<ITool> GetFunctionTools(Type type)
    {
        var methods = type.GetMethods();

        return methods
            .Select(method => new
            {
                method,
                methodDescriptionAttribute = method.GetCustomAttribute<FunctionDescriptionAttribute>()
            })
            .Where(t => t.methodDescriptionAttribute != null)
            .Select(t => (ITool)GetFunctionTool(t.method))
            .ToList();
    }

    /// <summary>
    ///     Creates a <see cref="FunctionTool" /> from a <see cref="PropertyDefinition" /> schema.
    ///     Use this for type-safe parameter definitions with IntelliSense support.
    /// </summary>
    /// <param name="name">The name of the function.</param>
    /// <param name="description">The description of the function.</param>
    /// <param name="parameters">The parameter schema as <see cref="PropertyDefinition" />.</param>
    /// <param name="strict">Whether to enforce strict parameter validation. Default is true.</param>
    /// <returns>A new <see cref="FunctionTool" /> instance.</returns>
    /// <example>
    ///     <code>
    /// var tool = FunctionCallingHelper.CreateFunctionTool(
    ///     name: "get_weather",
    ///     description: "Get current weather",
    ///     parameters: PropertyDefinition.DefineObject(
    ///         properties: new Dictionary&lt;string, PropertyDefinition&gt;
    ///         {
    ///             ["location"] = PropertyDefinition.DefineString("City name")
    ///         },
    ///         required: new List&lt;string&gt; { "location" },
    ///         additionalProperties: false,
    ///         description: null,
    ///         @enum: null
    ///     )
    /// );
    ///     </code>
    /// </example>
    public static FunctionTool CreateFunctionTool(
        string name,
        string? description,
        PropertyDefinition parameters,
        bool? strict = true)
    {
        return new FunctionTool
        {
            Name = name,
            Description = description,
            Parameters = ConvertToDictionary(parameters, strict),
            Strict = strict
        };
    }

    /// <summary>
    ///     Creates a <see cref="FunctionTool" /> from a raw JSON schema string.
    ///     The JSON is passed through without parsing for zero performance overhead.
    /// </summary>
    /// <param name="name">The name of the function.</param>
    /// <param name="description">The description of the function.</param>
    /// <param name="parametersJson">The raw JSON schema string for parameters.</param>
    /// <param name="strict">Whether to enforce strict parameter validation. Default is true.</param>
    /// <returns>A new <see cref="FunctionTool" /> instance.</returns>
    /// <example>
    ///     <code>
    /// var tool = FunctionCallingHelper.CreateFunctionTool(
    ///     name: "get_weather",
    ///     description: "Get current weather",
    ///     parametersJson: """
    ///         {
    ///             "type": "object",
    ///             "properties": {
    ///                 "location": { "type": "string", "description": "City name" }
    ///             },
    ///             "required": ["location"]
    ///         }
    ///         """
    /// );
    ///     </code>
    /// </example>
    public static FunctionTool CreateFunctionTool(
        string name,
        string? description,
        string parametersJson,
        bool? strict = true)
    {
        return new FunctionTool
        {
            Name = name,
            Description = description,
            Parameters = FunctionParameters.FromJson(parametersJson),
            Strict = strict
        };
    }

    /// <summary>
    ///     Creates a <see cref="FunctionTool" /> from a C# type using <see cref="PropertyDefinitionGenerator" />.
    ///     The schema is auto-generated from the type's properties.
    ///     Works on all .NET versions.
    /// </summary>
    /// <typeparam name="TParams">The type representing the function parameters.</typeparam>
    /// <param name="name">The name of the function.</param>
    /// <param name="description">The description of the function.</param>
    /// <param name="strict">Whether to enforce strict parameter validation. Default is true.</param>
    /// <returns>A new <see cref="FunctionTool" /> instance.</returns>
    /// <example>
    ///     <code>
    /// public class WeatherParams
    /// {
    ///     [JsonPropertyName("location")]
    ///     public string Location { get; set; }
    /// }
    /// 
    /// var tool = FunctionCallingHelper.CreateFunctionToolFromType&lt;WeatherParams&gt;(
    ///     name: "get_weather",
    ///     description: "Get current weather"
    /// );
    ///     </code>
    /// </example>
    public static FunctionTool CreateFunctionToolFromType<TParams>(
        string name,
        string? description = null,
        bool? strict = true)
    {
        return CreateFunctionToolFromType(typeof(TParams), name, description, strict);
    }

    /// <summary>
    ///     Creates a <see cref="FunctionTool" /> from a C# type using <see cref="PropertyDefinitionGenerator" />.
    ///     The schema is auto-generated from the type's properties.
    ///     Works on all .NET versions.
    /// </summary>
    /// <param name="paramsType">The type representing the function parameters.</param>
    /// <param name="name">The name of the function.</param>
    /// <param name="description">The description of the function.</param>
    /// <param name="strict">Whether to enforce strict parameter validation. Default is true.</param>
    /// <returns>A new <see cref="FunctionTool" /> instance.</returns>
    public static FunctionTool CreateFunctionToolFromType(
        Type paramsType,
        string name,
        string? description = null,
        bool? strict = true)
    {
        var schema = PropertyDefinitionGenerator.GenerateFromType(paramsType);
        return CreateFunctionTool(name, description, schema, strict);
    }

#if NET9_0_OR_GREATER
    /// <summary>
    ///     Creates a <see cref="FunctionTool" /> from a C# type using .NET 9's native JsonSchemaExporter.
    ///     This is the cleanest option for .NET 9+ projects.
    /// </summary>
    /// <typeparam name="TParams">The type representing the function parameters.</typeparam>
    /// <param name="name">The name of the function.</param>
    /// <param name="description">The description of the function.</param>
    /// <param name="strict">Whether to enforce strict parameter validation. Default is true.</param>
    /// <param name="options">Optional JsonSerializerOptions for schema generation.</param>
    /// <returns>A new <see cref="FunctionTool" /> instance.</returns>
    /// <example>
    ///     <code>
    /// public class WeatherParams
    /// {
    ///     [Description("City name")]
    ///     [Required]
    ///     public string Location { get; set; }
    /// }
    /// 
    /// var tool = FunctionCallingHelper.CreateFunctionToolFromTypeNet9&lt;WeatherParams&gt;(
    ///     name: "get_weather",
    ///     description: "Get current weather"
    /// );
    ///     </code>
    /// </example>
    public static FunctionTool CreateFunctionToolFromTypeNet9<TParams>(
        string name,
        string? description = null,
        bool? strict = true,
        JsonSerializerOptions? options = null)
    {
        // Clone caller options so schema export cannot freeze or mutate them.
        var schemaOptions = options == null ? new JsonSerializerOptions(JsonSerializerOptions.Default) : new JsonSerializerOptions(options);
        schemaOptions.MakeReadOnly(populateMissingResolver: true);
        var schemaNode = schemaOptions.GetJsonSchemaAsNode(typeof(TParams));
        if (strict == true) NormalizeStrictParameters(schemaNode);
        var schemaJson = schemaNode.ToJsonString();

        return new FunctionTool
        {
            Name = name,
            Description = description,
            Parameters = FunctionParameters.FromJson(schemaJson),
            Strict = strict
        };
    }
#endif

    /// <summary>
    ///     Converts a <see cref="FunctionDefinition" /> (Chat Completions API) to a <see cref="FunctionTool" /> (Responses API).
    /// </summary>
    /// <param name="funcDef">The function definition to convert.</param>
    /// <returns>A new <see cref="FunctionTool" /> instance.</returns>
    public static FunctionTool ConvertToFunctionTool(FunctionDefinition funcDef)
    {
        return new FunctionTool
        {
            Name = funcDef.Name,
            Description = funcDef.Description,
            Parameters = ConvertToDictionary(funcDef.Parameters, funcDef.Strict),
            Strict = funcDef.Strict
        };
    }

    /// <summary>
    ///     Converts a list of <see cref="ToolDefinition" /> (Chat Completions API) to a list of <see cref="ITool" /> (Responses API).
    /// </summary>
    /// <param name="toolDefinitions">The tool definitions to convert.</param>
    /// <returns>A list of <see cref="ITool" />.</returns>
    public static List<ITool> ConvertToFunctionTools(IEnumerable<ToolDefinition> toolDefinitions)
    {
        return toolDefinitions
            .Where(td => td.Function != null)
            .Select(td => (ITool)ConvertToFunctionTool(td.Function!))
            .ToList();
    }

    private static FunctionParameters? ConvertToDictionary(PropertyDefinition? definition, bool? strict = null)
    {
        if (definition == null) return null;
        // Raw FunctionParameters bypass the request serializer. Omit absent schema
        // keywords here: const:null and properties:null are actual constraints,
        // not harmless missing values once embedded in raw JSON.
        var schema = JsonSerializer.SerializeToNode(definition, new JsonSerializerOptions
        {
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
        })!;
        if (strict == true) NormalizeStrictParameters(schema);
        return FunctionParameters.FromJson(schema.ToJsonString());
    }

    private static void NormalizeStrictParameters(JsonNode schema)
    {
        NormalizeStrictSchema(schema);
        // The function argument document itself is an object, even when the CLR
        // reference type exported by System.Text.Json allows a null root value.
        if (schema is JsonObject root && root["type"] is JsonArray types && types.Any(t => t?.ToString() == "object"))
            root["type"] = "object";
    }

    private static void NormalizeStrictSchema(JsonNode? schema)
    {
        if (schema is not JsonObject obj) return;
        var properties = obj["properties"] as JsonObject;
        var isObject = properties != null || obj["type"]?.ToString() == "object" ||
            obj["type"] is JsonArray types && types.Any(t => t?.ToString() == "object");
        if (isObject)
        {
            if (properties == null)
            {
                properties = new JsonObject();
                obj["properties"] = properties;
            }
            obj["additionalProperties"] = false;
            var required = new JsonArray();
            foreach (var property in properties)
            {
                required.Add(property.Key);
                NormalizeStrictSchema(property.Value);
            }
            obj["required"] = required;
        }
        // Visit schema locations, never arbitrary default/const/example values.
        foreach (var key in new[] { "$defs", "definitions" })
            if (obj[key] is JsonObject definitions)
                foreach (var definition in definitions) NormalizeStrictSchema(definition.Value);
        foreach (var key in new[] { "items", "not", "if", "then", "else" }) NormalizeStrictSchema(obj[key]);
        foreach (var key in new[] { "anyOf", "oneOf", "allOf", "prefixItems" })
            if (obj[key] is JsonArray alternatives)
                foreach (var alternative in alternatives) NormalizeStrictSchema(alternative);
    }

    #endregion
}
