using CleanArchitecture.Aggregation.Domain.Entities;
using MassTransit;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace CleanArchitecture.Aggregation.WebApi.Consumers
{
    public class BookSellerConsumer : IConsumer<Product>
    {
        private readonly ILogger<BookSellerConsumer> _logger;
        public BookSellerConsumer(
            ILogger<BookSellerConsumer> logger
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
