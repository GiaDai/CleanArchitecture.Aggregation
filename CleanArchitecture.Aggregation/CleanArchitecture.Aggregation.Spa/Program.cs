using CleanArchitecture.Aggregation.Application;
using CleanArchitecture.Aggregation.Application.Interfaces;
using CleanArchitecture.Aggregation.Infrastructure.Identity;
using CleanArchitecture.Aggregation.Infrastructure.Persistence;
using CleanArchitecture.Aggregation.Infrastructure.Shared;
using CleanArchitecture.Aggregation.Spa.Extensions;
using CleanArchitecture.Aggregation.Spa.GraphQL.Mutations;
using CleanArchitecture.Aggregation.Spa.GraphQL.Queries;
using CleanArchitecture.Aggregation.Spa.Services;
using CleanArchitecture.Aggregation.WebApi.Extensions;

var builder = WebApplication.CreateBuilder(args);
IWebHostEnvironment _env = builder.Environment;
IConfiguration _config = builder.Configuration;
IServiceCollection services = builder.Services;
// Add services to the container.
services.AddApplicationLayer();
services.AddMySqlIdentityInfrastructure(_config, _env);
services.AddMySqlPersistenceInfrastructure(_config, _env.IsProduction());
services.AddSharedInfrastructure(_config);
services.AddControllers();
services.AddApiVersioningExtension();
services.AddHealthChecks();
services.AddScoped<IAuthenticatedUserService, AuthenticatedUserService>();
services.AddGraphQLServer()
    .AddQueryType<QueryType>()
    .AddMutationType<MutationType>()
    .AddFiltering()
    .AddSorting();
builder.Services.AddControllersWithViews();

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
app.UseErrorHandlingMiddleware();
app.UseHealthChecks("/health");
app.MapControllerRoute(
        name: "default",
        pattern: "{controller}/{action=Index}/{id?}"
    );
app.MapFallbackToFile("index.html");

app.Run();
