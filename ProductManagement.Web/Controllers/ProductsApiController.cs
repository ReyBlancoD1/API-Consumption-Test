using Microsoft.AspNetCore.Mvc;
using ProductManagement.Data.Repositories;
using ProductManagement.Domain.Entities;

namespace ProductManagement.Web.Controllers;

/// <summary>
/// REST API for Product CRUD operations.
///   GET    /api/products
///   GET    /api/products/{id}
///   POST   /api/products
///   PUT    /api/products/{id}
///   DELETE /api/products/{id}
/// </summary>
[ApiController]
[Route("api/products")]
public class ProductsApiController : ControllerBase
{
    private readonly IProductRepository _repository;
    private readonly ILogger<ProductsApiController> _logger;

    public ProductsApiController(IProductRepository repository, ILogger<ProductsApiController> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    // GET: api/products
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Product>>> GetAll()
    {
        var products = await _repository.GetAllAsync();
        return Ok(products);
    }

    // GET: api/products/5
    [HttpGet("{id:int}")]
    public async Task<ActionResult<Product>> GetById(int id)
    {
        var product = await _repository.GetByIdAsync(id);
        if (product == null)
            return NotFound(new { message = $"Product with id {id} not found." });
        return Ok(product);
    }

    // POST: api/products
    [HttpPost]
    public async Task<ActionResult<Product>> Create([FromBody] Product product)
    {
        if (string.IsNullOrWhiteSpace(product.Name))
            return BadRequest(new { message = "Name is required." });
        if (product.Price < 0)
            return BadRequest(new { message = "Price cannot be negative." });

        var created = await _repository.CreateAsync(product);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    // PUT: api/products/5
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] Product product)
    {
        product.Id = id;

        if (string.IsNullOrWhiteSpace(product.Name))
            return BadRequest(new { message = "Name is required." });

        var updated = await _repository.UpdateAsync(product);
        if (!updated)
            return NotFound(new { message = $"Product with id {id} not found." });

        return NoContent();
    }

    // DELETE: api/products/5
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _repository.DeleteAsync(id);
        if (!deleted)
            return NotFound(new { message = $"Product with id {id} not found." });
        return NoContent();
    }
}