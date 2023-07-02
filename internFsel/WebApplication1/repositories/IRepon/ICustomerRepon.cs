using WebApplication1.Models;
using WebApplication1.ViewModel.Customer;
using WebApplication1.ViewModel.User;

namespace WebApplication1.repositories.IRepon
{
    public interface ICustomerRepon
    {
        Task<List<Customer>> GetList();
        Task<bool> CheckEmail(string email);
        Task<bool> CheckEmail(Guid id, string email);
        Task<bool> CheckPhoneNumber(string phone);
        Task<bool> CheckPhoneNumber(Guid id, string phone);
        Task<int> Create(Customer customer);
        Task<int> Update(Customer customer);
        Task<int> Delete(Customer customer);
        Task<Customer> GetById(Guid Id);
        Task<Customer> GetBySdt(string sdt);
    }
}