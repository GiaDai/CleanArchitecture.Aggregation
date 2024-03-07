using CleanArchitecture.Aggregation.Application.Interfaces.Repositories.Database;
using CleanArchitecture.Aggregation.Domain.Entities;
using CleanArchitecture.Aggregation.Infrastructure.Persistence.Contexts;
using CleanArchitecture.Aggregation.Infrastructure.Persistence.Repository;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CleanArchitecture.Aggregation.Infrastructure.Persistence.Repositories
{
    public class ProductRepositoryAsync : GenericRepositoryAsync<Product>, IProductRepositoryAsync
    {
        private readonly DbSet<Product> _products;
        private readonly ApplicationDbContext _dbContext;
        public ProductRepositoryAsync(ApplicationDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
            _products = dbContext.Set<Product>();
        }

        public async Task<bool> IsUniqueBarcodeAsync(string barcode)
        {
            return await _products
                .AllAsync(p => p.Barcode != barcode);
        }

        public async Task<double> ComputeAverageRateAsync()
        {
            return (double)await _products
                .AverageAsync(p => p.Rate);
        }

        public async Task<int> AddRangeAsync(IEnumerable<Product> products)
        {
            await _dbContext.Products.AddRangeAsync(products);
            await _dbContext.SaveChangesAsync();
            return products.Count();
        }

        public async Task<int> DeleteAllAsync()
        {
            // Write linq to delete all products
            _dbContext.Products.RemoveRange(_products);
            await _dbContext.SaveChangesAsync();
            return 1;
        }

        public async Task<IEnumerable<Product>> SearchByNameAsync(string name)
        {
            return await _products
                .Where(p => p.Name.Contains(name))
                .ToListAsync();
        }
    }
}
