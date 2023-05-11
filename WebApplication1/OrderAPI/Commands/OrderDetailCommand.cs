using MediatR;
using OrderAPI.ViewModel.OrderDetail;

namespace OrderAPI.Commands
{
    public class OrderDetailCommand
    {
        public record CreateOrderDetailCommand(CreateOrderDetail model) : IRequest<int>;
        public record UpdateOrderDetailCommand(UpdateOrderDetail model) : IRequest<int>;
        public record DeleteOrderDetailCommand(Guid id) : IRequest<int>;
    }
}
