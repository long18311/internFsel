using MediatR;
using WebApplication1.Models;
using WebApplication1.ViewModel.Customer;

namespace WebApplication1.Commands
{
    public class CustomerCommand
    {
        public record CreateCustomerCommand(CreateCustomer model) : IRequest<int>;
        public record CreatetCustomerCommand(CreateCustomer model) : IRequest<Customer>;        
        public record UpdateCustomerCommand(Guid Id, UpdateCustomer model) : IRequest<int>;
        public record DeleteCustomerCommand(Guid Id) : IRequest<int>;
    }
}
