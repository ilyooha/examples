using System;
using System.Threading;
using System.Threading.Tasks;
using Idempotency.Services.Commands;
using Idempotency.Services.Queries;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Idempotency.Infrastructure.Impl
{
    public class OrderRequestHandler : IRequestHandler<CreateOrderRequest, Guid>,
        IRequestHandler<OrderRequest, IOrder>
    {
        private readonly AppDbContext _dbContext;

        public OrderRequestHandler(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Guid> Handle(CreateOrderRequest request, CancellationToken cancellationToken)
        {
            var dbOrder = new DbOrder
            {
                Id = Guid.NewGuid(),
                Description = request.Description
            };

            await _dbContext.Orders.AddAsync(dbOrder, cancellationToken);

            return dbOrder.Id;
        }

        public async Task<IOrder> Handle(OrderRequest request, CancellationToken cancellationToken)
        {
            var order = await _dbContext.Orders
                .FirstOrDefaultAsync(x => x.Id.Equals(request.Id), cancellationToken);

            return order;
        }
    }
}