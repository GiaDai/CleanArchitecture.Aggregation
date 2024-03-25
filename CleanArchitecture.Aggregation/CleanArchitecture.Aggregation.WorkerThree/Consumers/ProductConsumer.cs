using CleanArchitecture.Aggregation.Application.Interfaces.Repositories.RedisCache;
using CleanArchitecture.Aggregation.Domain.Entities;
using MassTransit;

namespace CleanArchitecture.Aggregation.WorkerThree.Consumers
{
    public class ProductConsumer : IConsumer<Product>
    {
        private readonly ILogger<ProductConsumer> _logger;
        private readonly IProductRedisCacheAsync _productRedisCacheAsync;
        public ProductConsumer(
            ILogger<ProductConsumer> logger,
            IProductRedisCacheAsync productRedisCacheAsync
            )
        {
            _logger = logger;
            _productRedisCacheAsync = productRedisCacheAsync;
        }
        public async Task Consume(ConsumeContext<Product> context)
        {
            var book = context.Message;
            _logger.LogInformation($"Logging by Worker One: {book.Barcode}");
            // Add the product to the cache
            book.Description = "This is a book which have updated by WorkerOne";
            // delay for 1 second
            //await Task.Delay(1000);
            await _productRedisCacheAsync.AddAsync(book.Barcode, book, TimeSpan.FromMinutes(5));
        }
    }
}
