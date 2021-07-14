using System;
using System.Threading;
using System.Threading.Tasks;
using Idempotency.Models;
using Idempotency.Services.Commands;
using Idempotency.Services.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Idempotency.Controllers
{
    [ApiController]
    [Route("api/orders")]
    public class OrdersController : ControllerBase
    {
        private readonly IMediator _mediator;

        public OrdersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> Get([FromRoute] Guid id, CancellationToken ct)
        {
            var order = await _mediator.Send(new OrderRequest {Id = id}, ct);

            if (order == null)
                return BadRequest();

            return Ok(new ApiOrder
            {
                Id = order.Id,
                Description = order.Description
            });
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateOrderApiRequest apiRequest, CancellationToken ct)
        {
            var command = new CreateOrderRequest {Description = apiRequest.Description};

            var id = await _mediator.Send(command, ct);

            return await Get(id, ct);
        }
    }
}