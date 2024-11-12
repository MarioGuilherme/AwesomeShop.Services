using AwesomeShop.Services.Orders.Application.Dtos.IntegrationsDtos;
using AwesomeShop.Services.Orders.Core.Entities;
using AwesomeShop.Services.Orders.Core.Events;
using AwesomeShop.Services.Orders.Core.Repositories;
using AwesomeShop.Services.Orders.Infrastructure.MessageBus;
using AwesomeShop.Services.Orders.Infrastructure.ServiceDiscovery;
using MediatR;
using System.Text.Json;

namespace AwesomeShop.Services.Orders.Application.Commands.Handlers;

public class AddOrderHandler(IOrderRepository orderRepository, IMessageBusClient messageBus, IServiceDiscoveryService serviceDiscoveryService) : IRequestHandler<AddOrder, Guid> {
    private readonly IOrderRepository _orderRepository = orderRepository;
    private readonly IMessageBusClient _messageBus = messageBus;
    private readonly IServiceDiscoveryService _serviceDiscoveryService = serviceDiscoveryService;

    public async Task<Guid> Handle(AddOrder request, CancellationToken cancellationToken) {
        Order order = request.ToEntity();

        #region Exemplo de uso do Consul (Service Discovery) para buscar dados de um outro microsserviço
        // Separar a lógica HTTP num Service de integração a parte (CustomerIntegrationService na Infrastructure por exemplo)
        Uri customerUrl = await this._serviceDiscoveryService.GetServiceUri("CustomerService", $"/api/customers/{order.Customer.Id}");
        using HttpClient httpClient = new();
        HttpResponseMessage result = await httpClient.GetAsync(customerUrl, cancellationToken);
        string stringResult = await result.Content.ReadAsStringAsync(cancellationToken);
        Console.WriteLine(stringResult);
        GetCustomerById customerDto = JsonSerializer.Deserialize<GetCustomerById>(stringResult)!;
        Console.WriteLine(customerDto.FullName);
        #endregion

        await this._orderRepository.AddAsync(order);

        foreach (IDomainEvent @event in order.Events) {
            string routingKey = @event.GetType().Name.ToKebabCase();
            await this._messageBus.PublishAsync(@event, routingKey, "order-service");
        }

        return order.Id;
    }
}