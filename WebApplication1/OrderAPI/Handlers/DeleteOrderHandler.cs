using Azure.Core;
using MediatR;
using OrderAPI.Models;
using OrderAPI.repositories.IRepon;
using static OrderAPI.Commands.OrderCommand;

namespace OrderAPI.Handlers
{
    public class DeleteOrderHandler : IRequestHandler<DeleteOrderCommand, int>
    {
        private readonly IOrderRepon _orderRepon;
        public DeleteOrderHandler(IOrderRepon orderRepon)
        {
            _orderRepon = orderRepon;
        }
        public async Task<int> Handle(DeleteOrderCommand request, CancellationToken cancellationToken)
        {
            Order order = await _orderRepon.GetOrderById(request.id);
            if (order == null)
            {
                return 2;
            }
            return await _orderRepon.Delete(order);
        }
    }
}
