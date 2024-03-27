using RabbitMQ.Client.Core.DependencyInjection.Services;
using RabbitMQ.Client.Core.DependencyInjection.Services.Interfaces;

namespace CleanArchitecture.Aggregation.WorkerFour
{
    public class Worker : BackgroundService
    {
        private readonly IProducingService _producingService;
        private readonly IConsumingService _consumingService;
        private readonly ILogger<Worker> _logger;

        public Worker(
            ILogger<Worker> logger,
            IProducingService producingService,
            IConsumingService consumingService
            )
        {
            _logger = logger;
            _consumingService = consumingService;
            _producingService = producingService;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            int i = 0;
            while (!stoppingToken.IsCancellationRequested)
            {
                if (_logger.IsEnabled(LogLevel.Information))
                {
                    _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                }
                var message = new Message
                {
                    Name = "Custom message",
                    Flag = true,
                    Index = i,
                    Numbers = new[] { 1, 2, 3 }
                };
                await _producingService.SendAsync(message, "DirectProductionExchange", "routing.key");
                i++;
                await Task.Delay(1000, stoppingToken);
            }
        }
    }

    public class Message
    {
        public string Name { get; set; }

        public bool Flag { get; set; }

        public int Index { get; set; }

        public IEnumerable<int> Numbers { get; set; }
    }
}
