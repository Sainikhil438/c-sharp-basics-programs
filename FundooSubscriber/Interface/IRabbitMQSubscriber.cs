using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;

namespace FundooSubscriber.Interface
{
    public interface IRabbitMQSubscriber
    {
        public void PublishMessage(string queueName, string message);
    }
}
