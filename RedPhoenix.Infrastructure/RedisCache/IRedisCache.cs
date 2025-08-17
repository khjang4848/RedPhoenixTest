namespace RedPhoenix.Infrastructure.RedisCache;

public interface IRedisCache : IDisposable
{
    T? Get<T>(string key);

    void Set<T>(string key, T value);
    long Increment(string key, long value);
    int DbIndex { set; }
}