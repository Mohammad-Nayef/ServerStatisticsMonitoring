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

        public RabbitMqConnector(string connectionName)
        {
            _factory.HostName = _config.RabbitMqConfig.HostName;
            _factory.ClientProvidedName = connectionName;
            _connection = _factory.CreateConnection();
            _channel = _connection.CreateModel();
        }
    }
}
