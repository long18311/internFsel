using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OrderAPI.Models;
using OrderAPI.repositories.IRepon;
using OrderAPI.repositories.Repon;
using OrderAPI.ViewModel.Order;

namespace OrderAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    
    public class OrderController : ControllerBase
    {
        private readonly IOrderRepon _orderRepon;
        public OrderController(IOrderRepon orderRepon)
        {
           _orderRepon = orderRepon;
        }
        [HttpGet]
        [Route("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _orderRepon.getAll();

            return Ok(result);
        }
        [HttpPost]
        //[Authorize]
        public async Task<IActionResult> Create( CreateOrder createOrder)
        {
            int result = await _orderRepon.Create(createOrder);
            if(result == 1) { return Ok("chưa có người dùng này mong bạn điền đầy đủ thông ti"); };
            if (result == 2) { return Ok("lỗi không thêm được người dùng"); };
            if (result == 3) { return Ok("thêm thành công"); };
            if (result == 4) { return Ok("lỗi không thêm được hoán đơn"); };
            if (result == 5) { return Ok("lỗi không thêm được hoán đơn chi tiết"); };
            return Ok("chưa biết ai hơn ai đâu");
        }
        [HttpPost]
        [Route("CreateCustomer")]
        public async Task<IActionResult> CreateCustomer(CreateCustomer createCustomer)
        {
            Customer customer = new Customer();
            using (var httpClient = new HttpClient())
            {
                customer = await (await httpClient.PostAsJsonAsync($"https://localhost:7283/api/Customer/Createt", createCustomer))
               .Content.ReadAsAsync<Customer>();
            }

            return Ok(customer);
        }
    }
}
