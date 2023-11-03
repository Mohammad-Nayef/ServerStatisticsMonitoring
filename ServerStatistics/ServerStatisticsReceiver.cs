using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text.Json;

namespace ServerStatistics
{
    public class ServerStatisticsReceiver : RabbitMqConnector
    {
        private string _queueName;

        public ServerStatisticsReceiver(string connectionName, string exchangeName,
            string queueName, string routingKey) : base(connectionName)
        {
            _channel.ExchangeDeclare(exchangeName, ExchangeType.Topic);
            _channel.QueueDeclare(queueName, false, false, false);
            _channel.QueueBind(queueName, exchangeName, routingKey);
            _queueName = queueName;
        }

        public void Consume()
        {
            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (sender, args) =>
            {
                var bytesMessage = args.Body.ToArray();
                var stringMessage = Encoding.UTF8.GetString(bytesMessage);
                var serverStatistics =
                    JsonSerializer.Deserialize<ServerStatisticsDTO>(stringMessage);
                MongoDb.Instance.Add(serverStatistics);
                _channel.BasicAck(args.DeliveryTag, false);
            };

            _channel.BasicConsume(_queueName, false, consumer);
        }
    }
}