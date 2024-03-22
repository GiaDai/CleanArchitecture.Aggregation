using CleanArchitecture.Aggregation.Domain.Entities;
using CleanArchitecture.Aggregation.WebApi.Models;
using MassTransit;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace CleanArchitecture.Aggregation.WebApi.Consumers
{
    public class BookEmailConsumer : IConsumer<Product>
    {
        private readonly ILogger<BookEmailConsumer> _logger;
        public BookEmailConsumer(
            ILogger<BookEmailConsumer> logger
            )
        {
            _logger = logger;
        }
        public async Task Consume(ConsumeContext<Product> context)
        {
            var book = context.Message;
            _logger.LogInformation($"Received book email: {book.Barcode}");
            // Save book to database
            // Send notification to user
        }
    }
}
