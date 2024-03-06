using CleanArchitecture.Aggregation.Application.Interfaces.Repositories;
using CleanArchitecture.Aggregation.Application.Wrappers;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace CleanArchitecture.Aggregation.Application.Features.Products.Commands.DeleteProductAll
{
    public class DeleteProductAllCommand : IRequest<Response<int>>
    {

    }

    public class DeleteProductAllCommandHandler : IRequestHandler<DeleteProductAllCommand, Response<int>>
    {
        private readonly IProductRepositoryAsync _productRepository;
        public DeleteProductAllCommandHandler(IProductRepositoryAsync productRepository)
        {
            _productRepository = productRepository;
        }
        public async Task<Response<int>> Handle(DeleteProductAllCommand request, CancellationToken cancellationToken)
        {
            await _productRepository.DeleteAllAsync();
            return new Response<int>(1);
        }
    }
}
