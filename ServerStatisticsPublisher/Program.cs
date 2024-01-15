using ServerStatistics;
using MessageQueue;
using GlobalConfigurations;
using ServerStatistics.Models;

var serverStatisticsCollector = new ServerStatisticsCollector();
var config = new AppConfigurations();
var samplingInterval = config.ServerStatisticsConfig.SamplingIntervalSeconds;
var serverIdentifier = config.ServerStatisticsConfig.ServerIdentifier;
var connectionName = config.RabbitMqConfig.PublisherConnectionName;
var exchangeName = config.RabbitMqConfig.ExchangeName;
var queueName = config.RabbitMqConfig.QueueName;
var routingKey = $"{queueName}.{serverIdentifier}";
var reconnectingIntervalSeconds = config.RabbitMqConfig.ReconnectIntervalSeconds;

Console.WriteLine(
    "This service is responsible for sending the server statistics to a message queue.\n");

Console.WriteLine("Connecting...");
TopicMessageSender messageSender = null;
ServerStatisticsDTO currentServerStatistics = null;

while (true) {

    try
    {
        messageSender = new TopicMessageSender(
            connectionName,
            exchangeName,
            queueName,
            routingKey,
            config);

        currentServerStatistics = serverStatisticsCollector.CurrentServerStatistics;
        messageSender.Send(currentServerStatistics);
    }
    catch
    {
        Console.WriteLine("\nConnection to RabbitMQ failed!");
        Thread.Sleep((int)(reconnectingIntervalSeconds * 1000));
        Console.WriteLine("Reconnecting...");
        
        continue;
    }

    Console.WriteLine(currentServerStatistics);

    Thread.Sleep(TimeSpan.FromSeconds(samplingInterval));
}
