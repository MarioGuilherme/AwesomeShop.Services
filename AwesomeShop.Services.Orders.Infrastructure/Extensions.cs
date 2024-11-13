using AwesomeShop.Services.Orders.Infrastructure.Persistences;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using AwesomeShop.Services.Orders.Core.Repositories;
using AwesomeShop.Services.Orders.Infrastructure.Persistences.Repositories;
using AwesomeShop.Services.Orders.Infrastructure.MessageBus;
using RabbitMQ.Client;
using Consul;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;
using AwesomeShop.Services.Orders.Infrastructure.ServiceDiscovery;
using AwesomeShop.Services.Orders.Infrastructure.CacheStorage;

namespace AwesomeShop.Services.Orders.Infrastructure;

public static class Extensions {
    public static IServiceCollection AddMongo(this IServiceCollection services) {
        services.AddSingleton(sp => {
            IConfiguration configuration = sp.GetService<IConfiguration>()!;
            MongoDbOptions mongoDbOptions = new();

            configuration.GetSection("Mongo").Bind(mongoDbOptions);

            return mongoDbOptions;
        });

        services.AddSingleton<IMongoClient>(sp => {
            MongoDbOptions mongoDbOptions = sp.GetService<MongoDbOptions>()!;
            return new MongoClient(mongoDbOptions.ConnectionString);
        });

        services.AddTransient(sp => {
            BsonSerializer.RegisterSerializer(new GuidSerializer(GuidRepresentation.Standard));

            MongoDbOptions mongoDbOptions = sp.GetService<MongoDbOptions>()!;
            IMongoClient mongoClient = sp.GetService<IMongoClient>()!;

            return mongoClient.GetDatabase(mongoDbOptions.Database);
        });

        return services;
    }

    public static IServiceCollection AddRepositories(this IServiceCollection services) {
        services.AddScoped<IOrderRepository, OrderRepository>();

        return services;
    }

    public static IServiceCollection AddMessageBus(this IServiceCollection services) {
        ConnectionFactory connectionFactory = new() {
            HostName = "localhost"
        };

        IConnection connection = connectionFactory.CreateConnectionAsync("order-service-producer").Result;

        services.AddSingleton(new ProducerConnection(connection));
        services.AddSingleton<IMessageBusClient, RabbitMqClient>(); // A conexão fica ativa, e onde solicitar criará um canal a partir da conexão

        return services;
    }

    public static IServiceCollection AddServiceDiscoveryConfig(this IServiceCollection services, IConfiguration configuration) {
        services.AddSingleton<IConsulClient, ConsulClient>(p => new(consulConfig => {
            string address = configuration.GetValue<string>("Consult:Host")!;
            consulConfig.Address = new(address);
        }));

        services.AddTransient<IServiceDiscoveryService, ConsulService>();

        return services;
    }

    public static IApplicationBuilder UseConsul(this IApplicationBuilder app) {
        IConsulClient consulClient = app.ApplicationServices.GetRequiredService<IConsulClient>();
        IHostApplicationLifetime lifeTime = app.ApplicationServices.GetRequiredService<IHostApplicationLifetime>();

        AgentServiceRegistration registration = new() {
            ID = $"order-service-{Guid.NewGuid()}",
            Name = "OrderServices",
            Address = "localhost",
            Port = 7067
        };

        consulClient.Agent.ServiceDeregister(registration.ID).ConfigureAwait(true);
        consulClient.Agent.ServiceRegister(registration).ConfigureAwait(true);

        Console.WriteLine("Service registered in Consul");

        lifeTime.ApplicationStopping.Register(() => {
            consulClient.Agent.ServiceDeregister(registration.ID).ConfigureAwait(true);
            Console.WriteLine("Service deregistered in Consul");
        });

        return app;
    }

    public static IServiceCollection AddRedisCache(this IServiceCollection services) {
        services.AddStackExchangeRedisCache(options => {
            options.InstanceName = "OrdersCache";
            options.Configuration = "localhost:6379";
        });

        services.AddTransient<ICacheService, RedisService>(); // Sempre cria uma nova conexão, por isso é transient

        return services;
    }
}