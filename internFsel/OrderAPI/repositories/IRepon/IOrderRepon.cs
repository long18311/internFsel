using Newtonsoft.Json.Linq;
using OrderAPI.Models;
using OrderAPI.ViewModel.Order;

namespace OrderAPI.repositories.IRepon
{
    public interface IOrderRepon
    {
        Task<int> Create(Order order, List<OrderDetail> orderDetails);
        Task<int> Update(Order order);
        Task<int> Delete(Order order);
        Task<List<OrderDetail>> GetOrderDetailbyId(Guid Id);
        Task<Guid> GetCustomerIdBySdt(string PhoneNumber);
        Task<Guid> GetCustomerIdByNewCustomer(CreateCustomer createCustomer);
        Task<Order> GetOrderById(Guid id);
        Task<List<Order>> getAll();
        Task<List<Order>> getlistbyCustomerid(Guid id);
    }
}
