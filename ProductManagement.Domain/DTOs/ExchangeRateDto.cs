namespace ProductManagement.Domain.DTOs;

public class ExchangeRateDto
{
    public string BaseCurrency { get; set; } = "USD";
    public string TargetCurrency { get; set; } = string.Empty;
    public decimal Rate { get; set; }
    public DateTime FetchedAt { get; set; }
}