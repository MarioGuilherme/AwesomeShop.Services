using AwesomeShop.Services.Orders.Core.Entities;
using AwesomeShop.Services.Orders.Core.Events;
using AwesomeShop.Services.Orders.Core.Repositories;
using AwesomeShop.Services.Orders.Infrastructure.MessageBus;
using MediatR;

namespace AwesomeShop.Services.Orders.Application.Commands.Handlers;

public class AddOrderHandler(IOrderRepository orderRepository, IMessageBusClient messageBus) : IRequestHandler<AddOrder, Guid> {
    private readonly IOrderRepository _orderRepository = orderRepository;
    private readonly IMessageBusClient _messageBus = messageBus;

    public async Task<Guid> Handle(AddOrder request, CancellationToken cancellationToken) {
        Order order = request.ToEntity();
        await this._orderRepository.AddAsync(order);

        foreach (IDomainEvent @event in order.Events) {
            string routingKey = @event.GetType().Name.ToKebabCase();
            await this._messageBus.PublishAsync(@event, routingKey, "order-service");
        }

        return order.Id;
    }
}