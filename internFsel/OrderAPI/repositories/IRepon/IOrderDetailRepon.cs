using OrderAPI.Models;
using OrderAPI.ViewModel.OrderDetail;

namespace OrderAPI.repositories.IRepon
{
    public interface IOrderDetailRepon
    {
        Task<int> Create(OrderDetail orderDetail);
        Task<int> Update(OrderDetail orderDetail);
        Task<int> Delete(OrderDetail orderDetail);
        Task<OrderDetail> GetOrderDetailById(Guid id);
        Task<List<OrderDetail>> GetAll();
        Task<List<OrderDetail>> GetListByOrderId(Guid orderid);
    }
}
