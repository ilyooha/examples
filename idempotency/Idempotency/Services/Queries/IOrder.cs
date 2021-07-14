using System;

namespace Idempotency.Services.Queries
{
    public interface IOrder
    {
        Guid Id { get; }
        string Description { get; }
    }
}