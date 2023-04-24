using OrderAPI.ViewModel.Order;

namespace OrderAPI.repositories.IRepon
{
    public interface IOrderRepon
    {
        Task<int> Create(CreateOrder model);
    }
}
