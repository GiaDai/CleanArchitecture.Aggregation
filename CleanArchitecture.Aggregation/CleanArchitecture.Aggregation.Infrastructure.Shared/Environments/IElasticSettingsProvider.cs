namespace CleanArchitecture.Aggregation.Infrastructure.Shared.Environments
{
    public interface IElasticSettingsProvider
    {
        string GetCloudId();
        string GetApiKey();
    }
}
