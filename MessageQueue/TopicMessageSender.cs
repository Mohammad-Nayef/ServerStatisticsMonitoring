using System.Text;
using System.Text.Json;
using RabbitMQ.Client;

namespace MessageQueue
{
    public class TopicMessageSender : RabbitMqConnector, IMessageSender
    {
        private string _exchangeName;
        private string _routingKey;

        public TopicMessageSender(
            string connectionName, 
            string exchangeName,
            string queueName, 
            string routingKey) : 
            base(connectionName, exchangeName, ExchangeType.Topic, queueName, routingKey)
        {
            _exchangeName = exchangeName;
            _routingKey = routingKey;
        }

        public void Send<T>(T message)
        {
            var stringMessage = JsonSerializer.Serialize(message);
            var bytesMessage = Encoding.UTF8.GetBytes(stringMessage);
            _channel.BasicPublish(_exchangeName, _routingKey, null, bytesMessage);
        }
    }
}
