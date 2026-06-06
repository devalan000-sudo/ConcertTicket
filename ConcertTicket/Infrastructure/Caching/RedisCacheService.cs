using ConcertTicket.Application.Interfaces;
using StackExchange.Redis;
using System.Text.Json;

namespace ConcertTicket.Infrastructure.Caching
{
    public class RedisCacheService : ICacheService
    {
        private readonly IDatabase _db;

        public RedisCacheService(IConnectionMultiplexer redis)
        {
            _db = redis.GetDatabase();
        }

        public async Task<T?> GetAsync<T>(string key)
        {
            var data = await _db.StringGetAsync(key);

            if (data.IsNullOrEmpty)
                return default;

            return JsonSerializer.Deserialize<T>(data!);
        }

        public async Task SetAsync<T>(string key, T value, TimeSpan? expiration = null)
        {
            var jsonData = JsonSerializer.Serialize(value);
            if (expiration.HasValue)
                await _db.StringSetAsync(key, jsonData, expiration.Value);
            else
                await _db.StringSetAsync(key, jsonData);
        }

        public async Task RemoveAsync(string key)
        {
            await _db.KeyDeleteAsync(key);
        }

        public async Task<long> DecrementAsync(string key)
        {
            return await _db.StringDecrementAsync(key);
        }

        public async Task<long> IncrementAsync(string key)
        {
            return await _db.StringIncrementAsync(key);
        }

        public async Task<long> DecrementAsync(string key, long amount = 1)
        {
            return await _db.StringDecrementAsync(key, amount);
        }

        public async Task<long> IncrementAsync(string key, long amount = 1)
        {
            return await _db.StringIncrementAsync(key, amount);
        }
    }
}
