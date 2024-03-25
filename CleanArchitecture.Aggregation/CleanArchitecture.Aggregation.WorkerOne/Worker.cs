namespace CleanArchitecture.Aggregation.WorkerOne
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IHostApplicationLifetime _lifetime;
        public Worker(ILogger<Worker> logger, IHostApplicationLifetime lifetime)
        {
            _logger = logger;
            _lifetime = lifetime;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                if (_logger.IsEnabled(LogLevel.Information))
                {
                    _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                }
                await Task.Delay(1000, stoppingToken);
            }
            //await Task.CompletedTask;
            //_logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
            //_lifetime.StopApplication();
        }
    }
}
