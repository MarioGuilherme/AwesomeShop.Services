using AwesomeShop.Services.Orders.Application.Commands;
using Microsoft.Extensions.DependencyInjection;

namespace AwesomeShop.Services.Orders.Application;

public static class Extensions {
    public static IServiceCollection AddHandlers(this IServiceCollection services) {
        services.AddMediatR(opt => opt.RegisterServicesFromAssemblyContaining<AddOrder>());

        return services;
    }
}