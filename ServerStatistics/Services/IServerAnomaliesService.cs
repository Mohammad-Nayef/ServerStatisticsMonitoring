using ServerStatistics.Models;

namespace ServerStatistics.Services
{
    public interface IServerAnomaliesService
    {
        Task DetectAndReportAsync(ServerStatisticsDTO serverStatistics);
    }
}