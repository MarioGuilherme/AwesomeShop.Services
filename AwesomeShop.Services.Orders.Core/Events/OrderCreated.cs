using AwesomeShop.Services.Orders.Core.ValueObjects;

namespace AwesomeShop.Services.Orders.Core.Events;

public class OrderCreated(Guid id, decimal totalPrice, PaymentInfo paymentInfo, string fullName, string email) : IDomainEvent {
    public Guid Id { get; } = id;
    public decimal TotalPrice { get; } = totalPrice;
    public PaymentInfo PaymentInfo { get; } = paymentInfo;
    public string FullName { get; } = fullName;
    public string Email { get; } = email;
}