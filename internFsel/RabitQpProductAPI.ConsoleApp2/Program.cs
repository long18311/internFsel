// See https://aka.ms/new-console-template for more information
//Here we specify the Rabbit MQ Server. we use rabbitmq docker image and use it
//Ở đây chúng tôi chỉ định Máy chủ Rabbit MQ. chúng tôi sử dụng hình ảnh docker RabbitMQ và sử dụng nó
using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using System.Text;

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
//channel.QueueDeclare("product", exclusive: false);
//Set Event object which listen message from chanel which is sent by producer
//Đặt đối tượng Sự kiện nghe tin nhắn từ chanel được gửi bởi nhà sản xuất
//var consumer = new EventingBasicConsumer(channel);
channel.ExchangeDeclare(exchange: "pubsub", type: ExchangeType.Fanout);
channel.ExchangeDeclare(exchange: "pubsub-exchange", type: ExchangeType.Direct);

var queueName = channel.QueueDeclare().QueueName;

channel.QueueBind(queue: queueName, exchange: "pubsub", routingKey: "routingKey");

var consumer = new EventingBasicConsumer(channel);
consumer.Received += (model, eventArgs) =>
{
    var body = eventArgs.Body.ToArray();
    var message = Encoding.UTF8.GetString(body);
    Console.WriteLine($"Product message received có routingKey: {message}");
};
//read the message
//Đọc tin nhắnchannel.ExchangeDeclare(exchange: "pubsub-exchange", type: ExchangeType.Direct);
channel.QueueDeclare("pubsub-queue", durable: true, exclusive: false, autoDelete: false, arguments: null);
channel.QueueBind("pubsub-queue", "pubsub-exchange", "account.init");
/*var queueName = channel.QueueDeclare().QueueName;

channel.QueueBind(queue: queueName, exchange: "pubsub", routingKey: "routingKey");*/

var consumerr = new EventingBasicConsumer(channel);
consumerr.Received += (model, eventArgs) =>
{
    var body = eventArgs.Body.ToArray();
    var message = Encoding.UTF8.GetString(body);
    Console.WriteLine($"Product message received có routingKey: {message}");
};
//read the message
//Đọc tin nhắn
channel.BasicConsume(queue: "pubsub-queue", autoAck: true, consumer: consumerr);
channel.BasicConsume(queue: queueName, autoAck: true, consumer: consumer);
Console.ReadKey();
