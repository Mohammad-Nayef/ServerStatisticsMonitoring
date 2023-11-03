using RabbitMQ.Client;

namespace ServerStatistics
{
    public abstract class RabbitMqConnector
    {
        private ConnectionFactory _factory = new();
        private IConnection _connection;
        protected IModel _channel;

        public RabbitMqConnector(string connectionName)
        {
            _factory.HostName = "localhost";
            _factory.ClientProvidedName = connectionName;
            _connection = _factory.CreateConnection();
            _channel = _connection.CreateModel();
        }

        ~RabbitMqConnector()
        {
            _connection.Dispose();
            _channel.Dispose();
        }
    }
}
