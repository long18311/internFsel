using MediatR;
using WebApplication1.Models;
using WebApplication1.repositories.IRepon;
using static WebApplication1.Queries.CustomerQuery;

namespace WebApplication1.Handlers
{
    public class GetCustomerbyIdHandlers : IRequestHandler<GetCustomerbyIdQuery, Customer>
    {
        private readonly ICustomerRepon _customerRepon;
        public GetCustomerbyIdHandlers(ICustomerRepon customerRepon) {
            _customerRepon = customerRepon;
        }
        public Task<Customer> Handle(GetCustomerbyIdQuery request, CancellationToken cancellationToken)
        {
            return _customerRepon.GetById(request.id);
        }
    }
}
