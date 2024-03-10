using CleanArchitecture.Aggregation.Application;
using CleanArchitecture.Aggregation.Application.Interfaces;
using CleanArchitecture.Aggregation.Infrastructure.Identity;
using CleanArchitecture.Aggregation.Infrastructure.Persistence;
using CleanArchitecture.Aggregation.Infrastructure.Shared;
using CleanArchitecture.Aggregation.WebApp.Extensions;
using CleanArchitecture.Aggregation.Application.Mappings;
using CleanArchitecture.Aggregation.WebApp.Services;
using CleanArchitecture.Aggregation.WebApp.Hubs;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var _config = builder.Configuration;
var _env = builder.Environment;
// Add services to the container.
services.AddEnvironmentVariablesExtension();
services.AddDependencyInjectionExtension();
services.AddApplicationLayer();
services.AddNpgSqlIdentityInfrastructure(_config, _env);
services.AddNpgSqlPersistenceInfrastructure(_config, _env.IsProduction());
services.AddSharedInfrastructure(_config);
services.AddAutoMapper(typeof(GeneralProfile));
services.AddRedisCacheExtension(_config);
services.AddElasicSearchExtension(_config);
services.AddSwaggerExtension();
//services.AddHostedService<RedisConnectionMonitor>();
//services.AddControllers().AddNewtonsoftJson(o => o.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);
services.AddApiVersioningExtension();
services.AddHealthChecks();
services.AddScoped<IAuthenticatedUserService, AuthenticatedUserService>();

services.AddControllersWithViews();

// config signalR
services.AddSignalR();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseSwaggerExtension();
app.UseHealthChecks("/health");

app.UseCors(builder =>
        builder.AllowAnyMethod()
        .AllowAnyHeader()
        .SetIsOriginAllowed(origin => true) // allow any origin
        .AllowCredentials()); // allow credentials

app.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action=Index}/{id?}");

app.MapFallbackToFile("index.html");
app.MapHub<SignalrHub>("/hub");
app.MapHub<JoinChat>("/chat");
app.Run();
