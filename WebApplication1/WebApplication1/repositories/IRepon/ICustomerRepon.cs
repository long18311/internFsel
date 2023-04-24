using WebApplication1.Models;
using WebApplication1.ViewModel.Customer;
using WebApplication1.ViewModel.User;

namespace WebApplication1.repositories.IRepon
{
    public interface ICustomerRepon
    {
        Task<List<Customer>> GetList(FilterCustomer filterCustomer);
        Task<int> Create(CreateCustomer model);
        Task<int> Update(Guid Id, UpdateCustomer model);
        Task<int> Delete(Guid Id);
        Task<Customer> GetById(Guid Id);
        Task<Customer> GetBySdt(string sdt);
        Task<Customer> Createt(CreateCustomer model);
    }
}
