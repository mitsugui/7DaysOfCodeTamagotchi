public static class Extensions
{
    public static string ToPascalCase(this string value)
    {
        if (string.IsNullOrEmpty(value))
            return value;

        return char.ToUpper(value[0]) + value[1..].ToLower();
    }
}