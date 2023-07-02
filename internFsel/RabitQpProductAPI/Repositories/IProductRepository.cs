using RabitQpProductAPI.Models;

namespace RabitQpProductAPI.Repositories
{
    public interface IProductRepository
    {
        public Task<List<Product>> GetProductList();
        public Task<Product> GetProductById(Guid id);
        public Task<Product> AddProduct(Product product);
        public Task<Product> UpdateProduct(Product product);
        public Task<bool> DeleteProduct(Guid Id);
    }
}
