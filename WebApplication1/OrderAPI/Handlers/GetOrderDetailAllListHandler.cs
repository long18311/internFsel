using MediatR;
using OrderAPI.Models;
using OrderAPI.repositories.IRepon;
using static OrderAPI.Queries.OrderDetailQuery;

namespace OrderAPI.Handlers
{
    public class GetOrderDetailAllListHandler : IRequestHandler<GetOrderDetailAllListQuery, List<OrderDetail>>
    {
        private readonly IOrderDetailRepon _orderDetailRepon;
        public GetOrderDetailAllListHandler(IOrderDetailRepon orderDetailRepon)
        {
            _orderDetailRepon = orderDetailRepon;
        }

        public async Task<List<OrderDetail>> Handle(GetOrderDetailAllListQuery request, CancellationToken cancellationToken)
        {
            return await _orderDetailRepon.GetAll();
        }
    }
}
