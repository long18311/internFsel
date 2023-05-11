
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrderAPI.Models;
using OrderAPI.repositories.IRepon;
using OrderAPI.Service;
using OrderAPI.ViewModel.Order;
using static OrderAPI.Commands.OrderCommand;
using static OrderAPI.Queries.OrderQuery;

namespace OrderAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class OrderController : ControllerBase
    {
        private readonly IMediator _mediator;
        public OrderController( IMediator mediator)
        {
            _mediator = mediator;


        }
        [HttpGet]
        [Route("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _mediator.Send(new GetOrderAllListQuery());

            return Ok(result);
        }
        [HttpGet]
        [Authorize]
        [Route("GetlstbyPhoneNumber")]        
        
        public async Task<IActionResult> Getlst( string? PhoneNumber)
        {

           



            List<Order> result;
            if (PhoneNumber == null || PhoneNumber == "string") {
                result = await _mediator.Send(new GetOrderAllListQuery());
                return Ok(result);
            } else
            {
                result = await _mediator.Send(new GetOrderListbyPhoneNumberQuery(PhoneNumber));
            }
            return Ok(result);
        }
        
        [HttpGet]
        [Route("GetbyId")]
        public async Task<IActionResult> GetbyId(Guid id) {
            
            var result = await _mediator.Send(new GetOrderByIdQuery(id));
            return Ok(result);
        }
        [HttpPost]
        //[Authorize]
        public async Task<IActionResult> Create([FromBody] CreateOrder createOrder)
        {
            
            int result = await _mediator.Send(new CreateOrderCommand(createOrder));
            if(result == 2) { return BadRequest("chưa có người dùng này mong bạn điền đầy đủ thông tin"); };
            if (result == 3) { return BadRequest("lỗi không thêm được người dùng"); };
            if (result == 1) { return Ok("thêm thành công"); };
            if (result == 0) { return BadRequest("lỗi không hệ thống"); };
            return Ok("chưa biết ai hơn ai đâu");
        }
        [HttpPut]
        //[Authorize]
        public async Task<IActionResult> Update([FromBody] UpdateOrder updateOrder)
        {
            
            int result = await _mediator.Send(new UpdateOrderCommand(updateOrder));
            if(result == 2) { return BadRequest("Vui lòn điền đủ thông tin"); }
            if (result == 3) { return BadRequest("không tìm thấy người mua này vui lòng điền lại số điện thoại người mua"); }
            if (result == 4) { return BadRequest("Không tìm thấy order này"); }
            if (result == 1) { return Ok("sủa thành công"); }
            if(result == 0) { return BadRequest("Lỗi hệ thống vui lòng thử lại sau"); }
            return Ok(result);
        }
        [HttpDelete]
        [Route("Delete/{id}")]
        public async Task<IActionResult> Detete(Guid id) {
            
            int result = await _mediator.Send(new DeleteOrderCommand(id));
            if(result == 2) { return BadRequest("không tìm thấy Order càn xóa"); }
            if (result == 1) { return Ok("xóa thành công"); }
            if (result == 0) { return BadRequest("lỗi hệ thống vui lòng thử lại sau"); }
            return Ok(result);
        }

    }
}
