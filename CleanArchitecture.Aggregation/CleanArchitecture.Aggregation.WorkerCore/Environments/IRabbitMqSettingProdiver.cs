using RabbitMQ.Client;

namespace CleanArchitecture.Aggregation.WorkerCore.Environments
{
    public interface IRabbitMqSettingProdiver
    {
        string GetHostName();
        string GetUserName();
        string GetPassword();
        string GetVHost();
        string GetPort();
        string GetConnectionString();
        bool IsHealthy();
        ConnectionFactory GetConnectionFactory();
    }
}
