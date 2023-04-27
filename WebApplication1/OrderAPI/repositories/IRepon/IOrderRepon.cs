using OrderAPI.Models;
using OrderAPI.ViewModel.Order;

namespace OrderAPI.repositories.IRepon
{
    public interface IOrderRepon
    {
        Task<int> Create(CreateOrder model);
        Task<int> Update(UpdateOrder model);
        Task<int> Delete(Guid id);
        Task<Order> GetOrderById(Guid id);
        Task<List<Order>> getAll();
        Task<List<Order>> getlst(string phoneNumber);
    }
}
