using OrderAPI.Models;
using OrderAPI.ViewModel.OrderDetail;

namespace OrderAPI.repositories.IRepon
{
    public interface IOrderDetailRepon
    {
        Task<int> Create(CreateOrderDetail createOrderDetail);
        Task<int> Update(UpdateOrderDetail updateOrderDetail);
        Task<int> Delete(Guid id);
        Task<OrderDetail> GetOrderDetailById(Guid id);
        Task<List<OrderDetail>> GetAll();
        Task<List<OrderDetail>> Getlist(Guid Orderid);
    }
}
