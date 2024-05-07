using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Track.Order.Application.Interfaces;
using Track.Order.Application.Services;

namespace Track.Order.Application;

public static class ApplicationBootstrapper
{
    public static IServiceCollection RegisterServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IOrderService, OrderService>();

        return services;
    }
}
