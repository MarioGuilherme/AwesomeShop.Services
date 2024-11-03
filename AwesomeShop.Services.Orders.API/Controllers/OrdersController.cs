using AwesomeShop.Services.Orders.Application.Commands;
using AwesomeShop.Services.Orders.Application.Dtos.ViewModels;
using AwesomeShop.Services.Orders.Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AwesomeShop.Services.Orders.API.Controllers;

//[Route("api/customers/{customerId}")]
// => [HttpGet("orders")]
//    public async Task<ActionResult> GetOrders(Guid customerId) { }
[Route("api/orders")]
[ApiController]
public class OrdersController(IMediator mediator) : ControllerBase {
    private readonly IMediator _mediator = mediator;

    [HttpGet("{id}")]
    public async Task<ActionResult> Get(Guid id) {
        GetOrderById getOrderById = new(id);
        OrderViewModel orderViewModel = await this._mediator.Send(getOrderById);
        if (orderViewModel is null) return this.NotFound();
        return this.Ok(orderViewModel);
    }

    [HttpPost]
    public async Task<ActionResult> Post([FromBody] AddOrder command) {
        Guid id = await this._mediator.Send(command);
        return this.CreatedAtAction(nameof(this.Get), new { id }, command);
    }
}