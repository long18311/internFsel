using MediatR;
using OrderAPI.Models;
using OrderAPI.repositories.IRepon;
using OrderAPI.repositories.Repon;
using static OrderAPI.Commands.OrderDetailCommand;

namespace OrderAPI.Handlers
{
    public class DeleteOrderDetailHandler : IRequestHandler<DeleteOrderDetailCommand, int>
    {
        private readonly IOrderDetailRepon _orderDetailRepon;
        private readonly IOrderRepon _orderRepon;
        public DeleteOrderDetailHandler(IOrderDetailRepon orderDetailRepon, IOrderRepon orderRepon)
        {
            _orderDetailRepon = orderDetailRepon;
            _orderRepon = orderRepon;
        }

        public async Task<int> Handle(DeleteOrderDetailCommand request, CancellationToken cancellationToken)
        {
            OrderDetail orderDetail = await _orderDetailRepon.GetOrderDetailById(request.id);
            if (orderDetail == null)
            {
                return 2;
            }
            Order order = await _orderRepon.GetOrderById(orderDetail.OrderId);
            order.TotalPrice = order.TotalPrice - (orderDetail.Quantity * orderDetail.UnitPrice);
            if (await _orderDetailRepon.Delete(orderDetail) == 1)
            {
                return await _orderRepon.Update(order);
            }
            else { return 3; }
        }
    }
}
