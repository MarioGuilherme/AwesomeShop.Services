using RabbitMQ.Client;

namespace AwesomeShop.Services.Orders.Infrastructure.MessageBus;
public class ProducerConnection(IConnection connection) { // Como o MongoDB usa também o IConnection, criou esse classe para isso
    public IConnection Connection { get; private set; } = connection;
}