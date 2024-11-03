using AwesomeShop.Services.Orders.Core.Entities;

namespace AwesomeShop.Services.Orders.Application.Dtos.ViewModels;

public class OrderViewModel(Guid id, decimal totalPrice, DateTime createdAt, string status) {
    public Guid Id { get; private set; } = id;
    public decimal TotalPrice { get; set; } = totalPrice;
    public DateTime CreatedAt { get; private set; } = createdAt;
    public string Status { get; private set; } = status;

    public static OrderViewModel FromEntity(Order order) => new(order.Id, order.TotalPrice, order.CreatedAt, order.Status.ToString());

    //public static explicit operator OrderViewModel(Order order) => new(order.Id, order.TotalPrice, order.CreatedAt, order.Status.ToString());
}