using Microsoft.AspNetCore.Mvc;
using ProductManagement.Data.Repositories;
using ProductManagement.Domain.Entities;
using ProductManagement.Services;

namespace ProductManagement.Web.Controllers;

/// <summary>
/// MVC controller that serves the HTML pages for managing products.
/// Routes live under /Products.
/// </summary>
public class ProductsController : Controller
{
    private readonly IProductRepository _repository;
    private readonly IExchangeRateService _exchangeRateService;
    private readonly ILogger<ProductsController> _logger;

    public ProductsController(
        IProductRepository repository,
        IExchangeRateService exchangeRateService,
        ILogger<ProductsController> logger)
    {
        _repository = repository;
        _exchangeRateService = exchangeRateService;
        _logger = logger;
    }

    // GET /Products
    [HttpGet("/Products")]
    public async Task<IActionResult> Index()
    {
        var products = await _repository.GetAllAsync();
        
        try
        {
            var rate = await _exchangeRateService.GetRateAsync("COP");
            ViewBag.UsdToCop = rate.Rate;
            ViewBag.RateFetchedAt = rate.FetchedAt;
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Could not fetch USD->COP exchange rate.");
            ViewBag.UsdToCop = null;
        }

        return View(products);
    }

    // GET /Products/Create
    [HttpGet("/Products/Create")]
    public IActionResult Create() => View();

    // POST /Products/Create
    [HttpPost("/Products/Create")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Product product)
    {
        if (!ModelState.IsValid) return View(product);

        await _repository.CreateAsync(product);
        TempData["Success"] = $"Product '{product.Name}' was created.";
        return RedirectToAction(nameof(Index));
    }

    // GET /Products/Edit/5
    [HttpGet("/Products/Edit/{id:int}")]
    public async Task<IActionResult> Edit(int id)
    {
        var product = await _repository.GetByIdAsync(id);
        if (product == null) return NotFound();
        return View(product);
    }

    // POST /Products/Edit/5
    [HttpPost("/Products/Edit/{id:int}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Product product)
    {
        product.Id = id;
        if (!ModelState.IsValid) return View(product);

        var updated = await _repository.UpdateAsync(product);
        if (!updated) return NotFound();

        TempData["Success"] = $"Product #{id} was updated.";
        return RedirectToAction(nameof(Index));
    }

    // POST /Products/Delete/5
    [HttpPost("/Products/Delete/{id:int}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _repository.DeleteAsync(id);
        if (deleted)
            TempData["Success"] = $"Product #{id} was deleted.";
        else
            TempData["Error"] = $"Product #{id} was not found.";

        return RedirectToAction(nameof(Index));
    }
}