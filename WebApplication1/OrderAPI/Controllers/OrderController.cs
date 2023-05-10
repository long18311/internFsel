
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrderAPI.Models;
using OrderAPI.repositories.IRepon;
using OrderAPI.Service;
using OrderAPI.ViewModel.Order;


namespace OrderAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class OrderController : ControllerBase
    {
        private readonly IOrderRepon _orderRepon;
        public OrderController(IOrderRepon orderRepon )
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
        [Authorize]
        [Route("GetlstbyPhoneNumber")]        
        
        public async Task<IActionResult> Getlst( string? PhoneNumber)
        {

           



            List<Order> result;
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
            var result = await _orderRepon.GetOrderById(id);
            return Ok(result);
        }
        [HttpPost]
        //[Authorize]
        public async Task<IActionResult> Create([FromBody] CreateOrder createOrder)
        {
            
            int result = await _orderRepon.Create(createOrder);
            if(result == 1) { return BadRequest("chưa có người dùng này mong bạn điền đầy đủ thông tin"); };
            if (result == 2) { return BadRequest("lỗi không thêm được người dùng"); };
            if (result == 3) { return BadRequest("thêm thành công"); };
            if (result == 4) { return BadRequest("lỗi không thêm được hoán đơn"); };
            if (result == 5) { return BadRequest("lỗi không thêm được hoán đơn chi tiết"); };
            return Ok("chưa biết ai hơn ai đâu");
        }
        [HttpPut]
        //[Authorize]
        public async Task<IActionResult> Update([FromBody] UpdateOrder updateOrder)
        {
            
            int result = await _orderRepon.Update(updateOrder);
            if(result == 1) { return BadRequest("Vui lòn điền đủ thông tin"); }
            if (result == 2) { return BadRequest("không tìm thấy người mua này vui lòng điền lại số điện thoại người mua"); }
            if (result == 3) { return BadRequest("Không tìm thấy order này"); }
            if (result == 4) { return BadRequest("sủa thành công"); }
            if(result == 5) { return BadRequest("Lỗi hệ thống vui lòng thử lại sau"); }
            return Ok(result);
        }
        [HttpDelete]
        public async Task<IActionResult> Detete(Guid Id) {
            int result = await _orderRepon.Delete(Id);
            if(result == 1) { return BadRequest("không tìm thấy Order càn xóa"); }
            if (result == 2) { return BadRequest("xóa thành công"); }
            if (result == 3) { return BadRequest("lỗi hệ thống vui lòng thử lại sau"); }
            return Ok(result);
        }

    }
}
