using MediatR;
using WebApplication1.Models;
using WebApplication1.ViewModel.Customer;

namespace WebApplication1.Queries
{
    public class CustomerQuery
    {
        public record GetCustomerListQuery(FilterCustomer filterCustomer) : IRequest<List<Customer>>;
        public record GetCustomerbyIdQuery(Guid id) : IRequest<Customer>;
        public record GetCustomerbyPhoneNumberQuery(string PhoneNumber) : IRequest<Customer>;
    }
}
