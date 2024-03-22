using CleanArchitecture.Aggregation.Application.Globals;
using MassTransit;
using System;
using System.Threading.Tasks;

namespace CleanArchitecture.Aggregation.WebApi.Services
{
    public class RabbitMqReceiveEndpointObserver : IReceiveEndpointObserver
    {
        public Task Ready(ReceiveEndpointReady ready)
        {
            Console.WriteLine($"Endpoint {ready.InputAddress} ready");
            ConnectionGlobal.IsRabbitMqConnection = true;
            return Task.CompletedTask;
        }

        public Task Stopping(ReceiveEndpointStopping stopping)
        {
            Console.WriteLine($"Endpoint {stopping.InputAddress} stopping");
            return Task.CompletedTask;
        }

        public Task Completed(ReceiveEndpointCompleted completed)
        {
            Console.WriteLine($"Endpoint {completed.InputAddress} completed");
            return Task.CompletedTask;
        }

        public Task Faulted(ReceiveEndpointFaulted faulted)
        {
            // Xử lý khi xảy ra lỗi kết nối
            Console.WriteLine($"Endpoint {faulted.InputAddress} faulted: {faulted.Exception.Message}");
            ConnectionGlobal.IsRabbitMqConnection = false;
            return Task.CompletedTask;
        }
    }
}
