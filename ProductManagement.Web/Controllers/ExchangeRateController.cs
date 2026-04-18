using Microsoft.AspNetCore.Mvc;
using ProductManagement.Services;

namespace ProductManagement.Web.Controllers;

/// <summary>
/// REST API for consuming the public exchange-rate service.
///   GET /api/exchangerate/{currency}  -> returns USD -> {currency} rate
/// Example: /api/exchangerate/COP
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class ExchangeRateController : ControllerBase
{
    private readonly IExchangeRateService _exchangeRateService;

    public ExchangeRateController(IExchangeRateService exchangeRateService)
    {
        _exchangeRateService = exchangeRateService;
    }

    [HttpGet("{currency}")]
    public async Task<IActionResult> Get(string currency)
    {
        try
        {
            var result = await _exchangeRateService.GetRateAsync(currency);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = ex.Message });
        }
    }
}