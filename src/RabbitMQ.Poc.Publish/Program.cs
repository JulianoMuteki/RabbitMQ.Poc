using RabbitMQ.Client;
using System;
using System.Text;
using System.Threading;

namespace RabbitMQ.Poc.Publish
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
                Password = "guest",
            };

            using (var connection = connectionFactory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                while (true)
                {                    
                    var teste = $"Sending.... test - " + 
                                $"{DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss")}";
                    Console.WriteLine(teste);

                    Thread.Sleep(5000);

                    channel.QueueDeclare(
                        queue: "teste2",
                        durable: false,
                        exclusive: false,
                        autoDelete: false,
                        arguments: null);

                    string message =
                        $"{DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss")} - " +
                        $"Text Message example: {teste}";
                    var body = Encoding.UTF8.GetBytes(message);

                    channel.BasicPublish(exchange: "",
                                         routingKey: "teste2",
                                         basicProperties: null,
                                         body: body);


                }
            }
        }
    }
}
