namespace AwesomeShop.Services.Orders.Core.Entities;

public class OrderItem(Guid productId, int quantity, decimal price) : IEntityBase {
    public Guid Id { get; private set; } = Guid.NewGuid();
    public Guid ProductId { get; private set; } = productId;
    public int Quantity { get; private set; } = quantity;
    public decimal Price { get; private set; } = price;
}