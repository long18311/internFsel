using MediatR;
using WebApplication1.Models;
using WebApplication1.repositories.IRepon;
using static WebApplication1.Commands.CustomerCommand;

namespace WebApplication1.Handlers
{
    public class CreatetCustomerHandler : IRequestHandler<CreatetCustomerCommand, Customer>
    {
        private readonly ICustomerRepon _customerRepon;
        public CreatetCustomerHandler(ICustomerRepon customerRepon)
        {
            _customerRepon = customerRepon;
        }
        public async Task<Customer> Handle(CreatetCustomerCommand request, CancellationToken cancellationToken)
        {
            var checkemail = await (_customerRepon.CheckEmail(request.model.Email));
            if (checkemail == false)
            {
                return null;
            }
            var checkphone = await (_customerRepon.CheckPhoneNumber(request.model.PhoneNumber));
            if (checkphone == false)
            {
                return null;
            }
            Customer customer = new Customer()
            {
                Id = Guid.NewGuid(),
                Fullname = request.model.Fullname,
                PhoneNumber = request.model.PhoneNumber,
                Birthday = request.model.Birthday,
                Email = request.model.Email,
                Address = request.model.Address
            };
            if (await _customerRepon.Create(customer) == 1)
            {
                return customer;
            }
            return null;
            
        }
            
    }
}
