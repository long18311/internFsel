using MediatR;
using OrderAPI.repositories.IRepon;
using OrderAPI.ViewModel.Order;
using static OrderAPI.Commands.OrderCommand;

namespace OrderAPI.Handlers
{
    public class UpdateOrderHandler : IRequestHandler<UpdateOrderCommand, int>
    {
        private readonly IOrderRepon _orderRepon;
        public UpdateOrderHandler(IOrderRepon orderRepon) {
            _orderRepon = orderRepon; 
        }
        public async Task<int> Handle(UpdateOrderCommand request, CancellationToken cancellationToken)
        {
            if (request.model == null)
            {
                return 2;
            }
            Guid customerid = await _orderRepon.GetCustomerIdBySdt(request.model.PhoneNumber);
            if (customerid == null || customerid == new Guid())
            {
                return 3;
            }
            var order = await _orderRepon.GetOrderById(request.model.Id);
            if (order == null)
            {
                return 4;
            }
            order.CustomerId = customerid;
            order.OrderDate = request.model.OrderDate;
            return await _orderRepon.Update(order);
        }
    }
}
