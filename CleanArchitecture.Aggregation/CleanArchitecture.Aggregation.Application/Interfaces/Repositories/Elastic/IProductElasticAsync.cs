using CleanArchitecture.Aggregation.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Aggregation.Application.Interfaces.Repositories.Elastic
{
    public interface IProductElasticAsync
    {
        // Add new product to elastic
        Task<bool> AddProductAsync<T>(T product, string indexName) where T : class;
        // Add range of products to elastic
        Task AddRangeAsync<T>(List<T> products, string indexName) where T : class;

        // Remove product from elastic
        Task<bool> RemoveProductAsync(string id, string indexName);

        // Remove all products from elastic
        Task<bool> RemoveAllProductsAsync(string indexName);

        // Serach for products in elastic by name
        Task<IReadOnlyCollection<Product>> SearchByName(string name, string indexName);
    }
}
