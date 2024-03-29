﻿using CleanArchitecture.Aggregation.Application.Interfaces;
using CleanArchitecture.Aggregation.Application.Interfaces.Repositories;
using CleanArchitecture.Aggregation.Infrastructure.Persistence.Contexts;
using CleanArchitecture.Aggregation.Infrastructure.Persistence.Repositories;
using CleanArchitecture.Aggregation.Infrastructure.Persistence.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using System;
using System.Collections.Generic;
using System.Text;

namespace CleanArchitecture.Aggregation.Infrastructure.Persistence
{
    public static class ServiceRegistration
    {
        public static void AddPersistenceInfrastructure(this IServiceCollection services, IConfiguration configuration, bool isProduction)
        {
            var serverVersion = new MySqlServerVersion(new Version(5, 7, 35));
            string appConnStr = configuration.GetConnectionString("DefaultConnection");
            if (isProduction)
            {
                appConnStr = Environment.GetEnvironmentVariable("DB_URI_MASTER_APP");
            }
            services.AddDbContext<ApplicationDbContext>(options =>
            options.UseMySql(
                appConnStr, serverVersion,
                b =>
                {
                    b.SchemaBehavior(MySqlSchemaBehavior.Ignore);
                    b.EnableRetryOnFailure(
                        maxRetryCount: 5,
                        maxRetryDelay: TimeSpan.FromSeconds(30),
                        errorNumbersToAdd: null);
                    b.MigrationsAssembly(isProduction ? typeof(ApplicationDbContext).Assembly.FullName : "CleanArchitecture.Aggregation.WebApi");
                    b.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
                }));

            #region Repositories
            services.AddTransient(typeof(IGenericRepositoryAsync<>), typeof(GenericRepositoryAsync<>));
            services.AddTransient<IProductRepositoryAsync, ProductRepositoryAsync>();
            #endregion
        }
    }
}
