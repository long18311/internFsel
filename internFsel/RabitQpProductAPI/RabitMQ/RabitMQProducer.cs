using Newtonsoft.Json;
using RabbitMQ.Client;
using RabitQpProductAPI.Models;
using System.Text;

namespace RabitQpProductAPI.RabitMQ
{
    public class RabitMQProducer : IRabitMQProducer
    {
        public void SendProductMessage<T>(Product message)
        {
            //Here we specify the Rabbit MQ Server. we use rabbitmq docker image and use it
            //Ở đây chúng tôi chỉ định Máy chủ Rabbit MQ. chúng tôi sử dụng hình ảnh docker RabbitMQ và sử dụng nó
            var factory = new ConnectionFactory
            {
                HostName = "localhost"
            };
            //Create the RabbitMQ connection using connection factory details as i mentioned above
            //Tạo kết nối RabbitMQ bằng cách sử dụng chi tiết nhà máy kết nối như tôi đã đề cập ở trên
            var connection = factory.CreateConnection();
            //Here we create channel with session and model
            //Ở đây chúng tôi tạo kênh với phiên và mô hình
            using var channel = connection.CreateModel();
            //declare the queue after mentioning name and a few property related to that
            //khai báo hàng đợi sau khi đề cập đến tên và một vài thuộc tính liên quan đến điều đó

            /*channel.QueueDeclare("product", exclusive: false);*/

            if (message.ProductStock == 0)
            {
                channel.ExchangeDeclare(exchange: "pubsub-exchange", type: ExchangeType.Direct);
                channel.BasicQos(0, 10, false);
                //Serialize the message
                //Tuần tự hóa tin nhắn
                var json = JsonConvert.SerializeObject(message);

                var body = Encoding.UTF8.GetBytes(json);
                //put the data on to the product queue
                //đưa dữ liệu vào hàng đợi sản phẩm
                channel.BasicPublish(exchange: "pubsub-exchange", routingKey: "account.init", body: body);
            }
            else {
                channel.ExchangeDeclare(exchange: "pubsub", type: ExchangeType.Fanout);
                //Serialize the message
                //Tuần tự hóa tin nhắn
                var json = JsonConvert.SerializeObject(message);
                var body = Encoding.UTF8.GetBytes(json);
                //put the data on to the product queue
                //đưa dữ liệu vào hàng đợi sản phẩm
                channel.BasicPublish(exchange: "pubsub", routingKey: "routingKey", body: body);
            }
        }
    }
}
