using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace CleanArchitecture.Aggregation.Test.TestProvider
{
    public class TestAsyncQueryProvider<TEntity> : IAsyncQueryProvider
    {
        private readonly IQueryProvider _inner;

        internal TestAsyncQueryProvider(IQueryProvider inner)
        {
            _inner = inner;
        }

        public IQueryable CreateQuery(Expression expression)
        {
            return new TestAsyncEnumerable<TEntity>(expression);
        }

        public IQueryable<TElement> CreateQuery<TElement>(Expression expression)
        {
            return new TestAsyncEnumerable<TElement>(expression);
        }

        public object Execute(Expression expression)
        {
            var result = _inner.Execute(expression);
            return result ?? throw new InvalidOperationException("The query returned null");
        }

        public TResult Execute<TResult>(Expression expression)
        {
            var result = _inner.Execute<TResult>(expression);
            return result ?? 
                throw new InvalidOperationException("The query returned null");
        }

        public IAsyncEnumerable<TResult> ExecuteAsync<TResult>(Expression expression)
        {
            return new TestAsyncEnumerable<TResult>(expression);
        }

        public Task<TResult> ExecuteAsync<TResult>(Expression expression, System.Threading.CancellationToken cancellationToken)
        {
            return Task.FromResult(Execute<TResult>(expression));
        }

        TResult IAsyncQueryProvider.ExecuteAsync<TResult>(Expression expression, CancellationToken cancellationToken)
        {
            return new ValueTask<TResult>(Execute<TResult>(expression)).Result;
        }
    }
}
