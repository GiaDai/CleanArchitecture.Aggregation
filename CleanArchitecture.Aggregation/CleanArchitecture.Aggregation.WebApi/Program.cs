using CleanArchitecture.Aggregation.Infrastructure.Identity.Contexts;
using CleanArchitecture.Aggregation.Infrastructure.Identity.Models;
using CleanArchitecture.Aggregation.Infrastructure.Persistence.Contexts;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace CleanArchitecture.Aggregation.WebApi
{
    public class Program
    {
        public async static Task Main(string[] args)
        {
            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            var host = CreateHostBuilder(args).Build();
            if (environment == Environments.Production)
            {
                using (var scope = host.Services.CreateScope())
                {
                    var services = scope.ServiceProvider;
                    var logger = services.GetService<ILogger<Program>>();
                    try
                    {
                        var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
                        var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
                        var appContext = services.GetRequiredService<ApplicationDbContext>();
                        var identityContext = services.GetRequiredService<IdentityContext>();
                        appContext.Database.Migrate();
                        identityContext.Database.Migrate();

                        await Infrastructure.Identity.Seeds.DefaultRoles.SeedAsync(userManager, roleManager);
                        await Infrastructure.Identity.Seeds.DefaultSuperAdmin.SeedAsync(userManager, roleManager);
                        await Infrastructure.Identity.Seeds.DefaultBasicUser.SeedAsync(userManager, roleManager);
                        logger.LogInformation("Finished Seeding Default Data");
                        logger.LogInformation("Application Starting");
                    }
                    catch (Exception ex)
                    {
                        logger.LogError(ex, "An error occurred seeding the DB");
                    }
                }
            }
            host.Run();
        }
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
