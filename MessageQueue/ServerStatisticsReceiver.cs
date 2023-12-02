using GlobalConfigurations;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using ServerStatistics.Extensions;
using ServerStatistics.Models;
using ServerStatistics.Services;

namespace MessageQueue
{
    public class ServerStatisticsReceiver : RabbitMqConnector, IMessageReceiver
    {
        private string _queueName;
        private IServerStatisticsService _statisticsService;
        private ServerStatisticsDTO _serverStatistics;
        private readonly IServerAnomaliesService _serverAnomaliesDetection;

        public ServerStatisticsReceiver(
            string connectionName,
            string exchangeName,
            string queueName,
            string routingKey,
            IServerStatisticsService statisticsService,
            IAppConfigurations config,
            IServerAnomaliesService serverAnomaliesDetection) :
            base(
                connectionName, 
                exchangeName, 
                ExchangeType.Topic, 
                queueName, 
                routingKey,
                config)
        {
            _queueName = queueName;
            _statisticsService = statisticsService;
            _serverAnomaliesDetection = serverAnomaliesDetection;
        }

        public void Consume()
        {
            var consumer = new EventingBasicConsumer(_channel);

            consumer.Received += async (sender, args) =>
            {
                try
                {
                    _serverStatistics = args.GetServerStatistics();
                    await _serverAnomaliesDetection.DetectAndReportAsync(_serverStatistics);
                    await PersistToDatabaseAsync(args);

                    _channel.BasicAck(args.DeliveryTag, false);
                }
                catch { }
            };

            _channel.BasicConsume(_queueName, false, consumer);
        }

        private async Task PersistToDatabaseAsync(BasicDeliverEventArgs args)
        {
            await _statisticsService.InsertAsync(
                _serverStatistics.GetServerStatisticsWithServerIdentifier(args));
        }
    }
}
