using AwesomeShop.Services.Orders.Application.Commands;
using AwesomeShop.Services.Orders.Application.Subscribers;
using Microsoft.Extensions.DependencyInjection;
using System.Text;

namespace AwesomeShop.Services.Orders.Application;

public static class Extensions {
    public static IServiceCollection AddHandlers(this IServiceCollection services) {
        services.AddMediatR(opt => opt.RegisterServicesFromAssemblyContaining<AddOrder>());

        return services;
    }

    public static IServiceCollection AddSubscribers(this IServiceCollection services) {
        services.AddHostedService<PaymentAcceptedSubscriber>();

        return services;
    }

    public static string ToKebabCase(this string text) {
        ArgumentNullException.ThrowIfNull(text);

        if (text.Length < 2) return text;

        StringBuilder stringBuilder = new();
        stringBuilder.Append(char.ToLowerInvariant(text[0]));

        for (int i = 1; i < text.Length; i++) {
            char c = text[i];
            if (char.IsUpper(c)) {
                stringBuilder.Append('-');
                stringBuilder.Append(char.ToLowerInvariant(c));
            } else
                stringBuilder.Append(c);
        }

        Console.WriteLine($"ToDashCase: {stringBuilder}");

        return stringBuilder.ToString();
    }
}