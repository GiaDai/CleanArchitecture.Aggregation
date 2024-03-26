using CleanArchitecture.Aggregation.Application.Globals;
using CleanArchitecture.Aggregation.Application.Interfaces.Repositories.RedisCache;
using CleanArchitecture.Aggregation.Domain.Entities;
using CleanArchitecture.Aggregation.Infrastructure.Shared.Environments;
using MassTransit;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using RabbitMQ.Client;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace CleanArchitecture.Aggregation.WebApi.Controllers
{
    [ApiExplorerSettings(IgnoreApi = false)]
    public class MetaController : BaseApiController
    {
        private readonly IWebHostEnvironment _env;
        private readonly IConfiguration _config;
        private readonly IBusControl _bus;
        private readonly ConnectionFactory _connectionFactory;
        private readonly IDatabaseSettingsProvider _databaseSettingsProvider;
        private readonly IRabbitMqSettingProdiver _rabbitMqSettingProdiver;
        private readonly IProductRedisCacheAsync _productRedisCache;
        public MetaController(
            IWebHostEnvironment env,
            IConfiguration config,
            IBusControl bus,
            IDatabaseSettingsProvider databaseSettingsProvider,
            IRabbitMqSettingProdiver rabbitMqSettingProdiver,
            IProductRedisCacheAsync productRedisCache
            )
        {
            _env = env;
            _bus = bus;
            _config = config;
            _databaseSettingsProvider = databaseSettingsProvider;
            _rabbitMqSettingProdiver = rabbitMqSettingProdiver;
            _productRedisCache = productRedisCache;
            _connectionFactory = new ConnectionFactory { Uri = new Uri(_rabbitMqSettingProdiver.GetConnectionString()) };
        }

        [HttpGet("/info")]
        public ActionResult<string> Info()
        {
            var assembly = typeof(Startup).Assembly;

            var lastUpdate = System.IO.File.GetLastWriteTime(assembly.Location);
            var version = FileVersionInfo.GetVersionInfo(assembly.Location).ProductVersion;

            return Ok($"Version: {version}, Last Updated: {lastUpdate}");
        }

        // Response Evniroment is production or development
        [HttpGet("/env")]
        public ActionResult<string> Env()
        {
            var postgre = _databaseSettingsProvider.GetMySQLConnectionString();

            return Ok($"Connection rabbitmq is: {ConnectionGlobal.IsRabbitMqConnection}");
        }

        // Response RabbitMQ status is healthy or not
        [HttpGet("/rabbitmq")]
        public async Task<ActionResult<string>> RabbitMQ()
        {
            var productOne = new Product
            {
                Barcode = Guid.NewGuid().ToString(),
                Description = "Test Product",
                Name = "Test Product",
                Rate = 100
            };
            var productTwo = new Product
            {
                Barcode = Guid.NewGuid().ToString(),
                Description = "Test Product",
                Name = "Test Product",
                Rate = 100
            };
            var productThree = new Product
            {
                Barcode = Guid.NewGuid().ToString(),
                Description = "Test Product",
                Name = "Test Product",
                Rate = 100
            };
            var productFour = new Product
            {
                Barcode = Guid.NewGuid().ToString(),
                Description = "Test Product",
                Name = "Test Product",
                Rate = 100
            };
            var productFive = new Product
            {
                Barcode = Guid.NewGuid().ToString(),
                Description = "Test Product",
                Name = "Test Product",
                Rate = 100
            };
            // add redis and rabbitmq in task whenall and return the result
            //var result = await Task.WhenAll(
            //    //_productRedisCache.AddAsync(product.Barcode, product, TimeSpan.FromDays(1)),
            //    SendMessageToQueueAsync(productOne, "bookQueue"),
            //    SendMessageToQueueAsync(productTwo, "bookQueue"),
            //    SendMessageToQueueAsync(productThree, "bookQueue"),
            //    SendMessageToQueueAsync(productFour, "bookQueue"),
            //    SendMessageToQueueAsync(productFive, "bookQueue")
            //);
            //Console.WriteLine($"Product One: {productOne.Barcode}");
            //Console.WriteLine($"Product Two: {productTwo.Barcode}");
            //Console.WriteLine($"Product Three: {productThree.Barcode}");
            //Console.WriteLine($"Product Four: {productFour.Barcode}");
            //Console.WriteLine($"Product Five: {productFive.Barcode}");
            await SendMessageToQueueAsync(productOne, "queue");
            // wait for the product to be added to the cache
            // var product = await WaitForProductReponse(productOne.Barcode);
            return Ok();

        }

        private async Task<Product> WaitForProductReponse(string productId)
        {
            while (true)
            {
                var product = await _productRedisCache.FindAsync(productId);
                if (product != null)
                {
                    // remove the product from cache
                    await _productRedisCache.RemoveAsync(productId);
                    return product;
                }
                await Task.Delay(1000);
            }
        }

        private async Task<bool> SendMessageToQueueAsync(Product product, string queueName)
        {
            //bool isRabbitMQHealthy = _rabbitMqSettingProdiver.IsHealthy();
            if (!ConnectionGlobal.IsRabbitMqConnection)
            {
                Console.WriteLine("RabbitMQ service is not healthy. Skipping message sending.");
                return false;
            }

            try
            {
                //await _rabbitMqSettingProdiver.GetUri(_bus, queueName, product);
                //return true;
                using (var connection = _connectionFactory.CreateConnection())
                using (var channel = connection.CreateModel())
                {
                    channel.QueueDeclare(queue: queueName,
                                         
                                         durable: true,
                                         exclusive: false,
                                         autoDelete: false,
                                         arguments: new Dictionary<string, object> {
                                             { "x-message-ttl", "5000" } // TTL cho message (milliseconds)
                                         });

                    var body = System.Text.Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(product));

                    channel.BasicPublish(exchange: "",
                                         routingKey: queueName,
                                         basicProperties: null,
                                         body: body);

                    return true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error sending message to queue: {ex.Message}");
                return false;
            }
        }
    }
}
