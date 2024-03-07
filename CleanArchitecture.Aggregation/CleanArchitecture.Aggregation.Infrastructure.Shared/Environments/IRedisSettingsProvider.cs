using StackExchange.Redis.Extensions.Core.Configuration;

namespace CleanArchitecture.Aggregation.Infrastructure.Shared.Environments
{
    public interface IRedisSettingsProvider
    {
        RedisConfiguration GetRedisConfiguration();
    }
}
