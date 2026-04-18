using Microsoft.Extensions.DependencyInjection;
using ProductManagement.Data.Repositories;

namespace ProductManagement.Data;

public static class DependencyInjection
{
    public static IServiceCollection AddDataLayer(this IServiceCollection services)
    {
        services.AddScoped<IProductRepository, ProductRepository>();
        return services;
    }
}