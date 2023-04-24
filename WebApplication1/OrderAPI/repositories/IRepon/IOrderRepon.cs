using OrderAPI.Models;
using OrderAPI.ViewModel.Order;

namespace OrderAPI.repositories.IRepon
{
    public interface IOrderRepon
    {
        Task<int> Create(CreateOrder model);
        Task<List<Order>> getAll();
    }
}
