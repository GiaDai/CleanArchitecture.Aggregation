using CleanArchitecture.Aggregation.WorkerCore.Environments;
using RabbitMQ.Client.Core.DependencyInjection;
using RabbitMQ.Client.Core.DependencyInjection.Configuration;

namespace CleanArchitecture.Aggregation.WorkerFour.Extensions
{
    public static class ServiceExtensions
    {
        public static void AddRabbitMQExtension(this IServiceCollection _services, IConfiguration _config)
        {
            var sp = _services.BuildServiceProvider();
            using (var scope = sp.CreateScope())
            {
                var _rabbitMqSettings = scope.ServiceProvider.GetRequiredService<IRabbitMqSettingProdiver>();
                var hostName = _rabbitMqSettings.GetHostName();
                var userName = _rabbitMqSettings.GetUserName();
                var password = _rabbitMqSettings.GetPassword();
                var port = _rabbitMqSettings.GetPort();
                var vHost = _rabbitMqSettings.GetVHost();

                var rabbitMqConfig = new RabbitMqServiceOptions()
                {
                    HostName = hostName ?? "",
                    Port = int.Parse(port ?? "0"),
                    UserName = userName ?? "",
                    Password = password ?? "",
                    VirtualHost = vHost ?? ""
                };
                var exchangeOptions = new RabbitMqExchangeOptions
                {
                    Type = "direct",
                    Durable = true,
                    AutoDelete = true,
                    Arguments = null,
                    RequeueFailedMessages = true,
                    DeadLetterExchange = "default.dlx.exchange",
                    Queues = new List<RabbitMqQueueOptions>
                    {
                        new RabbitMqQueueOptions
                        {
                            Name = "directProductionQueue",
                            Durable = true,
                            AutoDelete = false,
                            Exclusive = false,
                            Arguments = null,
                            RoutingKeys = new HashSet<string> { "routing.key" }
                        }
                    }
                };
                _services
                    .AddRabbitMqServices(rabbitMqConfig)
                    //.AddConsumptionExchange("DirectExchange",exchangeOptions)
                    .AddProductionExchange("DirectProductionExchange", exchangeOptions);
            }
        }

        public static void AddEnvironmentVariablesExtension(this IServiceCollection services)
        {
            services.AddTransient<IRabbitMqSettingProdiver, RabbitMqSettingProdiver>();
        }
    }
}
