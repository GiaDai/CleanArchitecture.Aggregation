using CleanArchitecture.Aggregation.Domain.Entities;
using MassTransit;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace CleanArchitecture.Aggregation.WebApi.Consumers
{
    public class ProductConsumer : IConsumer<Product>
    {
        private readonly ILogger<ProductConsumer> _logger;
        public ProductConsumer(
            ILogger<ProductConsumer> logger
            )
        {
            _logger = logger;
        }
        public async Task Consume(ConsumeContext<Product> context)
        {
            var book = context.Message;
            _logger.LogInformation($"Received book seller: {book.Barcode}");
            // Save book to database
            // Send notification to user
        }
    }
}
