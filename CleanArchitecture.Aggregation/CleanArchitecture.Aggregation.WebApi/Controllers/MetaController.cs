using CleanArchitecture.Aggregation.Application.Interfaces.Repositories.RedisCache;
using CleanArchitecture.Aggregation.Domain.Entities;
using CleanArchitecture.Aggregation.Infrastructure.Shared.Environments;
using CleanArchitecture.Aggregation.WebApi.Models;
using MassTransit;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Exceptions;
using System;
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
            return Ok(postgre);
        }

        // Response RabbitMQ status is healthy or not
        [HttpGet("/rabbitmq")]
        public async Task<ActionResult<string>> RabbitMQ()
        {
            var product = new Product
            {
                Barcode = Guid.NewGuid().ToString(),
                Description = "Test Product",
                Name = "Test Product",
                Rate = 100
            };
            // add redis and rabbitmq in task whenall and return the result
           var result = await Task.WhenAll(
                _productRedisCache.AddAsync(product.Barcode, product, TimeSpan.FromDays(1)),
                SendMessageToQueueAsync(product, "bookQueue")
            );
            var timeSpan = await _productRedisCache.CheckRedisAvailability();
            return Ok(result[1] ? $"RabbitMQ is healthy: {timeSpan.TotalMilliseconds}" : "RabbitMQ is not healthy");
            //return Ok("RabbitMQ is healthy");
            //await _productRedisCache.AddAsync(product.Barcode, product, TimeSpan.FromDays(1));
            //var isSendMessage = await SendMessageToQueueAsync(product, "bookQueue");
            //return Ok(isSendMessage ? "RabbitMQ is healthy" : "RabbitMQ is not healthy");
        }

        private async Task<bool> SendMessageToQueueAsync(Product product, string queueName)
        {
            bool isRabbitMQHealthy = _rabbitMqSettingProdiver.IsHealthy();
            if (!isRabbitMQHealthy)
            {
                Console.WriteLine("RabbitMQ service is not healthy. Skipping message sending.");
                return false;
            }

            try
            {
                await _rabbitMqSettingProdiver.GetUri(_bus, queueName, product);
                return true;
                //using (var connection = _connectionFactory.CreateConnection())
                //using (var channel = connection.CreateModel())
                //{
                //    channel.QueueDeclare(queue: queueName,
                //                         durable: false,
                //                         exclusive: false,
                //                         autoDelete: false,
                //                         arguments: null);

                //    var body = System.Text.Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(book));

                //    channel.BasicPublish(exchange: "",
                //                         routingKey: queueName,
                //                         basicProperties: null,
                //                         body: body);

                //    return true;
                //}
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error sending message to queue: {ex.Message}");
                return false;
            }
        }
    }
}
