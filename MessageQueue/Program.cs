using MessageQueue;
using ServerStatistics.Services;
using ServerStatistics.Repositories;
using SignalREndpoint;
using GlobalConfigurations;

Console.WriteLine(
    "This service is responsible for consuming server statistics from the message " +
    "queue and sending anomaly alerts through SignalR\n");

Console.WriteLine("Connecting...");

var config = new AppConfigurations();
var connectionName = config.RabbitMqConfig.ConsumerConnectionName;
var exchangeName = config.RabbitMqConfig.ExchangeName;
var queueName = config.RabbitMqConfig.QueueName;
var routingKey = $"{queueName}.*";
var reconnectingIntervalSeconds = config.RabbitMqConfig.ReconnectIntervalSeconds;

while (true)
{
    try
    {
        var messageConsumer = new ServerStatisticsReceiver(
            connectionName, 
            exchangeName, 
            queueName, 
            routingKey,
            new ServerStatisticsService(new MongoDbServerStatisticsRepository(config)),
            config,
            new ServerAnomaliesService(config, new SignalRAlertSender(config))
            );

        messageConsumer.Consume();

        Console.WriteLine("Connected successfully.");
        Console.ReadLine();
    }
    catch
    {
        Console.WriteLine("\nConnection to RabbitMQ failed!");
        Thread.Sleep((int)(reconnectingIntervalSeconds * 1000));
        Console.WriteLine("Reconnecting...");

        continue;
    }

    break;
}
