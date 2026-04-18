using ProductManagement.Domain.Entities;

namespace ProductManagement.Data.Repositories;

public interface IProductRepository
{
    Task<Product> CreateAsync(Product product);
    Task<IEnumerable<Product>> GetAllAsync();
    Task<Product?> GetByIdAsync(int id);
    Task<bool> UpdateAsync(Product product);
    Task<bool> DeleteAsync(int id);
}