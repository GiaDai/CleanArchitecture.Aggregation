using CleanArchitecture.Aggregation.WorkerOne;
using CleanArchitecture.Aggregation.WorkerOne.Extensions;

var builder = Host.CreateApplicationBuilder(args);
var _services = builder.Services;
var _config = builder.Configuration;

_services.AddEnvironmentVariablesExtension();
_services.AddDependencyInjectionExtension();
_services.AddRedisCacheExtension(_config);
_services.AddRabbitMQExtension(_config);
_services.AddHostedService<Worker>();

var host = builder.Build();
host.Run();
