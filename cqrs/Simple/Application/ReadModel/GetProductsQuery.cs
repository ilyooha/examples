using System.Collections.Generic;
using MediatR;

namespace Simple.Application.ReadModel
{
    public class GetProductsQuery : IRequest<IEnumerable<IProduct>>
    {
    }
}