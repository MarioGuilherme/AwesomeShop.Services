namespace AwesomeShop.Services.Orders.Infrastructure.Persistences;

public class MongoDbOptions {
    public string ConnectionString { get; set; } = null!;
    public string Database { get; set; } = null!;
}