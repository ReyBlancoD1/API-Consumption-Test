using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using ProductManagement.Domain.Entities;

namespace ProductManagement.Data.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly string _connectionString;

    public ProductRepository(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
    }

    public async Task<Product> CreateAsync(Product product)
    {
        using var connection = new SqlConnection(_connectionString);
        using var command = new SqlCommand("sp_Product_Create", connection)
        {
            CommandType = CommandType.StoredProcedure
        };

        command.Parameters.AddWithValue("@Name", product.Name);
        command.Parameters.AddWithValue("@Description", (object?)product.Description ?? DBNull.Value);
        command.Parameters.AddWithValue("@Price", product.Price);

        await connection.OpenAsync();
        using var reader = await command.ExecuteReaderAsync();

        if (await reader.ReadAsync()) return MapFromReader(reader);
        throw new InvalidOperationException("Failed to create product.");
    }

    public async Task<IEnumerable<Product>> GetAllAsync()
    {
        var products = new List<Product>();
        using var connection = new SqlConnection(_connectionString);
        using var command = new SqlCommand("sp_Product_GetAll", connection)
        {
            CommandType = CommandType.StoredProcedure
        };

        await connection.OpenAsync();
        using var reader = await command.ExecuteReaderAsync();
        while (await reader.ReadAsync()) products.Add(MapFromReader(reader));
        return products;
    }

    public async Task<Product?> GetByIdAsync(int id)
    {
        using var connection = new SqlConnection(_connectionString);
        using var command = new SqlCommand("sp_Product_GetById", connection)
        {
            CommandType = CommandType.StoredProcedure
        };
        command.Parameters.AddWithValue("@Id", id);

        await connection.OpenAsync();
        using var reader = await command.ExecuteReaderAsync();
        return await reader.ReadAsync() ? MapFromReader(reader) : null;
    }

    public async Task<bool> UpdateAsync(Product product)
    {
        using var connection = new SqlConnection(_connectionString);
        using var command = new SqlCommand("sp_Product_Update", connection)
        {
            CommandType = CommandType.StoredProcedure
        };
        command.Parameters.AddWithValue("@Id", product.Id);
        command.Parameters.AddWithValue("@Name", product.Name);
        command.Parameters.AddWithValue("@Description", (object?)product.Description ?? DBNull.Value);
        command.Parameters.AddWithValue("@Price", product.Price);

        await connection.OpenAsync();
        var result = await command.ExecuteScalarAsync();
        return Convert.ToInt32(result) > 0;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        using var connection = new SqlConnection(_connectionString);
        using var command = new SqlCommand("sp_Product_Delete", connection)
        {
            CommandType = CommandType.StoredProcedure
        };
        command.Parameters.AddWithValue("@Id", id);

        await connection.OpenAsync();
        var result = await command.ExecuteScalarAsync();
        return Convert.ToInt32(result) > 0;
    }

    private static Product MapFromReader(SqlDataReader reader) => new()
    {
        Id = reader.GetInt32(reader.GetOrdinal("Id")),
        Name = reader.GetString(reader.GetOrdinal("Name")),
        Description = reader.IsDBNull(reader.GetOrdinal("Description")) ? null : reader.GetString(reader.GetOrdinal("Description")),
        Price = reader.GetDecimal(reader.GetOrdinal("Price")),
        CreatedDate = reader.GetDateTime(reader.GetOrdinal("CreatedDate"))
    };
}