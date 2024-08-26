using System.Reflection;
using System.Text.Json.Serialization;
using OpenAI.ObjectModels.SharedModels;

namespace OpenAI.Utilities.FunctionCalling;

public class PropertyDefinitionGenerator
{
    public static PropertyDefinition GenerateFromType(Type type)
    {
        if (type == null)
            throw new ArgumentNullException(nameof(type));

        if (type.IsPrimitive || type == typeof(string) || type == typeof(DateTime))
        {
            return GeneratePrimitiveDefinition(type);
        }
        else if (type.IsEnum)
        {
            return GenerateEnumDefinition(type);
        }
        else if (type.IsArray || (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(List<>)))
        {
            return GenerateArrayDefinition(type);
        }
        else
        {
            return GenerateObjectDefinition(type);
        }
    }

    private static PropertyDefinition GeneratePrimitiveDefinition(Type type)
    {
        if (type == typeof(string))
            return PropertyDefinition.DefineString();
        else if (type == typeof(int) || type == typeof(long))
            return PropertyDefinition.DefineInteger();
        else if (type == typeof(float) || type == typeof(double) || type == typeof(decimal))
            return PropertyDefinition.DefineNumber();
        else if (type == typeof(bool))
            return PropertyDefinition.DefineBoolean();
        else if (type == typeof(DateTime))
            return PropertyDefinition.DefineString("ISO 8601 date-time string");
        else
            throw new ArgumentException($"Unsupported primitive type: {type.Name}");
    }

    private static PropertyDefinition GenerateEnumDefinition(Type type)
    {
        var enumValues = Enum.GetNames(type);
        return PropertyDefinition.DefineEnum(new List<string>(enumValues), $"Enum of type {type.Name}");
    }

    private static PropertyDefinition GenerateArrayDefinition(Type type)
    {
        Type elementType = type.IsArray ? type.GetElementType() : type.GetGenericArguments()[0];
        return PropertyDefinition.DefineArray(GenerateFromType(elementType));
    }

    private static PropertyDefinition GenerateObjectDefinition(Type type)
    {
        var properties = new Dictionary<string, PropertyDefinition>();
        var required = new List<string>();

        foreach (var prop in type.GetProperties(BindingFlags.Public | BindingFlags.Instance))
        {
            string propertyName = GetJsonPropertyName(prop);
            properties[propertyName] = GenerateFromType(prop.PropertyType);

            // You might want to customize this logic based on your needs
            if (!prop.PropertyType.IsValueType && Nullable.GetUnderlyingType(prop.PropertyType) == null)
            {
                required.Add(propertyName);
            }
        }

        return PropertyDefinition.DefineObject(
            properties,
            required,
            false, // Set additionalProperties to false by default
            $"Object of type {type.Name}",
            null
        );
    }

    private static string GetJsonPropertyName(PropertyInfo prop)
    {
        var jsonPropertyNameAttribute = prop.GetCustomAttribute<JsonPropertyNameAttribute>();
        return jsonPropertyNameAttribute != null ? jsonPropertyNameAttribute.Name : prop.Name;
    }
}