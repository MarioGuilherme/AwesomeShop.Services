using AwesomeShop.Services.Orders.Core.Entities;
using AwesomeShop.Services.Orders.Core.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace AwesomeShop.Services.Orders.Application.Subscribers;

public class PaymentAcceptedSubscriber : BackgroundService {
    private readonly IServiceProvider _serviceProvider;
    private readonly IConnection _connection;
    private readonly IChannel _channel;
    private const string QUEUE = "order-service/payment-accepted"; // exchange/fila
    private const string EXCHANGE = "order-service";
    private const string ROUTING_KEY = "payment-accepted";

    public PaymentAcceptedSubscriber(IServiceProvider serviceProvider)
    {
        this._serviceProvider = serviceProvider;
        ConnectionFactory connectionFactory = new() {
            HostName = "localhost"
        };
        this._connection = connectionFactory.CreateConnectionAsync("order-service-payment-accepted-subscriber").Result;
        this._channel = this._connection.CreateChannelAsync().Result;
        this._channel.ExchangeDeclareAsync(EXCHANGE, "topic", true).Wait();
        this._channel.QueueDeclareAsync(QUEUE, false, false, false, null).Wait();
        this._channel.QueueBindAsync(QUEUE, "payment-service", ROUTING_KEY);
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken) {
        AsyncEventingBasicConsumer consumer = new(this._channel);
        consumer.ReceivedAsync += async (sender, eventArgs) => {
            byte[] bytes = eventArgs.Body.ToArray();
            string content = Encoding.UTF8.GetString(bytes);
            PaymentAccepted message = JsonConvert.DeserializeObject<PaymentAccepted>(content)!;
            Console.WriteLine($"Message PaymentAccepted received with Id {message.Id}");
            bool isUpdated = await this.UpdateOrderAsync(message);
            if (isUpdated) await this._channel.BasicAckAsync(eventArgs.DeliveryTag, false);
        };
        await this._channel.BasicConsumeAsync(QUEUE, false, consumer, cancellationToken: stoppingToken);
    }

    private async Task<bool> UpdateOrderAsync(PaymentAccepted paymentAccepted) {
        using AsyncServiceScope scope = this._serviceProvider.CreateAsyncScope();
        IOrderRepository orderRepository = scope.ServiceProvider.GetService<IOrderRepository>()!;
        Order order = await orderRepository.GetByIdAsync(paymentAccepted.Id);
        order.SetAsCompleted();
        await orderRepository.UpdateAsync(order);
        return true;
    }
}

public class PaymentAccepted(Guid id, string fullName, string email) {
    public Guid Id { get; private set; } = id;
    public string FullName { get; private set; } = fullName;
    public string Email { get; private set; } = email;
}