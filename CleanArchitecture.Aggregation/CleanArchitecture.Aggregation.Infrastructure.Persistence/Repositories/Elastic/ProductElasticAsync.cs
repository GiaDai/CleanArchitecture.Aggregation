using CleanArchitecture.Aggregation.Application.Interfaces.Repositories.Elastic;
using CleanArchitecture.Aggregation.Domain.Entities;
using Elastic.Clients.Elasticsearch;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CleanArchitecture.Aggregation.Infrastructure.Persistence.Repositories.Elastic
{
    public class ProductElasticAsync : IProductElasticAsync
    {
        private readonly ElasticsearchClient _client;
        public ProductElasticAsync(
            ElasticsearchClient client
            )
        {
            _client = client;
        }

        public async Task<bool> AddProductAsync<T>(T product, string indexName) where T : class
        {
            var response = await _client.IndexAsync(product, indexName);
            return response.IsValidResponse;
        }

        public async Task<bool> RemoveProductAsync(string id, string indexName)
        {
            var response = await _client.DeleteByQueryAsync<Product>("product", d => d
                .Query(q => q
                    // where id is equal to id
                    .Term(t => t.Field(f => f.Id).Value(id))
                )
            );
            return response.IsValidResponse;
        }

        public async Task<bool> RemoveAllProductsAsync(string indexName)
        {
            var response = await _client.DeleteByQueryAsync<Product>("product", d => d
                           .Query(q => q
                            //where id is not empty
                            .Exists(e => e.Field(f => f.Id))
                        )
                    );
            return response.IsValidResponse;
        }

        public async Task<IReadOnlyCollection<Product>> SearchByName(string name, string indexName)
        {
            var response = await _client.SearchAsync<Product>(s => s
                .Index(indexName)
                .Query(q => q
                    // where title contains title
                    .Match(m => m.Field(f => f.Name).Query(name))
                )
            );

            if (response.IsValidResponse)
            {
                return response.Documents;
            }

            return null;
        }
    }
}
