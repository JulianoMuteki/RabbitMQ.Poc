using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;

namespace RabbitMQ.Poc.Consumer
{
    class Program
    {
        static void Main(string[] args)
        {
            var connectionFactory = new ConnectionFactory()
            {
                HostName = "localhost",
                Port = 5672,
                UserName = "guest",
                Password = "guest"
            };

            using (var connection = connectionFactory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(
                    queue: "ctrlinvest",
                    durable: false,
                    exclusive: false,
                    autoDelete: false,
                    arguments: null);

                var consumer = new EventingBasicConsumer(channel);

                consumer.Received += (sender, eventArgs) =>
                {
                    var body = eventArgs.Body.ToArray();
                    var message = Encoding.UTF8.GetString(body);                    

                    Console.WriteLine(Environment.NewLine + "[New message received] " + message);
                };

                channel.BasicConsume(queue: "ctrlinvest",
                     autoAck: true,
                     consumer: consumer);
                Console.WriteLine(Environment.NewLine);
                Console.WriteLine("Waiting messages to proccess");
                Console.WriteLine("Press some key to exit...");
                Console.ReadKey();
            }
        }
    }
}
