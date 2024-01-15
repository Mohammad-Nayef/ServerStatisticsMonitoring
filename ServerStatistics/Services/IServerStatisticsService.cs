using ServerStatistics.Models;

namespace ServerStatistics.Services
{
    public interface IServerStatisticsService
    {
        Task InsertAsync(ServerStatisticsWithServerIdentifierDTO serverStatistics);
    }
}
