using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace Simple.Application.WriteModel
{
    public class RegisterProductCommandHandler : IRequestHandler<RegisterProductCommand, Guid>
    {
        private readonly IProductRepository _productRepository;

        public RegisterProductCommandHandler(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<Guid> Handle(RegisterProductCommand command, CancellationToken cancellationToken)
        {
            var product = new Product(command.Name, command.Code);

            await _productRepository.Save(product, cancellationToken);

            return product.Id;
        }
    }
}