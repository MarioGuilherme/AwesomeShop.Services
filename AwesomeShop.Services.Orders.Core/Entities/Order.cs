using AwesomeShop.Services.Orders.Core.Enums;
using AwesomeShop.Services.Orders.Core.Events;
using AwesomeShop.Services.Orders.Core.ValueObjects;

namespace AwesomeShop.Services.Orders.Core.Entities;

public class Order : AggregateRoot {
    public decimal TotalPrice { get; private set; }
    public Customer Customer { get; set; }
    public DeliveryAddress DeliveryAddress { get; private set; }
    public PaymentAddress PaymentAddress { get; private set; }
    public PaymentInfo PaymentInfo { get; private set; }
    public IEnumerable<OrderItem> Items { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public OrderStatus Status { get; private set; }

    // Padrão Factory
    // public static Order CreateFromCustomerAddressesPaymentAndItems() { }
    public Order(Customer customer, DeliveryAddress deliveryAddress, PaymentAddress paymentAddress, PaymentInfo paymentInfo, IEnumerable<OrderItem> items) {
        this.Id = Guid.NewGuid();
        this.TotalPrice = items.Sum(i => i.Quantity * i.Price);
        this.Customer = customer;
        this.DeliveryAddress = deliveryAddress;
        this.PaymentAddress = paymentAddress;
        this.PaymentInfo = paymentInfo;
        this.Items = items;
        this.CreatedAt = DateTime.Now;
        this.Status = OrderStatus.Started;

        this.AddEvent(new OrderCreated(this.Id, this.TotalPrice, this.PaymentInfo, this.Customer.FullName, this.Customer.Email));
    }

    public void SetAsCompleted() => this.Status = OrderStatus.Completed;

    public void SetAsRejected() => this.Status = OrderStatus.Rejected; // Quando pagamento recusado
}