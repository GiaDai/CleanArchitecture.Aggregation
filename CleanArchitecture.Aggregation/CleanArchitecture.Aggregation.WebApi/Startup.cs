using AppAny.HotChocolate.FluentValidation;
using CleanArchitecture.Aggregation.Application;
using CleanArchitecture.Aggregation.Application.Interfaces;
using CleanArchitecture.Aggregation.Infrastructure.Identity;
using CleanArchitecture.Aggregation.Infrastructure.Persistence;
using CleanArchitecture.Aggregation.Infrastructure.Shared;
using CleanArchitecture.Aggregation.WebApi.Data;
using CleanArchitecture.Aggregation.WebApi.Extensions;
using CleanArchitecture.Aggregation.WebApi.GraphQL.Mutations;
using CleanArchitecture.Aggregation.WebApi.GraphQL.Queries;
using CleanArchitecture.Aggregation.WebApi.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;

namespace CleanArchitecture.Aggregation.WebApi
{
    public class Startup
    {
        private readonly IWebHostEnvironment _env;
        public IConfiguration _config { get; }
        private readonly ILogger<Startup> _logger;
        public Startup(IConfiguration config, IWebHostEnvironment env)
        {
            _config = config;
            _env = env;
            using var loggerFactory = LoggerFactory.Create(builder =>
            {
                builder.SetMinimumLevel(LogLevel.Information);
                builder.AddConsole();
                builder.AddEventSourceLogger();
            });
            _logger = loggerFactory.CreateLogger<Startup>();
        }
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddEnvironmentVariablesExtension();
            services.AddApplicationLayer();
            services.AddNpgSqlIdentityInfrastructure(_config,_env);
            services.AddNpgSqlPersistenceInfrastructure(_config,_env.IsProduction());
            services.AddSharedInfrastructure(_config);
            services.AddScoped<GamesService>();
            services.AddAutoMapper(typeof(Application.Mappings.GeneralProfile));
            services.AddRedisCacheExtension(_config);
            services.AddElasicSearchExtension(_config);
            services.AddSwaggerExtension();
            //services.AddHostedService<RedisConnectionMonitor>();
            services.AddControllers().AddNewtonsoftJson(o => o.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);
            services.AddApiVersioningExtension();
            services.AddHealthChecks();
            services.AddScoped<IAuthenticatedUserService, AuthenticatedUserService>();
            services.AddGraphQLServer()
                .ModifyRequestOptions(opt => opt.IncludeExceptionDetails = _env.IsDevelopment())
                .AddQueryType<QueryType>()
                .AddMutationType<MutationType>()
                .AddProjections()
                .AddFiltering()
                .AddSorting()
                .AddFluentValidation();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILogger<Startup> logger)
        {
            var redisConn = new ConfigurationOptions
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
            var redis = ConnectionMultiplexer.Connect(redisConn);
            redis.ConnectionFailed += (sender, args) =>
            {
                logger.LogError($"##################################Lost connection to Redis server: {args.Exception.Message}");
                // Handle the connection failure here, such as logging, sending notifications, etc.
            };
            redis.ConnectionRestored += (sender, args) =>
            {
                logger.LogInformation($"Connection to Redis server has been restored.");
                // Handle the connection restoration here, such as logging, sending notifications, etc.
            };
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
                app.UseErrorHandlingMiddleware();
            }
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseSwaggerExtension();
            app.UseHealthChecks("/health");

            app.UseEndpoints(endpoints =>
             {
                 endpoints.MapGraphQL("/graphql");
                 endpoints.MapControllers();
             });
        }
    }
}
