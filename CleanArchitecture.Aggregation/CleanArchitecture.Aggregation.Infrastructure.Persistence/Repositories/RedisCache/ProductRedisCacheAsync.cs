using CleanArchitecture.Aggregation.Application.Interfaces.Repositories.RedisCache;
using CleanArchitecture.Aggregation.Domain.Entities;
using StackExchange.Redis.Extensions.Core.Abstractions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CleanArchitecture.Aggregation.Infrastructure.Persistence.Repositories.RedisCache
{
    public class ProductRedisCacheAsync : IProductRedisCacheAsync
    {
        private readonly IRedisClient _redisCacheClient;
        private readonly IRedisDatabase _redisDatabase;
        private string _prefix = "PRODUCT_";
        public ProductRedisCacheAsync(
            IRedisClient redisCacheClient
            )
        {
            _redisCacheClient = redisCacheClient;
            _redisDatabase = _redisCacheClient.GetDefaultDatabase();
        }

        public async Task<bool> IsUniqueBarcodeAsync(string barcode)
        {
            // Check if the barcode is unique
            return !await _redisDatabase.ExistsAsync(barcode);
        }

        public async Task<bool> AddAsync(string key, Product value, TimeSpan expiry)
        {
            return await _redisDatabase.AddAsync($"{_prefix}{key}", value, expiry);
        }

        public async Task<bool> RemoveAsync(string key)
        {
            return await _redisDatabase.RemoveAsync(key);
        }

        public async Task RemoveAllAsync()
        {
            // Remove all products from cache where the key is the barcode
            var productsKey = await _redisDatabase.SearchKeysAsync($"{_prefix}*");
            foreach (var key in productsKey)
            {
                await _redisDatabase.RemoveAsync(key);
            }
        }

        public async Task AddRangeAsync(List<Product> value, TimeSpan expiry)
        {
            foreach (var product in value)
            {
                await _redisDatabase.AddAsync($"{_prefix}{product.Barcode}", product, expiry);
            }
        }

        public async Task<TimeSpan> CheckRedisAvailability()
        {
            try
            {
                return await _redisCacheClient.GetDefaultDatabase().Database.PingAsync();
            }
            catch (Exception ex)
            {
                // Xử lý khi có lỗi kết nối tới Redis (ví dụ: log lỗi, thông báo, ...)
                Console.WriteLine($"Error checking Redis availability: {ex.Message}");
                return TimeSpan.Zero;
            }
        }
    }
}
