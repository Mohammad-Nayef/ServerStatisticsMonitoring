using MessageQueue;
using ServerStatistics.Services;
using ServerStatistics.Repositories;
using SignalREndpoint;

Console.WriteLine(
    "This service is responsible for consuming server statistics from the message " +
    "queue and sending anomaly alerts through SignalR\n");

var messageConsumer = new ServerStatisticsReceiver(
        "Message consuming",
        "Statistics exchange",
        "ServerStatistics",
        "ServerStatistics.*",
        new ServerStatisticsService(new MongoDbServerStatisticsRepository()),
        new SignalRAlertSender()
        );

messageConsumer.Consume();

Console.ReadLine();