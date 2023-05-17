using MediatR;
using OrderAPI.Models;
using OrderAPI.repositories.IRepon;
using OrderAPI.repositories.Repon;
using OrderAPI.ViewModel.OrderDetail;
using static OrderAPI.Commands.OrderDetailCommand;

namespace OrderAPI.Handlers
{
    public class UpdateOrderDetailHandler : IRequestHandler<UpdateOrderDetailCommand, int>
    {
        private readonly IOrderDetailRepon _orderDetailRepon;
        private readonly IOrderRepon _orderRepon;
        public UpdateOrderDetailHandler(IOrderDetailRepon orderDetailRepon, IOrderRepon orderRepon)
        {
            _orderDetailRepon = orderDetailRepon;
            _orderRepon = orderRepon;
        }

        public async Task<int> Handle(UpdateOrderDetailCommand request, CancellationToken cancellationToken)
        {
            OrderDetail orderDetail = await _orderDetailRepon.GetOrderDetailById(request.model.Id);
            if (orderDetail == null)
            {
                return 2;
            }
            Order order = await _orderRepon.GetOrderById(orderDetail.OrderId);
            order.TotalPrice = order.TotalPrice + (orderDetail.Quantity * orderDetail.UnitPrice - request.model.Quantity * request.model.UnitPrice);
            orderDetail.ProductName = request.model.ProductName;
            orderDetail.Quantity = request.model.Quantity;
            orderDetail.UnitPrice = request.model.UnitPrice;
            if (await _orderDetailRepon.Update(orderDetail) == 1)
            {
                return await _orderRepon.Update(order);
            }
            else { return 3; }

        }
    }
}
