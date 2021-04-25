using System;
using System.Text;
using System.Text.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Receive
{
    class Program
    {
        static void Main(string[] args)
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: "sum",
                                     durable: false,
                                     exclusive: false,
                                     autoDelete: false,
                                     arguments: null);

                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += (model, ea) =>
                {
                    var body = ea.Body.ToArray();
                    var message = Encoding.UTF8.GetString(body);
                    CalcModel calc = JsonSerializer.Deserialize<CalcModel>(body);
                    Console.WriteLine(" [x] Received {0}", JsonSerializer.Serialize(calc));
                };

                channel.BasicConsume(queue: "sum",
                                     autoAck: true,
                                     consumer: consumer);

                Console.WriteLine(" Press [enter] to exit.");
                Console.ReadLine();
            }
        }
    }

    class CalcModel
    {
        public CalcModel(int num1, int num2)
        {
            Num1 = num1;
            Num2 = num2;
        }
        public int Num1 { get; init; }
        public int Num2 { get; init; }
        public int Result 
        { 
            get
            {
                return Num1 + Num2;
            } 
        }
    }
}
