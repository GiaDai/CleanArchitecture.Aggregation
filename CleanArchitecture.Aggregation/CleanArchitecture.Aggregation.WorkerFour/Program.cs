using CleanArchitecture.Aggregation.WorkerFour;
using CleanArchitecture.Aggregation.WorkerFour.Extensions;

var builder = Host.CreateApplicationBuilder(args);
var _services = builder.Services;
IConfiguration _config = builder.Configuration;
_services.AddEnvironmentVariablesExtension();
_services.AddRabbitMQExtension(_config);

_services.AddHostedService<Worker>();

var host = builder.Build();
host.Run();
