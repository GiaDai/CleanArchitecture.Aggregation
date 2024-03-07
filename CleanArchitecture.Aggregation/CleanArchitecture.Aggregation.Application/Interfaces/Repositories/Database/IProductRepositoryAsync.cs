using CleanArchitecture.Aggregation.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Aggregation.Application.Interfaces.Repositories.Database
{
    public interface IProductRepositoryAsync : IGenericRepositoryAsync<Product>
    {
        // Add a range of products
        Task<int> AddRangeAsync(IEnumerable<Product> products);
        // Check if the barcode is unique
        Task<bool> IsUniqueBarcodeAsync(string barcode);

        // Compute average rate of all products
        Task<double> ComputeAverageRateAsync();

        // Delete all products
        Task<int> DeleteAllAsync();

        // Search products by name
        Task<IEnumerable<Product>> SearchByNameAsync(string name);
    }
}
