using AwesomeShop.Services.Orders.Core.Entities;
using AwesomeShop.Services.Orders.Core.Repositories;
using MediatR;

namespace AwesomeShop.Services.Orders.Application.Commands.Handlers;

public class AddOrderHandler(IOrderRepository orderRepository) : IRequestHandler<AddOrder, Guid> {
    private readonly IOrderRepository _orderRepository = orderRepository;

    public async Task<Guid> Handle(AddOrder request, CancellationToken cancellationToken) {
        Order order = request.ToEntity();
        await this._orderRepository.AddAsync(order);
        return order.Id;
    }
}