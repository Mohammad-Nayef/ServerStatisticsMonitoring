using GlobalConfigurations;
using RabbitMQ.Client;

namespace MessageQueue
{
    public abstract class RabbitMqConnector
    {
        private AppConfigurations _config = new();
        private ConnectionFactory _factory = new();
        private IConnection _connection;
        protected IModel _channel;

        public RabbitMqConnector(
            string connectionName, 
            string exchangeName, 
            string topic, 
            string queueName, 
            string routingKey)
        {
            _factory.HostName = _config.RabbitMqConfig.HostName;
            _factory.ClientProvidedName = connectionName;
            _connection = _factory.CreateConnection();
            _channel = _connection.CreateModel();
            _channel.ExchangeDeclare(exchangeName, topic);
            _channel.QueueDeclare(queueName, false, false, false);
            _channel.QueueBind(queueName, exchangeName, routingKey);
        }
    }
}
