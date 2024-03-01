using AppAny.HotChocolate.FluentValidation;
using AutoMapper;
using CleanArchitecture.Aggregation.Application;
using CleanArchitecture.Aggregation.Application.Interfaces;
using CleanArchitecture.Aggregation.Application.Services;
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
            services.AddApplicationLayer();
            services.AddIdentityInfrastructure(_config,_env);
            services.AddPersistenceInfrastructure(_config,_env.IsProduction());
            services.AddSharedInfrastructure(_config);
            services.AddScoped<GamesService>();
            services.AddAutoMapper(typeof(Application.Mappings.GeneralProfile));
            services.AddRedisCacheExtension(_config);
            services.AddSwaggerExtension();
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

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
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
