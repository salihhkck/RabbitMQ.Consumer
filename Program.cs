using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace RabbitMQ.Consumer;

internal class Program
{
    static void Main(string[] args)
    {
        //Bağlantı Oluşturma RabbitMQ ile.
        ConnectionFactory connectionFactory = new();
        connectionFactory.Uri = new("amqps://rkjhqugu:ldW9uYt3EtaPRzn9_ulmFp657F27TyNA@jackal.rmq.cloudamqp.com/rkjhqugu");


        //Bağlantı Aktifleştirme
        using IConnection connection = connectionFactory.CreateConnection();
        using IModel channel= connection.CreateModel();

        //1.adım
        channel.ExchangeDeclare(exchange: "direct-exchange-example", type: ExchangeType.Direct);

        //2.adım
        string queueName = channel.QueueDeclare().QueueName;

        //3.adım
        channel.QueueBind(queue: queueName,
           exchange: "direct-exchange-example",
           routingKey: "direct-exchange-example");



        EventingBasicConsumer consumer = new(channel);
        channel.BasicConsume(queue:queueName,autoAck:true,consumer:consumer);
        consumer.Received += (sender, e) =>
        {
            string message = Encoding.UTF8.GetString(e.Body.Span);
            Console.WriteLine(message);
        };

        Console.Read();
    }
}
