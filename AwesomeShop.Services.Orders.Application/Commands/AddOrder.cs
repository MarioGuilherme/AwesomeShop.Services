using AwesomeShop.Services.Orders.Core.Entities;
using MediatR;

namespace AwesomeShop.Services.Orders.Application.Commands;

public class AddOrder : IRequest<Guid> {
    public CustomerInputModel Customer { get; private set; }
    public IEnumerable<OrderItemInputModel> Items { get; private set; }
    public DeliveryAddressInputModel DeliveryAddress { get; private set; }
    public PaymentAddressInputModel PaymentAddress { get; private set; }
    public PaymentInfoInputModel PaymentInfo { get; private set; }

    public Order ToEntity() => new(
        new(this.Customer.Id, this.Customer.FullName, this.Customer.Email),
        new(this.DeliveryAddress.Street, this.DeliveryAddress.Number, this.DeliveryAddress.City, this.DeliveryAddress.State, this.DeliveryAddress.ZipCode),
        new(this.PaymentAddress.Street, this.PaymentAddress.Number, this.PaymentAddress.City, this.PaymentAddress.State, this.PaymentAddress.ZipCode),
        new(this.PaymentInfo.CardNumber, this.PaymentInfo.FullName, this.PaymentInfo.ExpirationDate, this.PaymentInfo.Cvv),
        this.Items.Select(i => new OrderItem(i.ProductId, i.Quantity, i.Price))
    );
}

public class CustomerInputModel {
    public Guid Id { get; private set; }
    public string FullName { get; private set; }
    public string Email { get; private set; }
}

public class OrderItemInputModel {
    public Guid ProductId { get; private set; }
    public int Quantity { get; private set; }
    public decimal Price { get; private set; }
}

public class DeliveryAddressInputModel {
    public string Street { get; private set; }
    public string Number { get; private set; }
    public string City { get; private set; }
    public string State { get; private set; }
    public string ZipCode { get; private set; }
}

public class PaymentAddressInputModel {
    public string Street { get; private set; }
    public string Number { get; private set; }
    public string City { get; private set; }
    public string State { get; private set; }
    public string ZipCode { get; private set; }
}

public class PaymentInfoInputModel {
    public string CardNumber { get; private set; }
    public string ExpirationDate { get; private set; }
    public string FullName { get; private set; }
    public string Cvv { get; private set; }
}