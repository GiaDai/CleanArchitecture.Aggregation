using CleanArchitecture.Aggregation.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Aggregation.Application.Interfaces.Repositories
{
    public interface IProductRepositoryAsync : IGenericRepositoryAsync<Product>
    {
        // Check if the barcode is unique
        Task<bool> IsUniqueBarcodeAsync(string barcode);

        // Compute average rate of all products
        Task<double> ComputeAverageRateAsync();
    }
}
