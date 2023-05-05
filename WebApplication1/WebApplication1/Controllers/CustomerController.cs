using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.repositories.IRepon;
using WebApplication1.repositories.Repon;
using WebApplication1.ViewModel.Customer;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerRepon _customerRepon;
        public CustomerController(ICustomerRepon customerRepon)
        {
            _customerRepon = customerRepon;
        }
        [HttpGet]
        [Route("GetAll")]
        [Authorize]
        public async Task<IActionResult> Get(int page, int pagesize, [FromQuery] FilterCustomer filterCustomer)
        {
            var lstCustomer = await _customerRepon.GetList(filterCustomer);
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
            var result = await _customerRepon.GetById(id);
            if (result == null) { return NotFound(); }
            return Ok(result);
        }
        [HttpGet]
        [Route("GetBySdt")]
        [Authorize]
        public async Task<IActionResult> GetBySdt(string sdt)
        {
            var result = await _customerRepon.GetBySdt(sdt);
            if (result == null) { return Ok(null); }
            return Ok(result);
        }
        [HttpPost]
        [Route("Create")]
        [Authorize]
        public async Task<IActionResult> Create([FromBody] CreateCustomer model)
        {
            var result = await _customerRepon.Create( model);
            if (result == 1) { return BadRequest("Email đã được sử dụng"); };
            if (result == 2) { return BadRequest("Số điện thoại này đã được sử dụng"); };
            if (result == 4)
            {
                return BadRequest("Đã sảy ra rỗi, vui lòng thử lại sau");                
            };
            return Ok("Thêm thành công");
        }
        [HttpPost]
        [Route("Createt")]
        public async Task<IActionResult> Createt([FromBody] CreateCustomer model)
        {
            
            var result = await _customerRepon.Createt(model);
            
            return Ok(result);
        }

        [HttpPut]
        [Route("Update")]
        [Authorize]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateCustomer model)
        {
            var result = await _customerRepon.Update(id,model);
            if (result == 1) { return BadRequest("Email đã được sử dụng"); };
            if (result == 2) { return BadRequest("Số điện thoại này đã được sử dụng"); };
            if (result == 2)
            {
                return BadRequest("Đã sảy ra rỗi, vui lòng thử lại sau");
            };
            return Ok("Sửa thành công");
        }
        [HttpDelete]
        [Route("Delete")]
        [Authorize]
        public async Task<IActionResult> Delete(Guid id)
        {
            Console.WriteLine("vào rồi nè");
            var result = await _customerRepon.Delete(id);
            if (result == 1) { return BadRequest("không tìm thấy Customer");
            }
            if (result == 3)
            {
                return BadRequest("Lỗi hệ thống, vui lòng thử lại sau");
            }

            return Ok("xóa thành công");
        }
    }
}
