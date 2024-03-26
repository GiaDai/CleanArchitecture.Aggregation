using CleanArchitecture.Aggregation.Infrastructure.Shared.Environments;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CleanArchitecture.Aggregation.WebApi.Hosted
{
    public class ProductConsumerHosted : BackgroundService
    {
        private readonly IServiceProvider _service;
        private readonly string _queueName = "queue";
        public ProductConsumerHosted(
            IServiceProvider service
            )
        {
            _service = service;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            Console.WriteLine("##############ProductConsumerHosted");
            using (var scope = _service.CreateScope())
            {
                var _rabbitMqSettingProdiver = scope.ServiceProvider.GetRequiredService<IRabbitMqSettingProdiver>();
                ConnectionFactory _connectionFactory = new ConnectionFactory { Uri = new Uri(_rabbitMqSettingProdiver.GetConnectionString()) };
                using (var connection = _connectionFactory.CreateConnection())
                using (var channel = connection.CreateModel())
                {
                    channel.QueueDeclare(queue: _queueName,

                                         durable: true,
                                         exclusive: false,
                                         autoDelete: false,
                    arguments: null);

                    var consumer = new EventingBasicConsumer(channel);
                    consumer.Received += (model, ea) =>
                    {
                        try
                        {
                            var body = ea.Body.ToArray();
                            var message = Encoding.UTF8.GetString(body);
                            Console.WriteLine($"[x] Received '{message}'");

                            // Xử lý message ở đây

                            // Xác nhận (ack) message đã được xử lý thành công
                            channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Error consuming message: {ex.Message}");
                        }
                        
                    };

                    channel.BasicConsume(queue: _queueName,
                                  autoAck: false, // Tắt auto acknowledgment
                                  consumer: consumer);

                    //channel.Close();
                    //connection.Close();
                    await Task.CompletedTask;
                }
            }
        }
    }
}
