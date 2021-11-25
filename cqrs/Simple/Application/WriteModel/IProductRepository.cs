using System;
using System.Threading;
using System.Threading.Tasks;

namespace Simple.Application.WriteModel
{
    public interface IProductRepository
    {
        Task<Product?> Get(Guid id, CancellationToken cancellationToken = new());
        Task Save(Product product, CancellationToken cancellationToken = new());
    }
}