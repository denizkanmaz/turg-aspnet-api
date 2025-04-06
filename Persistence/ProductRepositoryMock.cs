using Turg.App.Models;

namespace Turg.App.Persistence;

public class ProductRepositoryMock(ILogger<ProductRepositoryMock> logger) : IProductRepository
{
    private readonly List<Product> _products = new List<Product>
        {
            new Product
            {
                Id = Guid.NewGuid(),
                Name = "Product 1",
                Description = "Description 1",
                Price = 10.0,
                Category = "Category 1"
            },
            new Product
            {
                Id = Guid.NewGuid(),
                Name = "Product 2",
                Description = "Description 2",
                Price = 20.0,
                Category = "Category 2"
            }
        };

    public async Task<IEnumerable<Product>> Get(string category = null)
    {
        logger.LogInformation("Get products from API");

        return category == null
            ? _products
            : _products.Where(p => p.Category.Equals(category, StringComparison.OrdinalIgnoreCase));
    }

    public async Task<Guid> Insert(Product product)
    {
        logger.LogInformation("Insert product into API");

        _products.Add(product);

        return product.Id;
    }


    public async Task Update(Product product, Guid? id = null)
    {
        logger.LogInformation("Update product in API");

        var existingProduct = _products.FirstOrDefault(p => p.Id == id);
        if (existingProduct != null)
        {
            existingProduct.Name = product.Name;
            existingProduct.Description = product.Description;
            existingProduct.Price = product.Price;
            existingProduct.Category = product.Category;
        }
    }
}
