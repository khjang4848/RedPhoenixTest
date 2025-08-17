namespace RedPhoenix.Infrastructure.RedisCache;

using Serialization;
using StackExchange.Redis;
using System.Runtime.CompilerServices;


public class RedisCache(ITextSerializer serializer) : IRedisCache
{
    private readonly ITextSerializer _serializer =
        serializer ?? throw new ArgumentNullException(nameof(serializer));

    private static readonly Lazy<ConnectionMultiplexer> _redis = new(() =>
    {
        var cacheConnection = Environment.GetEnvironmentVariable(
            "ConnectionStrings:RedisConnecntionString",
            EnvironmentVariableTarget.Process);

        return ConnectionMultiplexer.Connect(cacheConnection);
    });


    public void Dispose()
    {
    }


    public T? Get<T>(string key)
    {
        key.GuardAgainstNullOrEmpty(nameof(key));

        var database = _redis.Value.GetDatabase(DbIndex);
        var value = string.Empty;

        try
        {
            value = database.StringGet(key);
        }
        catch (Exception e)
        {
            OnCachingFailed(e);
        }

        if (value != null) return (T)_serializer.Deserialize(value)!;

        return default;

    }

    public void Set<T>(string key, T value)
    {
        key.GuardAgainstNullOrEmpty(nameof(key));

        var database = _redis.Value.GetDatabase(DbIndex);

        try
        {
            database.StringSet(key, _serializer.Serialize(value));
        }
        catch (Exception e)
        {
            OnCachingFailed(e);
        }

    }

    public long Increment(string key, long value)
    {
        key.GuardAgainstNullOrEmpty(nameof(key));

        var database = _redis.Value.GetDatabase(DbIndex);

        try
        {
            return database.StringIncrement(key, value);
        }
        catch (Exception e)
        {
            OnCachingFailed(e);
        }

        return 0;
    }

    public int DbIndex { get; set; } = 0;

    private void OnCachingFailed(Exception e, [CallerMemberName] string memberName = "")
    {
        throw new Exception("Redis | Caching failed for " + memberName + e);
    }
}