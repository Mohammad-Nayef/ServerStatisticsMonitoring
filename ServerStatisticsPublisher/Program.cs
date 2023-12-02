using ServerStatistics;
using MessageQueue;
using GlobalConfigurations;
using ServerStatistics.Models;

var serverStatisticsCollector = new ServerStatisticsCollector();
var config = new AppConfigurations();
var samplingInterval = config.ServerStatisticsConfig.SamplingIntervalSeconds;
var serverIdentifier = config.ServerStatisticsConfig.ServerIdentifier;

Console.WriteLine(
    "This service is responsible for sending the server statistics to a message queue.\n");

Console.WriteLine("Connecting...");
TopicMessageSender messageSender = null;
ServerStatisticsDTO currentServerStatistics = null;

while (true) {

    try
    {
        messageSender = new TopicMessageSender(
            "Message sending",
            "Statistics exchange",
            "ServerStatistics",
            $"ServerStatistics.{serverIdentifier}",
            config);

        currentServerStatistics = serverStatisticsCollector.CurrentServerStatistics;
        messageSender.Send(currentServerStatistics);
    }
    catch
    {
        Console.WriteLine("\nConnection to RabbitMQ failed!");
        Thread.Sleep(2000);
        Console.WriteLine("Reconnecting...");
        
        continue;
    }


    Console.WriteLine(currentServerStatistics);

    Thread.Sleep(TimeSpan.FromSeconds(samplingInterval));
}
