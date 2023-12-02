using GlobalConfigurations;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using ServerStatistics.Extensions;
using ServerStatistics.Models;
using ServerStatistics.Services;
using SignalREndpoint;

namespace MessageQueue
{
    public class ServerStatisticsReceiver : RabbitMqConnector, IMessageReceiver
    {
        private string _queueName;
        private IServerStatisticsService _statisticsService;
        private ServerStatisticsDTO _serverStatistics;
        private IAlertSender _alertSender;
        private bool _hasPreviousStatistics = false;
        private ServerStatisticsDTO _previousStatistics;

        public ServerStatisticsReceiver(
            string connectionName,
            string exchangeName,
            string queueName,
            string routingKey,
            IServerStatisticsService statisticsService,
            IAlertSender alertSender,
            IAppConfigurations config) :
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
            _alertSender = alertSender;
        }

        public void Consume()
        {
            var consumer = new EventingBasicConsumer(_channel);

            consumer.Received += async (sender, args) =>
            {
                try
                {
                    _serverStatistics = args.GetServerStatistics();

                    await DetectAndReportAnomaliesAsync();
                    await PersistToDatabaseAsync(args);

                    _channel.BasicAck(args.DeliveryTag, false);
                }
                catch { }
            };

            _channel.BasicConsume(_queueName, false, consumer);
        }

        private async Task PersistToDatabaseAsync(BasicDeliverEventArgs args)
        {
            await _statisticsService.InsertAsync(GetServerStatisticsWithServerIdentifier(args));
        }

        private ServerStatisticsWithServerIdentifierDTO GetServerStatisticsWithServerIdentifier(
            BasicDeliverEventArgs args)
        {
            var serverIdentifier = args.RoutingKey
                .Split('.')
                .LastOrDefault();

            return _serverStatistics.IncludeServerIdentifier(serverIdentifier);
        }

        private async Task DetectAndReportAnomaliesAsync()
        {
            if (_hasPreviousStatistics)
            {
                if (_serverStatistics.HasSuddenMemoryUsageIncrease(_previousStatistics))
                    await _alertSender.SendAsync("Memory usage anomaly alert");

                if (_serverStatistics.HasSuddenCpuUsageIncrease(_previousStatistics))
                    await _alertSender.SendAsync("CPU usage anomaly alert");
            }
            else
            {
                _hasPreviousStatistics = true;
            }
            
            _previousStatistics = _serverStatistics;

            if (_serverStatistics.MemoryUsageExceededThreshold())
                await _alertSender.SendAsync("High memory usage alert");

            if (_serverStatistics.CpuUsageExceededThreshold())
                await _alertSender.SendAsync("High CPU usage alert");
        }
    }
}
