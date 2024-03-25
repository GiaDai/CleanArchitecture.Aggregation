using CleanArchitecture.Aggregation.WorkerThree;
using CleanArchitecture.Aggregation.WorkerThree.Extensions;


IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext, services) =>
    {
        IConfiguration _config = hostContext.Configuration;
        //services.AddEnvironmentVariablesExtension();
        //services.AddDependencyInjectionExtension();
        //services.AddRedisCacheExtension(_config);
        //services.AddRabbitMQExtension(_config);
        services.AddHostedService<Worker>();
    })
    .Build();

host.Run();
