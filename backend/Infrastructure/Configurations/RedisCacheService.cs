using System.Text.Json;
using Domain.Interfaces;
using StackExchange.Redis;

namespace Infrastructure.Configurations;

public class RedisCacheService : ICacheService
{
    private readonly IConnectionMultiplexer _redis;
    public RedisCacheService(IConnectionMultiplexer redis)
    {
        _redis = redis;
    }

    public async Task<T?> GetAsync<T>(string key)
    {
        var db = _redis.GetDatabase();
        var value = await db.StringGetAsync(key);
        if (value.IsNullOrEmpty)
            return default;

        return JsonSerializer.Deserialize<T>(value.ToString()!);
    }

    public async Task SetAsync<T>(string key, T value, TimeSpan expiration)
    {
        var db = _redis.GetDatabase();
        var json = JsonSerializer.Serialize(value);
        await db.StringSetAsync(key, json, expiration);
    }
}