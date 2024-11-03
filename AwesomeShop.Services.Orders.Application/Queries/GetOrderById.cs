using AwesomeShop.Services.Orders.Application.Dtos.ViewModels;
using MediatR;

namespace AwesomeShop.Services.Orders.Application.Queries;

public class GetOrderById(Guid id) : IRequest<OrderViewModel> {
    public Guid Id { get; private set; } = id;
}