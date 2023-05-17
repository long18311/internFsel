using MediatR;
using WebApplication1.Models;
using WebApplication1.repositories.IRepon;
using static WebApplication1.Queries.CustomerQuery;

namespace WebApplication1.Handlers
{
    public class GetCustomerbyPhoneNumberHandler : IRequestHandler<GetCustomerbyPhoneNumberQuery, Customer>
    {
        private readonly ICustomerRepon _customerRepon;
        public GetCustomerbyPhoneNumberHandler(ICustomerRepon customerRepon)
        {
            _customerRepon = customerRepon;
        }
        public Task<Customer> Handle(GetCustomerbyPhoneNumberQuery request, CancellationToken cancellationToken)
        {
            return _customerRepon.GetBySdt(request.PhoneNumber);
        }
    }
}
