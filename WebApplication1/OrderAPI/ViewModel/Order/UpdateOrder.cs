using System.ComponentModel.DataAnnotations;

namespace OrderAPI.ViewModel.Order
{
    public class UpdateOrder
    {
        [Required(ErrorMessage = "Không được bỏ trống số Id hóa đơn")]
        public Guid Id { get; set; }
        [Required(ErrorMessage = "Không được bỏ trống số đien thoai")]
        [RegularExpression(@"^0\d{9}$", ErrorMessage = "Hãy nhập đúng định dạng Số điện thoại")]
        public string PhoneNumber { get; set; }
        [Required(ErrorMessage = "Không được bỏ trống ngày Order")]
        public DateTime OrderDate { get; set; }
    }
}
