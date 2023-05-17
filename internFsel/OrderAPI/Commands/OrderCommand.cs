using MediatR;
using OrderAPI.ViewModel.Order;
using OrderAPI.ViewModel.OrderDetail;

namespace OrderAPI.Commands
{
    public class OrderCommand
    {
        public record CreateOrderCommand(CreateOrder model) : IRequest<int>;
        public record UpdateOrderCommand(UpdateOrder model) : IRequest<int>;
        public record DeleteOrderCommand(Guid id) : IRequest<int>;
    }
}
