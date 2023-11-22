using ServerStatistics.Models;

namespace ServerStatistics.Repositories
{
    public interface IServerStatisticsRepository
    {
        Task InsertAsync(ServerStatisticsWithServerIdentifierDTO serverStatistics);
    }
}
