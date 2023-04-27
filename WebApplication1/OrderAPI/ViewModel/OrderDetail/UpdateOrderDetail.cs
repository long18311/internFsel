using System.ComponentModel.DataAnnotations;

namespace OrderAPI.ViewModel.OrderDetail
{
    public class UpdateOrderDetail
    {
        [Required(ErrorMessage = "không được bỏ trống Id Hóa đơn Chi tiết")]
        public Guid Id { get; set; }
        [Required(ErrorMessage = "Không được bỏ trống Tên sản phẩm")]
        public string ProductName { get; set; }
        [Required(ErrorMessage = "Không được bỏ trống đơn giá")]
        public long UnitPrice { get; set; }
        [Required(ErrorMessage = "Không được bỏ trống số lượng sản phẩm")]
        public int Quantity { get; set; }
        
    }
}
