using CommonLayer.Models;
using FundooSubscriberApp.Interface;
using MassTransit;
using MassTransit.Context;
using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace FundooSubscriberApp.Services
{
    public class RabbitMQSubscriber : IRabbitMQSubscriber
    {
        private readonly ConnectionFactory factory;
        private readonly IConfiguration configuration;
        private readonly IBusControl _busControl;

        public RabbitMQSubscriber(ConnectionFactory _factory, IConfiguration _configuration, IBusControl busControl)
        {
            factory = _factory;
            configuration = _configuration;
            _busControl = busControl;

            ConsumeMessages();
        }
        public void ConsumeMessages()
        {
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                var queueName = "User-Registration-Queue";
                channel.QueueDeclare(queue: queueName, durable: false, exclusive: false, autoDelete: false, arguments: null);

                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += async (model, ea) =>
                {
                    var body = ea.Body.ToArray();
                    var message = Encoding.UTF8.GetString(body);

                    await _busControl.Publish<UserRegistrationMessage>(new
                    {
                        Email = message
                    });
                };

                channel.BasicConsume(queue: queueName, autoAck: true, consumer: consumer);
            }
        }
    }
}
