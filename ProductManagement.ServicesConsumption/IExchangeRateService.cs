using ProductManagement.Domain.DTOs;

namespace ProductManagement.Services;

public interface IExchangeRateService
{
    Task<ExchangeRateDto> GetRateAsync(string targetCurrency);
}