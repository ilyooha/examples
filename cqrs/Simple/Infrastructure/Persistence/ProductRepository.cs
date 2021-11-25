using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Simple.Application.WriteModel;

namespace Simple.Infrastructure.Persistence
{
    public class ProductRepository : IProductRepository
    {
        private readonly AppDbContext _dbContext;

        public ProductRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<Product?> Get(Guid id, CancellationToken cancellationToken = new())
        {
            var product = await _dbContext.Products
                .FirstOrDefaultAsync(x => x.Id.Equals(id), cancellationToken);

            return product is null
                ? null
                : new Product(product.Id, product.Name, product.Code);
        }

        public async Task Save(Product product, CancellationToken cancellationToken = new())
        {
            var existingProduct = await _dbContext.Products
                .FirstOrDefaultAsync(x => x.Id.Equals(product.Id), cancellationToken);

            if (existingProduct is null)
            {
                await _dbContext.AddAsync(new EfProduct
                {
                    Id = product.Id,
                    Name = product.Name,
                    Code = product.Code
                }, cancellationToken);
            }
            else
            {
                existingProduct.Name = product.Name;
                existingProduct.Code = product.Code;
            }
        }
    }
}