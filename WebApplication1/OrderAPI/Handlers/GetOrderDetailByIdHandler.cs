using MediatR;
using OrderAPI.Models;
using OrderAPI.repositories.IRepon;
using static OrderAPI.Queries.OrderDetailQuery;

namespace OrderAPI.Handlers
{
    public class GetOrderDetailByIdHandler : IRequestHandler<GetOrderDetailByIdQuery, OrderDetail>
    {
        private readonly IOrderDetailRepon _orderDetailRepon;
        public GetOrderDetailByIdHandler(IOrderDetailRepon orderDetailRepon) {
            _orderDetailRepon = orderDetailRepon;
        }
        public async Task<OrderDetail> Handle(GetOrderDetailByIdQuery request, CancellationToken cancellationToken)
        {
            return await _orderDetailRepon.GetOrderDetailById(request.id);
        }
    }
}
