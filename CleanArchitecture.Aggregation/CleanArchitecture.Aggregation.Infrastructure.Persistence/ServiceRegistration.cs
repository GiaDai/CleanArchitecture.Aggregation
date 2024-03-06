using CleanArchitecture.Aggregation.Application.Interfaces;
using CleanArchitecture.Aggregation.Application.Interfaces.Repositories;
using CleanArchitecture.Aggregation.Infrastructure.Persistence.Contexts;
using CleanArchitecture.Aggregation.Infrastructure.Persistence.Repositories;
using CleanArchitecture.Aggregation.Infrastructure.Persistence.Repository;
using CleanArchitecture.Aggregation.Infrastructure.Shared.Environments;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using System;

namespace CleanArchitecture.Aggregation.Infrastructure.Persistence
{
    public static class ServiceRegistration
    {
        public static void AddPersistenceInfrastructure(this IServiceCollection services, IConfiguration configuration, bool isProduction)
        {
            // Build the intermediate service provider
            var sp = services.BuildServiceProvider();
            using (var scope = sp.CreateScope())
            {
                var _dbSetting = scope.ServiceProvider.GetRequiredService<IDatabaseSettingsProvider>();
                string appConnStr = _dbSetting.GetMySQLConnectionString();
                if (!string.IsNullOrWhiteSpace(appConnStr))
                {
                    var serverVersion = new MySqlServerVersion(new Version(5, 7, 35));
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
                }
            }
            #region Repositories
            services.AddTransient(typeof(IGenericRepositoryAsync<>), typeof(GenericRepositoryAsync<>));
            services.AddTransient<IProductRepositoryAsync, ProductRepositoryAsync>();
            #endregion
        }
    }
}
