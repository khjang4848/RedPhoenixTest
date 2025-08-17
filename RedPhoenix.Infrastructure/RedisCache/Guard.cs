namespace RedPhoenix.Infrastructure.RedisCache;
public static class Guard
{
    public static void GuardAgainstNullOrEmpty(this string value, string parameterName)
    {
        if (string.IsNullOrEmpty(value))
        {
            throw new ArgumentOutOfRangeException(parameterName);
        }
    }

    public static void GuardAgainstNull<T>(this T value, string parameterName)
    {
        GuardAgainstValueType<T>(parameterName);

        if (value == null)
        {
            throw new ArgumentNullException(parameterName);
        }
    }

    private static void GuardAgainstValueType<T>(string parameterName)
    {
        if (typeof(T).IsValueType)
        {
            throw new ArgumentException("parameter should be reference type, not value type", parameterName);
        }
    }
}