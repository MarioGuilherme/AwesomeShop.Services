using AwesomeShop.Services.Orders.Infrastructure.Persistences;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using AwesomeShop.Services.Orders.Core.Repositories;
using AwesomeShop.Services.Orders.Infrastructure.Persistences.Repositories;

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
}