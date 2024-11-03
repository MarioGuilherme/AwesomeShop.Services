using AwesomeShop.Services.Orders.Application.Dtos.ViewModels;
using AwesomeShop.Services.Orders.Core.Entities;
using AwesomeShop.Services.Orders.Core.Repositories;
using MediatR;

namespace AwesomeShop.Services.Orders.Application.Queries.Handlers;

public class GetOrderByIdHandler(IOrderRepository repository) : IRequestHandler<GetOrderById, OrderViewModel> {
    private readonly IOrderRepository _repository = repository;

    public async Task<OrderViewModel> Handle(GetOrderById request, CancellationToken cancellationToken) {
        Order order = await this._repository.GetByIdAsync(request.Id);
        OrderViewModel orderViewModel = OrderViewModel.FromEntity(order);
        //OrderViewModel orderViewModel = (OrderViewModel)order; // explicit operator
        return orderViewModel;
    }
}