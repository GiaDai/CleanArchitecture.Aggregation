using CleanArchitecture.Aggregation.Application.Interfaces;
using CleanArchitecture.Aggregation.Application.Wrappers;
using CleanArchitecture.Aggregation.Domain.Settings;
using CleanArchitecture.Aggregation.Infrastructure.Identity.Contexts;
using CleanArchitecture.Aggregation.Infrastructure.Identity.Models;
using CleanArchitecture.Aggregation.Infrastructure.Identity.Services;
using CleanArchitecture.Aggregation.Infrastructure.Shared.Environments;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using System;
using System.Text;

namespace CleanArchitecture.Aggregation.Infrastructure.Identity
{
    public static class ServiceExtensions
    {
        public static void AddMySqlIdentityInfrastructure(this IServiceCollection services, IConfiguration configuration, IWebHostEnvironment env)
        {
            // Build the intermediate service provider
            var sp = services.BuildServiceProvider();
            using (var scope = sp.CreateScope())
            {
                var _dbSetting = scope.ServiceProvider.GetRequiredService<IDatabaseSettingsProvider>();
                string identityConStr = _dbSetting.GetMySQLConnectionString();
                if (!string.IsNullOrWhiteSpace(identityConStr))
                {

                    var serverVersion = new MySqlServerVersion(new Version(5, 7, 35));
                    services.AddDbContext<IdentityContext>(options =>
                    options.UseMySql(
                        identityConStr, serverVersion,
                        b =>
                        {
                            b.SchemaBehavior(MySqlSchemaBehavior.Ignore);
                            b.EnableRetryOnFailure(
                                maxRetryCount: 5,
                                maxRetryDelay: TimeSpan.FromSeconds(30),
                                errorNumbersToAdd: null);
                            b.MigrationsAssembly(env.IsProduction() ? typeof(IdentityContext).Assembly.FullName : "CleanArchitecture.Aggregation.WebApi");
                        }));
                }
                services.AddIdentity<ApplicationUser, IdentityRole>().AddEntityFrameworkStores<IdentityContext>().AddDefaultTokenProviders();
                #region Services
                services.AddTransient<IAccountService, AccountService>();
                #endregion
                services.Configure<JWTSettings>(configuration.GetSection("JWTSettings"));
                services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                    .AddJwtBearer(o =>
                    {
                        o.RequireHttpsMetadata = false;
                        o.SaveToken = false;
                        o.TokenValidationParameters = new TokenValidationParameters
                        {
                            ValidateIssuerSigningKey = true,
                            ValidateIssuer = true,
                            ValidateAudience = true,
                            ValidateLifetime = true,
                            ClockSkew = TimeSpan.Zero,
                            ValidIssuer = configuration["JWTSettings:Issuer"],
                            ValidAudience = configuration["JWTSettings:Audience"],
                            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWTSettings:Key"]))
                        };
                        o.Events = new JwtBearerEvents()
                        {
                            OnAuthenticationFailed = c =>
                            {
                                c.NoResult();
                                c.Response.StatusCode = 500;
                                c.Response.ContentType = "text/plain";
                                return c.Response.WriteAsync(c.Exception.ToString());
                            },
                            OnChallenge = context =>
                            {
                                context.HandleResponse();
                                context.Response.StatusCode = 401;
                                context.Response.ContentType = "application/json";
                                var result = JsonConvert.SerializeObject(new Response<string>("You are not Authorized"));
                                return context.Response.WriteAsync(result);
                            },
                            OnForbidden = context =>
                            {
                                context.Response.StatusCode = 403;
                                context.Response.ContentType = "application/json";
                                var result = JsonConvert.SerializeObject(new Response<string>("You are not authorized to access this resource"));
                                return context.Response.WriteAsync(result);
                            },
                        };
                    });
            }
        }

        // Add persistence infrastructure for NpgSql
        public static void AddNpgSqlIdentityInfrastructure(this IServiceCollection services, IConfiguration configuration, IWebHostEnvironment env)
        {
            // Build the intermediate service provider
            var sp = services.BuildServiceProvider();
            using (var scope = sp.CreateScope())
            {
                var _dbSetting = scope.ServiceProvider.GetRequiredService<IDatabaseSettingsProvider>();
                string identityConStr = _dbSetting.GetPostgresConnectionString();
                if (!string.IsNullOrWhiteSpace(identityConStr))
                {
                    services.AddDbContext<IdentityContext>(options =>
                    options.UseNpgsql( identityConStr,
                        b =>
                        {
                            b.MigrationsAssembly(env.IsProduction() ? typeof(IdentityContext).Assembly.FullName : "CleanArchitecture.Aggregation.WebApi");
                        }));
                }
                services.AddIdentity<ApplicationUser, IdentityRole>().AddEntityFrameworkStores<IdentityContext>().AddDefaultTokenProviders();
                #region Services
                services.AddTransient<IAccountService, AccountService>();
                #endregion
                services.Configure<JWTSettings>(configuration.GetSection("JWTSettings"));
                services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                    .AddJwtBearer(o =>
                    {
                        o.RequireHttpsMetadata = false;
                        o.SaveToken = false;
                        o.TokenValidationParameters = new TokenValidationParameters
                        {
                            ValidateIssuerSigningKey = true,
                            ValidateIssuer = true,
                            ValidateAudience = true,
                            ValidateLifetime = true,
                            ClockSkew = TimeSpan.Zero,
                            ValidIssuer = configuration["JWTSettings:Issuer"],
                            ValidAudience = configuration["JWTSettings:Audience"],
                            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWTSettings:Key"]))
                        };
                        o.Events = new JwtBearerEvents()
                        {
                            OnAuthenticationFailed = c =>
                            {
                                c.NoResult();
                                c.Response.StatusCode = 500;
                                c.Response.ContentType = "text/plain";
                                return c.Response.WriteAsync(c.Exception.ToString());
                            },
                            OnChallenge = context =>
                            {
                                context.HandleResponse();
                                context.Response.StatusCode = 401;
                                context.Response.ContentType = "application/json";
                                var result = JsonConvert.SerializeObject(new Response<string>("You are not Authorized"));
                                return context.Response.WriteAsync(result);
                            },
                            OnForbidden = context =>
                            {
                                context.Response.StatusCode = 403;
                                context.Response.ContentType = "application/json";
                                var result = JsonConvert.SerializeObject(new Response<string>("You are not authorized to access this resource"));
                                return context.Response.WriteAsync(result);
                            },
                        };
                    });
            }
        }
    }
}
