using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace CleanArchitecture.Aggregation.WebApi.Hosted
{
    public class RedisConnectionMonitor : BackgroundService
    {
        private readonly ILogger<RedisConnectionMonitor> _logger;
        private readonly ConfigurationOptions _configurationOptions;
        private readonly TimeSpan _checkInterval;
        public RedisConnectionMonitor(IConfiguration _config, ILogger<RedisConnectionMonitor> logger)
        {
            _logger = logger;
            _configurationOptions = new ConfigurationOptions
            {
                
                EndPoints = {
                    "redis-17098.c292.ap-southeast-1-1.ec2.cloud.redislabs.com:17098",
                }, // Replace with your Redis server endpoint
                Password = "VVgSUFS48Ih6JApUA1lHHBB9HUGtFGQH",
                ClientName = "default",
                ConnectTimeout = 5000, // Set connection timeout to 5 seconds
                ConnectRetry = 3, // Set number of retries on connection failure to 3
                AbortOnConnectFail = false // Disable automatic abort on connection failure
            };
            _checkInterval = TimeSpan.FromSeconds(10); // Check connection status every 10 seconds
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var redis = await ConnectionMultiplexer.ConnectAsync(_configurationOptions);

                    // Wait for a short interval before checking connection status again
                    await Task.Delay(_checkInterval, stoppingToken);

                    if (!redis.IsConnected)
                    {
                        _logger.LogError("Lost connection to Redis server.");
                        // Perform actions such as sending notifications, etc.
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Failed to connect to Redis server: {ex.Message}");
                }
            }
        }
    }
}
