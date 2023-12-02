using ServerStatistics.Repositories;
using ServerStatistics.Models;

namespace ServerStatistics.Services
{
    public class ServerStatisticsService : IServerStatisticsService
    {
        private IServerStatisticsRepository _repository;

        public ServerStatisticsService(IServerStatisticsRepository repository)
        {
            _repository = repository;
        }

        public async Task InsertAsync(ServerStatisticsWithServerIdentifierDTO serverStatistics)
        {
            await _repository.InsertAsync(serverStatistics);
        }
    }
}
