using CleanArchitecture.Aggregation.Application.Interfaces.Repositories.Elastic;
using CleanArchitecture.Aggregation.Application.Interfaces.Repositories.RedisCache;
using CleanArchitecture.Aggregation.Infrastructure.Persistence.Repositories.Elastic;
using CleanArchitecture.Aggregation.Infrastructure.Persistence.Repositories.RedisCache;
using CleanArchitecture.Aggregation.Infrastructure.Shared.Environments;
using CleanArchitecture.Aggregation.WebApi.Consumers;
using CleanArchitecture.Aggregation.WebApi.Services;
using Elastic.Clients.Elasticsearch;
using Elastic.Transport;
using GreenPipes;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using StackExchange.Redis.Extensions.Newtonsoft;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace CleanArchitecture.Aggregation.WebApi.Extensions
{
    public static class ServiceExtensions
    {
        public static void AddElasicSearchExtension(this IServiceCollection services, IConfiguration _config)
        {
            // Build the intermediate service provider
            var sp = services.BuildServiceProvider();
            using (var scope = sp.CreateScope())
            {
                var _elasticSettings = scope.ServiceProvider.GetRequiredService<IElasticSettingsProvider>();
                var cloudId = _elasticSettings.GetCloudId();
                var apiKey = _elasticSettings.GetApiKey();
                var client = new ElasticsearchClient(cloudId, new ApiKey(apiKey));
                services.AddSingleton(client);
            }
        }
        public static void AddSwaggerExtension(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                // c.IncludeXmlComments(string.Format(@"{0}\CleanArchitecture.Aggregation.WebApi.xml", System.AppDomain.CurrentDomain.BaseDirectory));
                c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory,$"{Assembly.GetExecutingAssembly().GetName().Name}.xml"));
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Clean Architecture - CleanArchitecture.Aggregation.WebApi",
                    Description = "This Api will be responsible for overall data distribution and authorization.",
                    Contact = new OpenApiContact
                    {
                        Name = "codewithmukesh",
                        Email = "hello@codewithmukesh.com",
                        Url = new Uri("https://codewithmukesh.com/contact"),
                    }
                });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    Description = "Input your Bearer token in this format - Bearer {your token here} to access this API",
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer",
                            },
                            Scheme = "Bearer",
                            Name = "Bearer",
                            In = ParameterLocation.Header,
                        }, new List<string>()
                    },
                });
            });
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
                    x.UsingRabbitMq((context, cfg) =>
                    {
                        cfg.Host(hostName, vHost, h =>
                        {
                            h.Username(userName);
                            h.Password(password);
                        });

                        cfg.UseRetry(retryConfig =>
                        {
                            retryConfig.Interval(5, TimeSpan.FromSeconds(5));
                            // Cấu hình retry policy theo ý muốn
                        });
                    });
                });
                services.AddMassTransitHostedService();
            }
        }

        public static void AddApiVersioningExtension(this IServiceCollection services)
        {
            services.AddApiVersioning(config =>
            {
                // Specify the default API Version as 1.0
                config.DefaultApiVersion = new ApiVersion(1, 0);
                // If the client hasn't specified the API version in the request, use the default API version number 
                config.AssumeDefaultVersionWhenUnspecified = true;
                // Advertise the API versions supported for the particular endpoint
                config.ReportApiVersions = true;
            });
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
            services.AddTransient<IProductElasticAsync, ProductElasticAsync>();
        }
    }
}
