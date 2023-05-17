using MediatR;
using System.Collections.Generic;
using WebApplication1.Models;
using WebApplication1.repositories.IRepon;
using WebApplication1.ViewModel.Customer;
using static WebApplication1.Queries.CustomerQuery;

namespace WebApplication1.Handlers
{
    public class GetCustomerListHandler : IRequestHandler<GetCustomerListQuery, List<Customer>>
    {
        private readonly ICustomerRepon _customerRepon;
        public GetCustomerListHandler(ICustomerRepon customerRepon)
        {
            _customerRepon = customerRepon;
        }
        public async Task<List<Customer>> Handle(GetCustomerListQuery request, CancellationToken cancellationToken)
        {
            var result = await _customerRepon.GetList();
            if (request.filterCustomer.Fullname != null)
            {
                result = result.Where(p => p.Fullname.Contains(request.filterCustomer.Fullname)).ToList();
            }
            if (request.filterCustomer.Birthday != null && request.filterCustomer.Birthday != new DateTime())
            {
                Console.WriteLine(request.filterCustomer.Birthday);
                result = result.Where(p => p.Birthday == request.filterCustomer.Birthday).ToList();
            }
            if (request.filterCustomer.PhoneNumber != null)
            {
                result = result.Where(p => p.PhoneNumber.Contains(request.filterCustomer.PhoneNumber)).ToList();
            }
            return result;
        }
    }
}
