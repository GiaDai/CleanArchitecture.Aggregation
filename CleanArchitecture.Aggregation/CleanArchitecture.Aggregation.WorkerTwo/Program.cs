using CleanArchitecture.Aggregation.WorkerTwo;
using CleanArchitecture.Aggregation.WorkerTwo.Extensions;

var builder = Host.CreateApplicationBuilder(args);
var _services = builder.Services;
var _config = builder.Configuration;
_services.AddDependencyInjectionExtension();
_services.AddEnvironmentVariablesExtension();
_services.AddRedisCacheExtension(_config);
_services.AddRabbitMQExtension(_config);
builder.Services.AddHostedService<Worker>();

var host = builder.Build();
host.Run();
