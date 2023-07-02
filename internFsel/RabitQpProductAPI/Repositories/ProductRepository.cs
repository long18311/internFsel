using RabitQpProductAPI.Data;
using RabitQpProductAPI.Models;
using RabitQpProductAPI.Repositories;

namespace RabitQpProductAPI.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly DbContextClass _dbContext;

        public ProductRepository(DbContextClass dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<List<Product>> GetProductList()
        {
            return _dbContext.products.ToList();
        }
        public async Task<Product> GetProductById(Guid id)
        {
            return _dbContext.products.Where(x => x.ProductId == id).FirstOrDefault();
        }
        public async Task<Product> AddProduct(Product product)
        {
            try
            {
                await _dbContext.products.AddRangeAsync(product);
                _dbContext.SaveChanges();
                return product;
            }catch (Exception ex)
            {
                return null;
            }
        }
        public async Task<Product> UpdateProduct(Product product)
        {
            try { 
            _dbContext.products.Update(product);
            _dbContext.SaveChanges();
            return product;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public async Task<bool> DeleteProduct(Guid Id)
        {
            try { 
            Product filteredData = _dbContext.products.Where(x => x.ProductId == Id).FirstOrDefault();
            var result = _dbContext.Remove(filteredData);
            _dbContext.SaveChanges();
            return result != null ? true : false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
