using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Simple.Application.ReadModel;

namespace Simple.Infrastructure.Persistence
{
    public class ProductQueryHandler : IRequestHandler<GetProductsQuery, IEnumerable<IProduct>>,
        IRequestHandler<GetProductByIdQuery, IProduct?>
    {
        private class ProductDto : IProduct
        {
            public Guid Id { get; set; }
            public string Title { get; set; } = "";
        }

        private readonly AppDbContext _appDbContext;

        public ProductQueryHandler(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext ?? throw new ArgumentNullException(nameof(appDbContext));
        }

        public async Task<IEnumerable<IProduct>> Handle(GetProductsQuery request, CancellationToken cancellationToken)
        {
            var items = await _appDbContext.Products
                .AsNoTracking()
                .ToListAsync(cancellationToken);

            return items.Select(Map);
        }

        public async Task<IProduct?> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
        {
            var product = await _appDbContext.Products
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id.Equals(request.Id), cancellationToken);

            return product == null ? null : Map(product);
        }

        private static IProduct Map(EfProduct efProduct)
        {
            return new ProductDto
            {
                Id = efProduct.Id,
                Title = $"[{efProduct.Code}] {efProduct.Name}"
            };
        }
    }
}