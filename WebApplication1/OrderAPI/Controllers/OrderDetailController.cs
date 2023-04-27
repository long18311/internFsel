using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OrderAPI.repositories.IRepon;
using OrderAPI.ViewModel.OrderDetail;

namespace OrderAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderDetailController : ControllerBase
    {
        private readonly IOrderDetailRepon _orderDetailRepon;
        public OrderDetailController(IOrderDetailRepon orderDetailRepon) {
            _orderDetailRepon = orderDetailRepon;
        }
        [HttpGet]
        [Route("getbyId")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _orderDetailRepon.GetOrderDetailById(id);
            if (result == null) {
                return Ok("không tim thấy");
            }
            return Ok(result);
        }
        [HttpGet]
        [Route("getlst")]
        //[Authorize]
        public async Task<IActionResult> GetLstByOrderId(Guid orderId)
        {
            if (orderId == null|| orderId == new Guid())
            {
                return Ok(await _orderDetailRepon.GetAll());
            }
            var result = await _orderDetailRepon.Getlist(orderId);
            return Ok(result);
        }
        [HttpPost]
        //[Authorize]
        public async Task<IActionResult> Create([FromBody] CreateOrderDetail createOrderDetail)
        {
            int result = await _orderDetailRepon.Create(createOrderDetail);
            if (result == 1) { return Ok("không tìm thấy hóa đơn để thêm hóa đơn chi tiết"); }
            if (result == 2) { return Ok("thêm thành công"); }
            if(result == 3) { return Ok("Lỗi hệ thống vui lòng thử lại sau"); }
            return Ok(result);
            }
        [HttpPut]
        //[Authorize]
        public async Task<IActionResult> Update([FromBody]  UpdateOrderDetail updateOrderDetail)
        {
            int result = await _orderDetailRepon.Update(updateOrderDetail);
            if (result == 1) { return Ok("Không tìm thấy Order Detail đẻ sửa");}
            if (result == 2) { return Ok("sủa thành công"); }
            if( result == 3) { return Ok("Lỗi hệ thống vui long thử lại sau"); }
            return Ok(result);
        }
        [HttpDelete]
        //[Authorize]
        public async Task<IActionResult> Delete([FromBody] Guid Id) {
            int result = await _orderDetailRepon.Delete(Id);
            if (result == 1) { return Ok("Không tìm thấy hóa đơn chi tiết để xóa"); }
            if (result == 2) { return Ok("xóa thành công"); }
            return Ok(result);
        }
    }
}
