using CleanArchitecture.Aggregation.Application.Interfaces.Repositories.Elastic;
using CleanArchitecture.Aggregation.Application.Interfaces.Repositories.RedisCache;
using CleanArchitecture.Aggregation.Infrastructure.Persistence.Repositories.Elastic;
using CleanArchitecture.Aggregation.Infrastructure.Persistence.Repositories.RedisCache;
using CleanArchitecture.Aggregation.Infrastructure.Shared.Environments;
using CleanArchitecture.Aggregation.WorkerTwo.Consumers;
using GreenPipes;
using MassTransit;
using StackExchange.Redis.Extensions.Newtonsoft;

namespace CleanArchitecture.Aggregation.WorkerTwo.Extensions
{
    public static class ServiceExtensions
    {
        // Add rabbitmq extension
        public static void AddRabbitMQExtension(this IServiceCollection services, IConfiguration config)
        {
            // Build the intermediate service provider
            var sp = services.BuildServiceProvider();
            using (var scope = sp.CreateScope())
            {
                var _rabbitMqSettings = scope.ServiceProvider.GetRequiredService<IRabbitMqSettingProdiver>();
                var hostName = _rabbitMqSettings.GetHostName();
                var userName = _rabbitMqSettings.GetUserName();
                var password = _rabbitMqSettings.GetPassword();
                var vHost = _rabbitMqSettings.GetVHost();

                services.AddMassTransit(x =>
                {
                    x.AddConsumer<ProductConsumer>();
                    x.UsingRabbitMq((context, cfg) =>
                    {
                        cfg.Host(hostName, vHost, h =>
                        {
                            h.Username(userName);
                            h.Password(password);
                        });

                        cfg.ReceiveEndpoint("productQueue", e =>
                        {
                            e.PrefetchCount = 1;
                            //e.UseMessageRetry(r => r.Interval(2, 100));
                            e.UseMessageRetry(r => r.Interval(2, 100));
                            e.Consumer<ProductConsumer>(context);

                        });
                    });
                });
                services.AddMassTransitHostedService();
            }
        }

        public static void AddRedisCacheExtension(this IServiceCollection services, IConfiguration configuration)
        {
            // Build the intermediate service provider
            var sp = services.BuildServiceProvider();
            using (var scope = sp.CreateScope())
            {
                var _redisSettings = scope.ServiceProvider.GetRequiredService<IRedisSettingsProvider>();
                var _redisConfig = _redisSettings.GetRedisConfiguration();
                _redisConfig.ConnectTimeout = 5000; // 5 seconds
                _redisConfig.ConnectRetry = 3; // 3 times
                _redisConfig.AbortOnConnectFail = false; // do not abort
                _redisConfig.CertificateValidation += (sender, certificate, chain, sslPolicyErrors) => true; // ignore certificate errors

                services.AddStackExchangeRedisExtensions<NewtonsoftSerializer>(_redisConfig);
            }
        }
        public static void AddEnvironmentVariablesExtension(this IServiceCollection services)
        {
            services.AddTransient<IDatabaseSettingsProvider, DatabaseSettingsProvider>();
            services.AddTransient<IRedisSettingsProvider, RedisSettingsProvider>();
            services.AddTransient<IElasticSettingsProvider, ElasticSettingsProvider>();
            services.AddTransient<IRabbitMqSettingProdiver, RabbitMqSettingProdiver>();
        }

        public static void AddDependencyInjectionExtension(this IServiceCollection services)
        {
            services.AddTransient<IProductRedisCacheAsync, ProductRedisCacheAsync>();
            //services.AddTransient<IProductElasticAsync, ProductElasticAsync>();
        }
    }
}
