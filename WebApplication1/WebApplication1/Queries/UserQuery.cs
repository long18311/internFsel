using MediatR;
using WebApplication1.ViewModel.Customer;
using WebApplication1.ViewModel.User;

namespace WebApplication1.Queries
{
    public class UserQuery
    {
        public record LoginQuery(Loginmodel loginmodel) : IRequest<string>;
    }
}
