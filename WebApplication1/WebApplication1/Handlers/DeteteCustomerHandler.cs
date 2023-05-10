using MediatR;
using WebApplication1.repositories.IRepon;
using static WebApplication1.Commands.CustomerCommand;

namespace WebApplication1.Handlers
{
    public class DeteteCustomerHandler : IRequestHandler<DeteteCustomerCommand, int>
    {
        private readonly ICustomerRepon _customerRepon;
        public DeteteCustomerHandler(ICustomerRepon customerRepon) {
            _customerRepon = customerRepon;
        }
        public async Task<int> Handle(DeteteCustomerCommand request, CancellationToken cancellationToken)
        {
            var customer = await _customerRepon.GetById(request.Id);
            if (customer == null) return 2;
            return await _customerRepon.Delete(customer);
        }
    }
}
