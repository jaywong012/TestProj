using System.ComponentModel;

namespace Infrastructure.Utilities;

public static class EnumExtensions
{
    public static string GetDescription(this Enum value)
    {
        var fieldInfo = value.GetType().GetField(value.ToString());
        if (fieldInfo == null) return value.ToString();

        var attribute = (DescriptionAttribute)fieldInfo
            .GetCustomAttributes(typeof(DescriptionAttribute), false)
            .FirstOrDefault()!;

        return attribute?.Description ?? value.ToString();
    }

    public static T ToEnum<T>(this string value, bool ignoreCase = true) where T : struct, Enum
    {
        if (Enum.TryParse(value, ignoreCase, out T result))
        {
            return result;
        }

        throw new ArgumentException($"Invalid enum value: '{value}' for enum type {typeof(T).Name}.");
    }
}