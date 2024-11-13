namespace AwesomeShop.Services.Orders.Infrastructure.CacheStorage;

public interface ICacheService {
    Task<T?> GetAsync<T>(string cacheKey);
    Task SetAsync<T>(string cacheKey, T value);
}