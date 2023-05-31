using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Commands;
using WebApplication1.Queries;
using WebApplication1.repositories.IRepon;
using WebApplication1.repositories.Repon;
using WebApplication1.ViewModel.Customer;
using static WebApplication1.Commands.CustomerCommand;
using static WebApplication1.Queries.CustomerQuery;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CustomerController : ControllerBase
    {
        
        private readonly IMediator _mediator;
        public CustomerController( IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpGet]
        [Route("GetAll")]
        [Authorize(Policy = "view_customer_Only")]
        public async Task<IActionResult> Get(int page, int pagesize, [FromQuery] FilterCustomer filterCustomer)
        {
            var lstCustomer = await _mediator.Send(new GetCustomerListQuery(filterCustomer));
            Console.WriteLine(page);
            if (page == null || page == 0)
            {
                var iresponse = new
                {
                    TotalItems = lstCustomer.Count,
                    TotalPagers = 0,
                    PageSize = pagesize,
                    CurrentPage = page,
                    Items = lstCustomer,
                };
                return Ok(iresponse);
            }
            // lấy tổng bản ghi
            int totalItems = lstCustomer.Count;
            // tính toán xem có bao nhiêu trang trên số lượng bản ghi
            int totalPagers = (int)Math.Ceiling((double)totalItems / totalItems);
            // tính toán chỉ số của item đầu tiên trên trang hiện tại
            int skip = (page - 1) * pagesize;
            // chọn ra các item cho trang hiện tại bằn cách sử dụng phương thức Skip và take của LIQN
            var pagedItems = lstCustomer.Skip(skip).Take(pagesize);
            // trả về danh sách item đã chọn và các thông tin về phần trang
            var response = new
            {
                TotalItems = totalItems,
                TotalPagers = totalPagers,
                PageSize = pagesize,
                CurrentPage = page,
                Items = pagedItems,
            };
            return Ok(response);
        }
        [HttpGet]
        [Route("GetById")]
        [Authorize]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _mediator.Send(new GetCustomerbyIdQuery(id));
            if (result == null) { return NotFound(); }
            return Ok(result);
        }
        [HttpGet]
        [Route("GetBySdt")]
        [Authorize]
        public async Task<IActionResult> GetBySdt(string sdt)
        {
            //string token = HttpContext.Request.Cookies["access_token"] ?? HttpContext.Request.Headers["Authorization"];
            //Console.WriteLine(token);
            var result = await _mediator.Send(new GetCustomerbyPhoneNumberQuery(sdt));
            if (result == null) { return Ok(null); }
            return Ok(result);
        }
        [HttpPost]
        [Route("Create")]
        [Authorize]
        public async Task<IActionResult> Create([FromBody] CreateCustomer model)
        {
            var result = await _mediator.Send(new CreateCustomerCommand(model));
            //var result = await _customerRepon.Create( model);
            if (result == 2) { return BadRequest("Email đã được sử dụng"); };
            if (result == 3) { return BadRequest("Số điện thoại này đã được sử dụng"); };
            if (result == 0)
            {
                return BadRequest("Đã sảy ra rỗi, vui lòng thử lại sau");                
            };
            return Ok("Thêm thành công");
        }
        [HttpPost]
        [Route("Createt")]
        public async Task<IActionResult> Createt([FromBody] CreateCustomer model)
        {
            
            var result = await _mediator.Send(new CreatetCustomerCommand(model));

            return Ok(result);
        }

        [HttpPut]
        [Route("Update")]
        [Authorize]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateCustomer model)
        {
            var result = await _mediator.Send(new UpdateCustomerCommand(id, model));
            if (result == 2) { return BadRequest("Email đã được sử dụng"); };
            if (result == 3) { return BadRequest("Số điện thoại này đã được sử dụng"); };
            if (result == 0)
            {
                return BadRequest("Đã sảy ra rỗi, vui lòng thử lại sau");
            };
            return Ok("Sửa thành công");
        }
        [HttpDelete]
        [Route("Delete/{id}")]
        [Authorize]
        public async Task<IActionResult> Delete(Guid id)
        {
            Console.WriteLine("vào rồi nè");
            var result = await _mediator.Send(new DeleteCustomerCommand(id));
            if (result == 2) { return BadRequest("không tìm thấy Customer");
            }
            if (result == 0)
            {
                return BadRequest("Lỗi hệ thống, vui lòng thử lại sau");
            }

            return Ok("xóa thành công");
        }
    }
}
