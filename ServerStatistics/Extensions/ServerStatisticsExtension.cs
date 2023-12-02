using ServerStatistics.Models;
using System.Text.Json;
using System.Text;
using RabbitMQ.Client.Events;

namespace ServerStatistics.Extensions
{
    public static class ServerStatisticsExtension
    {
        public static ServerStatisticsDTO GetServerStatistics(this BasicDeliverEventArgs args)
        {
            var bytesMessage = args.Body.ToArray();
            var stringMessage = Encoding.UTF8.GetString(bytesMessage);

            return JsonSerializer.Deserialize<ServerStatisticsDTO>(stringMessage);
        }

        public static ServerStatisticsWithServerIdentifierDTO 
            GetServerStatisticsWithServerIdentifier(
            this ServerStatisticsDTO serverStatistics, BasicDeliverEventArgs args)
        {
            var serverIdentifier = args.RoutingKey
                .Split('.')
                .LastOrDefault();

            return serverStatistics.IncludeServerIdentifier(serverIdentifier);
        }
    }
}
