using System.Text;
using System.Text.Json;
using GlobalConfigurations;
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
            string routingKey,
            IAppConfigurations config) : 
            base(
                connectionName, 
                exchangeName, 
                ExchangeType.Topic, 
                queueName, 
                routingKey,
                config)
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
