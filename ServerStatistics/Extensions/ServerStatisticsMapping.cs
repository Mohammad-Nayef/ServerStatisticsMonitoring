using ServerStatistics.Models;

namespace ServerStatistics.Extensions
{
    public static class ServerStatisticsMapping
    {
        public static ServerStatisticsWithServerIdentifierDTO IncludeServerIdentifier(
            this ServerStatisticsDTO serverStatistics, string serverIdentifier)
        {
            return new ServerStatisticsWithServerIdentifierDTO
            {
                AvailableMemory = serverStatistics.AvailableMemory,
                CpuUsage = serverStatistics.CpuUsage,
                MemoryUsage = serverStatistics.MemoryUsage,
                Timestamp = serverStatistics.Timestamp,
                ServerIdentifier = serverIdentifier
            };
        }
    }
}
