using System.Text.Json;

namespace TamagotchiApp.Shared.Utils;

public static class Extensions
{
	private static readonly JsonSerializerOptions _serializerOptions = new() { WriteIndented = false };

    public static string ToPascalCase(this string value)
    {
        if (string.IsNullOrEmpty(value))
            return value;

        return char.ToUpper(value[0]) + value[1..].ToLower();
    }

    public static string ToJson<T>(this T value)
    {
        return JsonSerializer.Serialize(value, _serializerOptions);
    }
}