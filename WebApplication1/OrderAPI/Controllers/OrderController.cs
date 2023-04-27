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
        [HttpGet]
        [Route("GetlstbyPhoneNumber")]
        public async Task<IActionResult> Getlst(string? PhoneNumber)
        {
            List<Order> result = null;
            if (PhoneNumber == null || PhoneNumber == "string") {
                result = await _orderRepon.getAll();
                return Ok(result);
            } else
            {
                result = await _orderRepon.getlst(PhoneNumber);
            }
            return Ok(result);
        }
        [HttpGet]
        [Route("GetbyId")]
        public async Task<IActionResult> GetbyId(Guid id) {
            var result = _orderRepon.GetOrderById(id);
            return Ok(result);
        }
        [HttpPost]
        //[Authorize]
        public async Task<IActionResult> Create([FromBody] CreateOrder createOrder)
        {
           
            int result = await _orderRepon.Create(createOrder);
            if(result == 1) { return Ok("chưa có người dùng này mong bạn điền đầy đủ thông tin"); };
            if (result == 2) { return Ok("lỗi không thêm được người dùng"); };
            if (result == 3) { return Ok("thêm thành công"); };
            if (result == 4) { return Ok("lỗi không thêm được hoán đơn"); };
            if (result == 5) { return Ok("lỗi không thêm được hoán đơn chi tiết"); };
            return Ok("chưa biết ai hơn ai đâu");
        }
        [HttpPut]
        //[Authorize]
        public async Task<IActionResult> Update([FromBody] UpdateOrder updateOrder)
        {
            int result = await _orderRepon.Update(updateOrder);
            if(result == 1) { return Ok("Vui lòn điền đủ thông tin"); }
            if (result == 2) { return Ok("không tìm thấy người mua này vui lòng điền lại số điện thoại người mua"); }
            if (result == 3) { return Ok("Không tìm thấy order này"); }
            if (result == 4) { return Ok("sủa thành công"); }
            if(result == 5) { return Ok("Lỗi hệ thống vui lòng thử lại sau"); }
            return Ok(result);
        }
        [HttpDelete]
        public async Task<IActionResult> Detete(Guid Id) {
            int result = await _orderRepon.Delete(Id);
            if(result == 1) { return Ok("không tìm thấy Order càn xóa"); }
            if (result == 2) { return Ok("xóa thành công"); }
            if (result == 3) { return Ok("lỗi hệ thống vui lòng thử lại sau"); }
            return Ok(result);
        }

    }
}
