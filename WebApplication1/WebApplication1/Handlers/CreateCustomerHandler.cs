using MediatR;
using WebApplication1.Models;
using WebApplication1.repositories.IRepon;
using static WebApplication1.Commands.CustomerCommand;

namespace WebApplication1.Handlers
{
    public class CreateCustomerHandler : IRequestHandler<CreateCustomerCommand, int>
    {
        private readonly ICustomerRepon _customerRepon;
        public CreateCustomerHandler(ICustomerRepon customerRepon)
        {
            _customerRepon = customerRepon;
        }
        public async Task<int> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
        {

            var checkemail = await (_customerRepon.CheckEmail(request.model.Email));
            if (checkemail == false)
            {
                return 2;
            }
            var checkphone = await (_customerRepon.CheckPhoneNumber(request.model.PhoneNumber));
            if (checkphone == false)
            {
                return 3;
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

            return await _customerRepon.Create(customer);



        }
    }
}
