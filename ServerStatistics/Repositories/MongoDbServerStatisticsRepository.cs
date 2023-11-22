using GlobalConfigurations;
using MongoDB.Driver;
using ServerStatistics.Models;

namespace ServerStatistics.Repositories
{
    public class MongoDbServerStatisticsRepository : IServerStatisticsRepository
    {
        private AppConfigurations _config = new();
        private MongoClient _client;
        private IMongoDatabase _database;
        private IMongoCollection<ServerStatisticsWithServerIdentifierDTO> _serverStatistics;

        public MongoDbServerStatisticsRepository()
        {
            _client = new MongoClient(_config.MongoDbConfig.ConnectionString);
            _database = _client.GetDatabase("ServerMonitoring");

            _serverStatistics = _database.
                GetCollection<ServerStatisticsWithServerIdentifierDTO>("ServerStatistics");
        }

        public async Task InsertAsync(ServerStatisticsWithServerIdentifierDTO serverStatistics)
        {
            await _serverStatistics.InsertOneAsync(serverStatistics);
        }
    }
}
