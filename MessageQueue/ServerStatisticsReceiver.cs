using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text.Json;
using ServerStatistics.Models;
using ServerStatistics.Services;
using ServerStatistics.Extensions;
using SignalREndpoint;

namespace MessageQueue
{
    public class ServerStatisticsReceiver : RabbitMqConnector
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
            IAlertSender alertSender) : base(connectionName)
        {
            _channel.ExchangeDeclare(exchangeName, ExchangeType.Topic);
            _channel.QueueDeclare(queueName, false, false, false);
            _channel.QueueBind(queueName, exchangeName, routingKey);
            _queueName = queueName;
            _statisticsService = statisticsService;
            _alertSender = alertSender;
        }

        public void Consume()
        {
            var consumer = new EventingBasicConsumer(_channel);

            consumer.Received += async (sender, args) =>
            {
                _serverStatistics = GetServerStatistics(args);

                await DetectAndReportAnomaliesAsync();
                await PersistToDatabaseAsync(args);

                _channel.BasicAck(args.DeliveryTag, false);
            };

            _channel.BasicConsume(_queueName, false, consumer);
        }

        private ServerStatisticsDTO? GetServerStatistics(BasicDeliverEventArgs args)
        {
            var bytesMessage = args.Body.ToArray();
            var stringMessage = Encoding.UTF8.GetString(bytesMessage);
            
            return JsonSerializer.Deserialize<ServerStatisticsDTO>(stringMessage);
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
                _previousStatistics = _serverStatistics;
                _hasPreviousStatistics = true;
            }

            if (_serverStatistics.MemoryUsageExceededThreshold())
                await _alertSender.SendAsync("High memory usage alert");

            if (_serverStatistics.CpuUsageExceededThreshold())
                await _alertSender.SendAsync("High CPU usage alert");
        }
    }
}