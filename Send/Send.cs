using System;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using RabbitMQ.Client;

namespace Send
{
    class Send
    {
        static void Main(string[] args)
        {
            try
            {
                ConnectionFactory factory = new ConnectionFactory() { HostName = "localhost"};

                using (var connection = factory.CreateConnection())
                {
                    using (var channel = connection.CreateModel())
                    {
                        channel.QueueDeclare(
                            queue: "sum",
                            durable: false,
                            exclusive: false,
                            autoDelete: false,
                            arguments: null);

                        var calc = new CalcModel(1, 2);

                        string message = JsonSerializer.Serialize(calc);

                        var body = Encoding.UTF8.GetBytes(message);

                        channel.BasicPublish(exchange: "", routingKey: "sum", basicProperties: null, body: body);

                        Console.WriteLine($"[x] Sent {message}");
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
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

    }
}