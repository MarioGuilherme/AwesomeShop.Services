using AwesomeShop.Services.Orders.Core.Entities;
using AwesomeShop.Services.Orders.Core.Repositories;
using MongoDB.Driver;

namespace AwesomeShop.Services.Orders.Infrastructure.Persistences.Repositories;

public class OrderRepository(IMongoDatabase mongoDatabase) : IOrderRepository {
    private readonly IMongoCollection<Order> _collection = mongoDatabase.GetCollection<Order>("orders");

    public Task AddAsync(Order order) => this._collection.InsertOneAsync(order);

    public Task<Order> GetByIdAsync(Guid id) => this._collection.Find(o => o.Id == id).SingleOrDefaultAsync();

    public Task UpdateAsync(Order order) => this._collection.ReplaceOneAsync(o => o.Id == order.Id, order);
}