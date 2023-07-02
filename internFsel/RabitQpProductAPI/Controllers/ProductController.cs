using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RabitQpProductAPI.Models;
using RabitQpProductAPI.RabitMQ;
using RabitQpProductAPI.Repositories;
using RabitQpProductAPI.ViewModel.Product;

namespace RabitQpProductAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductRepository _productRepository;
        private readonly IRabitMQProducer _rabitMQProducer;
        public ProductController(IProductRepository productService, IRabitMQProducer rabitMQProducer)
        {
            _productRepository = productService;
            _rabitMQProducer = rabitMQProducer;
        }
        [HttpGet("productlist")]
        public async Task<IActionResult> ProductList()
        {
            var productList = _productRepository.GetProductList();
            return Ok(productList);
        }
        [HttpGet("getproductbyid")]
        public async Task<IActionResult> GetProductById(Guid Id)
        {
            return Ok(_productRepository.GetProductById(Id));
        }
        [HttpPost("addproduct")]
        public async Task<IActionResult> AddProduct(ProductCreate productCreate)
        {
            Product product = new Product()
            {
                ProductId = Guid.NewGuid(),
                ProductName = productCreate.ProductName,
                ProductDescription = productCreate.ProductDescription,
                ProductPrice = productCreate.ProductPrice,
                ProductStock = productCreate.ProductStock
            };
            /*Product productData = await _productRepository.AddProduct(product);*/
            //send the inserted product data to the queue and consumer will listening this data from queue
            //gửi dữ liệu sản phẩm đã chèn vào hàng đợi và người tiêu dùng sẽ nghe dữ liệu này từ hàng đợi
            _rabitMQProducer.SendProductMessage<Product>(product);
            return Ok(product);
        }
        [HttpPut("updateproduct")]
        public async Task<IActionResult> UpdateProduct(Product product)
        {
            var result = await _productRepository.UpdateProduct(product);
            return Ok(result);
        }
        [HttpDelete("deleteproduct")]
        public async Task<IActionResult> DeleteProduct(Guid Id)
        {
            var result = await _productRepository.DeleteProduct(Id);
            return Ok(result);
        }
    }
}
