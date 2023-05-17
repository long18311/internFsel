using MediatR;
using WebApplication1.Models;
using WebApplication1.repositories.IRepon;
using static WebApplication1.Commands.CustomerCommand;

namespace WebApplication1.Handlers
{
    public class UpdateCustomerHandler : IRequestHandler<UpdateCustomerCommand, int>
    {
        private readonly ICustomerRepon _customerRepon;
        public UpdateCustomerHandler(ICustomerRepon customerRepon)
        {
            _customerRepon = customerRepon;
        }
        public async Task<int> Handle(UpdateCustomerCommand request, CancellationToken cancellationToken)
        {

            var checkemail = await (_customerRepon.CheckEmail(request.Id, request.model.Email));
            if (checkemail == false)
            {
                return 2;
            }
            var checkphone = await (_customerRepon.CheckPhoneNumber(request.Id, request.model.Email));
            if (checkphone == false)
            {
                return 3;
            }
            Customer customer = await _customerRepon.GetById(request.Id);
            customer.Fullname = request.model.Fullname;
            customer.PhoneNumber = request.model.PhoneNumber;
            customer.Birthday = request.model.Birthday;
            customer.Email = request.model.Email;
            customer.Address = request.model.Address;
            return await _customerRepon.Update(customer);
        }
    }
}
