namespace AwesomeShop.Services.Orders.Infrastructure.MessageBus;

public interface IMessageBusClient {
    Task PublishAsync(object message, string routingKey, string exchange);
}