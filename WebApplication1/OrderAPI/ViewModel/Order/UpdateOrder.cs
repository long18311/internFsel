using System.ComponentModel.DataAnnotations;

namespace OrderAPI.ViewModel.Order
{
    public class UpdateOrder
    {
        [Required(ErrorMessage = "Không được bỏ trống số Id hóa đơn")]
        public Guid Id { get; set; }
        [Required(ErrorMessage = "Không được bỏ trống số đien thoai")]
        public string PhoneNumber { get; set; }
        [Required(ErrorMessage = "Không được bỏ trống ngày Order")]
        public DateTime OrderDate { get; set; }
    }
}
