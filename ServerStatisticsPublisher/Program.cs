using ServerStatistics;
using MessageQueue;
using GlobalConfigurations;

var serverStatistics = new ServerStatisticsCollector();
var config = new AppConfigurations();
var samplingInterval = config.ServerStatisticsConfig.SamplingIntervalSeconds;
var serverIdentifier = config.ServerStatisticsConfig.ServerIdentifier;

Console.WriteLine(
    "This service is responsible for sending the server statistics to a message queue.\n");

Console.WriteLine("Connecting...");
TopicMessageSender? messageSender = null;

while (true) {

    try
    {
        messageSender = new TopicMessageSender(
            "Message sending",
            "Statistics exchange",
            "ServerStatistics",
            $"ServerStatistics.{serverIdentifier}",
            config);
        
        messageSender.Send(serverStatistics.CurrentServerStatistics);
    }
    catch
    {
        Console.WriteLine("\nConnection to RabbitMQ failed!");
        Thread.Sleep(2000);
        Console.WriteLine("Reconnecting...");
        
        continue;
    }


    Console.WriteLine($"\nMemory Usage: {serverStatistics.CurrentServerStatistics.MemoryUsage}");
    Console.WriteLine($"Available Memory: " +
        $"{serverStatistics.CurrentServerStatistics.AvailableMemory}");
    Console.WriteLine($"CPU Usage: {serverStatistics.CurrentServerStatistics.CpuUsage}%");
    Console.WriteLine($"Timestamp: {serverStatistics.CurrentServerStatistics.Timestamp}");

    Thread.Sleep(TimeSpan.FromSeconds(samplingInterval));
}
