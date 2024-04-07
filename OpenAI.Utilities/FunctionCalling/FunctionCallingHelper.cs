using System.Reflection;
using System.Text.Json;
using OpenAI.Builders;
using OpenAI.ObjectModels.RequestModels;
using OpenAI.ObjectModels.SharedModels;

namespace OpenAI.Utilities.FunctionCalling;

/// <summary>
///     Helper methods for Function Calling
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

        var result = new FunctionDefinitionBuilder(
            methodDescriptionAttribute?.Name ?? methodInfo.Name, methodDescriptionAttribute?.Description);

        var parameters = methodInfo.GetParameters().ToList();

        foreach (var parameter in parameters)
        {
            var parameterDescriptionAttribute = parameter.GetCustomAttribute<ParameterDescriptionAttribute>();
            var description = parameterDescriptionAttribute?.Description;

            PropertyDefinition definition;

            switch (parameter.ParameterType, parameterDescriptionAttribute?.Type == null)
            {
                case (_, false):
                    definition = new PropertyDefinition
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

                    var enumValues = string.IsNullOrEmpty(parameterDescriptionAttribute?.Enum)
                        ? Enum.GetNames(parameter.ParameterType).ToList()
                        : parameterDescriptionAttribute.Enum.Split(',').Select(x => x.Trim()).ToList();

                    definition =
                        PropertyDefinition.DefineEnum(enumValues, description);

                    break;
                default:
                    throw new Exception($"Parameter type '{parameter.ParameterType}' not supported");
            }

            result.AddParameter(
                parameterDescriptionAttribute?.Name ?? parameter.Name!,
                definition,
                parameterDescriptionAttribute?.Required ?? true);
        }

        return result.Build();
    }

    public static ToolDefinition GetToolDefinition(MethodInfo methodInfo)
    {
        return new ToolDefinition()
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

        var result = methods
            .Select(method => new
            {
                method,
                methodDescriptionAttribute = method.GetCustomAttribute<FunctionDescriptionAttribute>()
            })
            .Where(t => t.methodDescriptionAttribute != null)
            .Select(t => GetToolDefinition(t.method)).ToList();

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
            throw new InvalidFunctionCallException(
                $"Method '{functionCall.Name}' on type '{obj.GetType()}' has return type '{methodInfo.ReturnType}' but expected '{typeof(T)}'");
        }

        var parameters = methodInfo.GetParameters().ToList();
        var arguments = functionCall.ParseArguments();
        var args = new List<object?>();

        foreach (var parameter in parameters)
        {
            var parameterDescriptionAttribute =
                parameter.GetCustomAttribute<ParameterDescriptionAttribute>();

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
                    throw new Exception($"Argument '{name}' not found");
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
        var methodsWithAttributes = type
            .GetMethods()
            .FirstOrDefault(m => m.GetCustomAttributes(typeof(FunctionDescriptionAttribute), false).FirstOrDefault() is FunctionDescriptionAttribute attr && attr.Name == functionCall.Name);

        return methodsWithAttributes;
    }

}
