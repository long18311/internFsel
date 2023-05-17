using MediatR;
using OrderAPI.Models;
using OrderAPI.repositories.IRepon;
using OrderAPI.repositories.Repon;
using OrderAPI.ViewModel.OrderDetail;
using static OrderAPI.Commands.OrderDetailCommand;

namespace OrderAPI.Handlers
{
    public class CreateOrderDetailHandler : IRequestHandler<CreateOrderDetailCommand, int>
    {
        private readonly IOrderDetailRepon _orderDetailRepon;
        private readonly IOrderRepon _orderRepon;
        public CreateOrderDetailHandler(IOrderDetailRepon orderDetailRepon, IOrderRepon orderRepon)
        {
            _orderDetailRepon = orderDetailRepon;
            _orderRepon = orderRepon;
        }
        public async Task<int> Handle(CreateOrderDetailCommand request, CancellationToken cancellationToken)
        {
            Order order = await _orderRepon.GetOrderById(request.model.OrderId);
            if (order == null)
            {
                return 2;
            }
            order.TotalPrice = order.TotalPrice + (request.model.Quantity * request.model.UnitPrice);
            OrderDetail orderDetail = new OrderDetail()
            {
                Id = Guid.NewGuid(),
                ProductName = request.model.ProductName,
                Quantity = request.model.Quantity,
                UnitPrice = request.model.UnitPrice,
                Order = order,
                OrderId = order.Id,
            };
            if(await _orderDetailRepon.Create(orderDetail) == 1)
            {
                return await _orderRepon.Update(order);
            }
            else { return 3; }
            
        }
    }
}
