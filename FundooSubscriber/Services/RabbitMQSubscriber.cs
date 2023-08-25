using FundooSubscriber.Interface;
using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;
using System.Text;

namespace FundooSubscriber.Services
{
    public class RabbitMQSubscriber : IRabbitMQSubscriber
    {
        private readonly ConnectionFactory factory;
        private readonly IConfiguration configuration;

        public RabbitMQSubscriber(ConnectionFactory _factory, IConfiguration _configuration)
        {
            factory = _factory;
            configuration = _configuration;
        }

        public void PublishMessage(string queueName, string message)
        {
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: queueName,durable: false, exclusive: false, autoDelete: false,arguments: null);

                var messageBytes = Encoding.UTF8.GetBytes(message);
                channel.BasicPublish(exchange:"", routingKey: queueName, basicProperties: null, body: messageBytes);
            }
        }
    }
}
