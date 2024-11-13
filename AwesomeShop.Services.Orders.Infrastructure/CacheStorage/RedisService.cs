
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace AwesomeShop.Services.Orders.Infrastructure.CacheStorage;

public class RedisService(IDistributedCache redis) : ICacheService {
    private readonly IDistributedCache _redis = redis;

    public async Task<T?> GetAsync<T>(string cacheKey) {
        string? objectString = await this._redis.GetStringAsync(cacheKey);

        if (string.IsNullOrWhiteSpace(objectString)) {
            Console.WriteLine($"Cache key '{cacheKey}' not found");
            return default;
        }

        Console.WriteLine($"Cache key found for key '{cacheKey}'");

        return JsonSerializer.Deserialize<T>(objectString);
    }

    public async Task SetAsync<T>(string cacheKey, T value) {
        DistributedCacheEntryOptions memoryCacheEntryOption = new() {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(3600), // Daqui 3600, ele expirará
            SlidingExpiration = TimeSpan.FromSeconds(1200) // Se 20min sem ser acessado, ele expirará
        };

        string objectString = JsonSerializer.Serialize(value);

        Console.WriteLine($"Cache set for key '{cacheKey}'");

        await this._redis.SetStringAsync(cacheKey, objectString, memoryCacheEntryOption);
    }
}