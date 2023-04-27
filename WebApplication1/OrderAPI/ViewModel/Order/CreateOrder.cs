using OrderAPI.Models;
using System.ComponentModel.DataAnnotations;

namespace OrderAPI.ViewModel.Order
{
    public class CreateOrder
    {
        [Required(ErrorMessage = "Không được bỏ trống số điện thoai")]
        [RegularExpression(@"^0\d{9}$", ErrorMessage = "Hãy nhập đúng định dạng Số điện thoại")]
        public string PhoneNumber { get; set; }
        [Required(ErrorMessage = "Không được bỏ trống ngày nhận")]
        public DateTime OrderDate { get; set; }
        public List<OrderDetail_CreateOrder>? OrderDetails { get; set; }
        public string? Fullname { get; set; }
        public DateTime? Birthday { get; set; }
        public string? Address { get; set; }
        public string? Email { get; set; }
    }
    public class OrderDetail_CreateOrder
    {
        public string ProductName { get; set; }
        public long UnitPrice { get; set; }
        public int Quantity { get; set; }
        public long Total()
        {
            return UnitPrice * Quantity;
        }
    }
}
