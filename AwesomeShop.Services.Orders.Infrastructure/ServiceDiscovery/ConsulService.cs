using Consul;

namespace AwesomeShop.Services.Orders.Infrastructure.ServiceDiscovery;

public class ConsulService(IConsulClient consulClient) : IServiceDiscoveryService {
    private readonly IConsulClient _consulClient = consulClient;

    public async Task<Uri> GetServiceUri(string serviceName, string requestUrl) {
        QueryResult<Dictionary<string, AgentService>> allRegisteredSerice = await this._consulClient.Agent.Services();
        IEnumerable<AgentService> registeredServices = allRegisteredSerice.Response?
            .Where(s => s.Value.Service.Equals(serviceName, StringComparison.OrdinalIgnoreCase))
            .Select(s => s.Value)!;

        AgentService service = registeredServices.First();

        Console.WriteLine(service.Address);

        // localhost, Port: 7067, requestUrl: api/customer/123645645387
        return new($"https://{service.Address}:{service.Port}/{requestUrl}");
    }
}