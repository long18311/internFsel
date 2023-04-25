using System.ComponentModel.DataAnnotations;

namespace OrderAPI.ViewModel.OrderDetail
{
    public class CreateOrderDetail
    {
        [Required(ErrorMessage = "Không được bỏ trống Id hóa đơn")]
        public Guid OrderId { get; set; }
        [Required(ErrorMessage = "Không được bỏ trống Tên sản phẩm")]
        public string ProductName { get; set; }
        [Required(ErrorMessage = "Không được bỏ trống đơn giá")]
        public long UnitPrice { get; set; }
        [Required(ErrorMessage = "Không được bỏ trống số lượng sản phẩm")]
        public int Quantity { get; set; }
        
    }
}
