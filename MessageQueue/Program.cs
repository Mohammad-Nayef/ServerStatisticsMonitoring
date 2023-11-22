using MessageQueue;
using ServerStatistics.Services;
using ServerStatistics.Repositories;
using SignalREndpoint;

Console.WriteLine(
    "This service is responsible for consuming server statistics from the message " +
    "queue and sending anomaly alerts through SignalR\n");

Console.WriteLine("Connecting...");

while (true)
{
    try
    {
        var messageConsumer = new ServerStatisticsReceiver(
                "Message consuming",
                "Statistics exchange",
                "ServerStatistics",
                "ServerStatistics.*",
                new ServerStatisticsService(new MongoDbServerStatisticsRepository()),
                new SignalRAlertSender()
                );

        messageConsumer.Consume();

        Console.WriteLine("Connected successfully.");
        Console.ReadLine();
    }
    catch
    {
        Console.WriteLine("\nConnection to RabbitMQ failed!");
        Thread.Sleep(2000);
        Console.WriteLine("Reconnecting...");

        continue;
    }

    break;
}
