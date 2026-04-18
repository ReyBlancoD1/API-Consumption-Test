using Microsoft.Extensions.DependencyInjection;

namespace ProductManagement.Services;

public static class DependencyInjection
{
    public static IServiceCollection AddServicesLayer(this IServiceCollection services)
    {
        services.AddHttpClient<IExchangeRateService, ExchangeRateService>();
        return services;
    }
}