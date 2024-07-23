namespace  Trip.Advisor.Be.Core.Extentions;

public static class ObjectExtensions
{
    public static bool IsNullOrEmpty<T>(this IEnumerable<T> source)
    {
        return source?.Any() != true;
    }

    public static bool IsNullObject<T>(this T value)
    {
        if (typeof(T) == typeof(string))
            return string.IsNullOrEmpty(value as string);

        return value == null || value.Equals(default(T));
    }
}
