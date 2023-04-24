

using System.ComponentModel.DataAnnotations;

namespace WebApplication1.ViewModel.Customer
{
    public class CreateCustomer
    {
        [Required(ErrorMessage = "Không được bỏ trống Họ và tên")]
        public string Fullname { get; set; }
        [Required(ErrorMessage = "Không được bỏ trống Ngày sinh")]
        public DateTime Birthday { get; set; }
        [Required(ErrorMessage = "Không được bỏ trống Số điện thoại")]
        [RegularExpression(@"^0\d{9}$", ErrorMessage = "Hãy nhập đúng định dạng Số điện thoại")]
        public string PhoneNumber { get; set; }
        [Required(ErrorMessage = "Không được bỏ trống Địa Chỉ")]
        public string Address { get; set; }
        [Required(ErrorMessage = "Không được bỏ trống Email")]
        [EmailAddress(ErrorMessage = "Vui lòng nhập đúng Email")]
        public string Email { get; set; }
    }
}
