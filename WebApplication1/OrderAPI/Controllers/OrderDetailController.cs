using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OrderAPI.Models;
using OrderAPI.repositories.IRepon;
using OrderAPI.ViewModel.OrderDetail;
using static OrderAPI.Commands.OrderDetailCommand;
using static OrderAPI.Queries.OrderDetailQuery;
using static OrderAPI.Queries.OrderQuery;

namespace OrderAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class OrderDetailController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IOrderDetailRepon _orderDetailRepon;
        public OrderDetailController(IMediator mediator) {
            _mediator = mediator;
        }
        [HttpGet]
        [Route("getbyId")]
        public async Task<IActionResult> GetById(Guid id)
        {
            
            var result = await _mediator.Send(new GetOrderDetailByIdQuery(id));
            if (result == null) {
                return Ok("không tim thấy");
            }
            return Ok(result);
        }
        [HttpGet]
        [Route("getlst")]
        //
        public async Task<IActionResult> GetLstByOrderId(Guid orderId)
        {
            if (orderId == null|| orderId == new Guid())
            {
                
                return Ok(await _mediator.Send(new GetOrderDetailAllListQuery()));
            }
            var result = await _mediator.Send(new GetOrderDetailByOrderIdListQuery(orderId));
            return Ok(result);
        }
        [HttpPost]
        //[Authorize]
        public async Task<IActionResult> Create([FromBody] CreateOrderDetail createOrderDetail)
        {
            
            int result = await _mediator.Send(new CreateOrderDetailCommand(createOrderDetail));
            if (result == 2) { return Ok("không tìm thấy hóa đơn để thêm hóa đơn chi tiết"); }
            if (result == 1) { return Ok("thêm thành công"); }
            if (result == 0) { return Ok("Lỗi hệ thống trong quá trính sửa Order thử lại sau"); }
            if (result == 3) { return Ok("Lỗi hệ thống trong quá trính thêm Order Detail thử lại sau"); }
                return Ok(result);
            }
        [HttpPut]
        //[Authorize]
        public async Task<IActionResult> Update([FromBody]  UpdateOrderDetail updateOrderDetail)
        {
            
            int result = await _mediator.Send(new UpdateOrderDetailCommand(updateOrderDetail));
            if (result == 2) { return Ok("Không tìm thấy Order Detail đẻ sửa");}
            if (result == 1) { return Ok("sủa thành công"); }
            if( result == 0) { return Ok("Lỗi hệ thống vui long thử lại sau"); }
            return Ok(result);
        }
        [HttpDelete]
        [Route("Delete/{id}")]
        //[Authorize]
        public async Task<IActionResult> Delete( Guid id) {
            
            int result = await _mediator.Send(new DeleteOrderDetailCommand(id));
            if (result == 0) { return Ok("Không tìm thấy hóa đơn chi tiết để xóa"); }
            if (result == 0) { return Ok("Không sủa được hóa đơn chi tiết"); }
            if (result == 1) { return Ok("xóa thành công"); }
            return Ok(result);
        }
    }
}
