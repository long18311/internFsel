using RabitQpProductAPI.Models;

namespace RabitQpProductAPI.RabitMQ
{
    public interface IRabitMQProducer
    {
        public void SendProductMessage<T>(Product message);
    }
}
