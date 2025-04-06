using Turg.App.Models;

namespace Turg.App.Persistence;

public interface IProductRepository
{
     Task<IEnumerable<Product>> Get(string category = null);
     Task<Guid> Insert(Product product);
     Task Update(Product product, Guid? id = null);
}
