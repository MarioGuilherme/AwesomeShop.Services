using AwesomeShop.Services.Orders.Application.Dtos.ViewModels;
using AwesomeShop.Services.Orders.Core.Entities;
using AwesomeShop.Services.Orders.Core.Repositories;
using AwesomeShop.Services.Orders.Infrastructure.CacheStorage;
using MediatR;

namespace AwesomeShop.Services.Orders.Application.Queries.Handlers;

public class GetOrderByIdHandler(IOrderRepository repository, ICacheService cache) : IRequestHandler<GetOrderById, OrderViewModel> {
    private readonly IOrderRepository _repository = repository;
    private readonly ICacheService _cache = cache;

    public async Task<OrderViewModel> Handle(GetOrderById request, CancellationToken cancellationToken) {
        string cacheKey = request.Id.ToString();
        OrderViewModel? orderViewModel = await this._cache.GetAsync<OrderViewModel>(cacheKey);

        if (orderViewModel is not null) return orderViewModel;

        Order order = await this._repository.GetByIdAsync(request.Id);
        orderViewModel = OrderViewModel.FromEntity(order);
        //OrderViewModel orderViewModel = (OrderViewModel)order; // explicit operator
        await this._cache.SetAsync(cacheKey, orderViewModel);

        return orderViewModel;
    }
}