using System.Text.Json;
using ProductManagement.Domain.DTOs;

namespace ProductManagement.Services;

public class ExchangeRateService : IExchangeRateService
{
    private readonly HttpClient _httpClient;
    private const string API_URL = "https://open.er-api.com/v6/latest/USD";

    public ExchangeRateService(HttpClient httpClient) => _httpClient = httpClient;

    public async Task<ExchangeRateDto> GetRateAsync(string targetCurrency)
    {
        targetCurrency = targetCurrency.ToUpperInvariant();

        var response = await _httpClient.GetAsync(API_URL);
        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadAsStringAsync();
        using var doc = JsonDocument.Parse(json);

        var rates = doc.RootElement.GetProperty("rates");
        if (!rates.TryGetProperty(targetCurrency, out var rateElement))
            throw new InvalidOperationException($"Currency '{targetCurrency}' not found in API response.");

        return new ExchangeRateDto
        {
            BaseCurrency = "USD",
            TargetCurrency = targetCurrency,
            Rate = rateElement.GetDecimal(),
            FetchedAt = DateTime.UtcNow
        };
    }
}