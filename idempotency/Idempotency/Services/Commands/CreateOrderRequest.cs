using System;
using Infrastructure.Idempotency.Abstractions;
using MediatR;

namespace Idempotency.Services.Commands
{
    public class CreateOrderRequest : IIdempotentCommand<Guid>, IRequest<Guid>
    {
        public string CommandTypeId => "createOrder";

        public string Description { get; set; }

        public string SerializeResult(Guid input)
        {
            return input.ToString();
        }

        public Guid DeserializeResult(string input)
        {
            return Guid.Parse(input);
        }
    }
}