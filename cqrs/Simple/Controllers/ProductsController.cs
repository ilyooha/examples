using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Simple.Application.ReadModel;
using Simple.Application.WriteModel;

namespace Simple.Controllers
{
    [Route("api/products")]
    public class ProductsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ProductsController(IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        [HttpGet]
        public async Task<IEnumerable<IProduct>> Get(CancellationToken cancellationToken = new())
        {
            var query = new GetProductsQuery();
            var products = await _mediator.Send(query, cancellationToken);
            return products;
        }

        [HttpGet("{id:guid}")]
        public async Task<IProduct> GetById([FromRoute] Guid id, CancellationToken cancellationToken = new())
        {
            var query = new GetProductByIdQuery { Id = id };
            var product = await _mediator.Send(query, cancellationToken);

            if (product is null)
                throw new InvalidOperationException($"Product '{id}' doesn't exist.");

            return product;
        }

        [HttpPost]
        public async Task<IProduct> Post([FromBody] RegisterProductCommand command,
            CancellationToken cancellationToken = new())
        {
            var id = await _mediator.Send(command, cancellationToken);
            return await GetById(id, cancellationToken);
        }
    }
}