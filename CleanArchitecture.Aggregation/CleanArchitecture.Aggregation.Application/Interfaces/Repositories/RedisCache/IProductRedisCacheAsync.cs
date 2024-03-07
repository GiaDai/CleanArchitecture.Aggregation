using CleanArchitecture.Aggregation.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CleanArchitecture.Aggregation.Application.Interfaces.Repositories.RedisCache
{
    public interface IProductRedisCacheAsync
    {
        // Check if the barcode is unique
        Task<bool> IsUniqueBarcodeAsync(string barcode);
        // Add a product to cache
        Task<bool> AddAsync(string key, Product value, TimeSpan expiry);
        // Add range of products to cache
        Task AddRangeAsync(List<Product> value, TimeSpan expiry);
        // Remove a product from cache
        Task<bool> RemoveAsync(string key);
        // Remove all products from cache
        Task RemoveAllAsync();
    }
}
