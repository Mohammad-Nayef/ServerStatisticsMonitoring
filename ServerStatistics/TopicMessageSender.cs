using System.Text;
using System.Text.Json;
using RabbitMQ.Client;

namespace ServerStatistics
{
    public class TopicMessageSender : RabbitMqConnector
    {
        private string _exchangeName;
        private string _routingKey;

        public TopicMessageSender(string connectionName, string exchangeName,
            string queueName, string routingKey) : base(connectionName)
        {
            _channel.ExchangeDeclare(exchangeName, ExchangeType.Topic);
            _channel.QueueDeclare(queueName, false, false, false);
            _channel.QueueBind(queueName, exchangeName, routingKey);
            _exchangeName = exchangeName;
            _routingKey = routingKey;
        }

        public void Publish<T>(T message)
        {
            var stringMessage = JsonSerializer.Serialize(message);
            var bytesMessage = Encoding.UTF8.GetBytes(stringMessage);
            _channel.BasicPublish(_exchangeName, _routingKey, null, bytesMessage);
        }
    }
}
