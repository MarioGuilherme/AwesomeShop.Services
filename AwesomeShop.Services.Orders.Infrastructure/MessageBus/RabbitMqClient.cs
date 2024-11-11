using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using RabbitMQ.Client;
using System.Text;

namespace AwesomeShop.Services.Orders.Infrastructure.MessageBus;

public class RabbitMqClient(ProducerConnection producerConnection) : IMessageBusClient {
    private readonly IConnection _connection = producerConnection.Connection;
    private readonly JsonSerializerSettings _settings = new() {
        NullValueHandling = NullValueHandling.Ignore,
        ContractResolver = new CamelCasePropertyNamesContractResolver()
    };

    public async Task PublishAsync(object message, string routingKey, string exchange) {
        IChannel channel = await this._connection.CreateChannelAsync();

        string payload = JsonConvert.SerializeObject(message, this._settings);
        byte[] body = Encoding.UTF8.GetBytes(payload);

        await channel.ExchangeDeclareAsync(exchange, "topic", true); // Da para criar com as conexões (exchange bindings) já para conectar os bindings/routing
        await channel.BasicPublishAsync(exchange, routingKey, false, body);
    }
}