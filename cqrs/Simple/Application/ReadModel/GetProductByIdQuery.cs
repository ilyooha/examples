using System;
using MediatR;

namespace Simple.Application.ReadModel
{
    public class GetProductByIdQuery : IRequest<IProduct?>
    {
        public Guid Id { get; set; }
    }
}