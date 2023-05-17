using MediatR;
using OrderAPI.Models;
using OrderAPI.repositories.IRepon;
using static OrderAPI.Queries.OrderQuery;

namespace OrderAPI.Handlers
{
    public class GetOrderByIdHandler : IRequestHandler<GetOrderByIdQuery, Order>
    {
        private readonly IOrderRepon _orderRepon;
        public GetOrderByIdHandler(IOrderRepon orderRepon) {
            _orderRepon = orderRepon;
        }
        public async Task<Order> Handle(GetOrderByIdQuery request, CancellationToken cancellationToken)
        {
            var order = await _orderRepon.GetOrderById(request.id);
            if(order == null) { return null; }
            List<OrderDetail> orderDetails = await _orderRepon.GetOrderDetailbyId(request.id);
            order.OrderDetails = orderDetails;
            return order;
        }
    }
}
