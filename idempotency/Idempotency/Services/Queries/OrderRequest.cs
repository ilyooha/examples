using System;
using Infrastructure.CQRS.Abstractions;

namespace Idempotency.Services.Queries
{
    public class OrderRequest : IQuery<IOrder>
    {
        public Guid Id { get; set; }
    }
}