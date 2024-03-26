using CleanArchitecture.Aggregation.WorkerApi.Extensions;

var builder = WebApplication.CreateBuilder(args);
var _services = builder.Services;
var _config = builder.Configuration;
var _env = builder.Environment;
// Add services to the container.

builder.Services.AddControllers();
_services.AddEnvironmentVariablesExtension();
_services.AddDependencyInjectionExtension();
_services.AddRedisCacheExtension(_config);
_services.AddRabbitMQExtension(_config);
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
